using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MijiaScanner.Models;
using MijiaScanner.Services;
using MvvmCross.ViewModels;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;

namespace MijiaScanner.Droid.Services
{
    public class MijiaScannerService : IMijiaScannerService
    {
        private TimeSpan _scanInterval;

        private CancellationTokenSource _scanningTokenSource;

        public MvxObservableCollection<MijiaDevice> Devices { get; } = new MvxObservableCollection<MijiaDevice>();

        public DateTime LastFetchTime { get; private set; }

        public void StartScanning(TimeSpan scanInterval)
        {
            _scanInterval = scanInterval;

            if (_scanningTokenSource != null)
            {
                _scanningTokenSource.Cancel();
                _scanningTokenSource.Dispose();
                _scanningTokenSource = null;
            }

            _scanningTokenSource = new CancellationTokenSource();

            Task.Run(() => Worker(_scanningTokenSource.Token), _scanningTokenSource.Token);
        }

        private async Task Worker(CancellationToken token)
        {
            var hasPermission = await RequestPermissions();

            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;
            adapter.DeviceDiscovered += OnDeviceDiscovered;

            token.Register(() =>
            {
                adapter.StopScanningForDevicesAsync();
            });

            while (!token.IsCancellationRequested)
            {
                if (ble.IsAvailable && ble.IsOn)
                {
                    adapter.ScanMode = ScanMode.LowLatency;
                    await adapter.StartScanningForDevicesAsync(null, DeviceFilter, cancellationToken: token);

                    LastFetchTime = DateTime.Now;
                }

                await Task.Delay(_scanInterval, token);
            }
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            var device = Devices.FirstOrDefault(d => d.DeviceId == e.Device.Id);
            var newDevice = false;
            if (device == null)
            {
                device = new MijiaDevice
                {
                    DeviceId = e.Device.Id,
                    Name = e.Device.Name
                };

                newDevice = true;
            }

            foreach(var record in e.Device.AdvertisementRecords)
            {
                System.Diagnostics.Debug.WriteLine($"{record.Type} : {string.Join(" ", record.Data.Select(d => $"{d:X2}"))}");

                if (record.Type == Plugin.BLE.Abstractions.AdvertisementRecordType.ServiceData)
                {
                    var (battery, temperature, humidity) = ReadServiceData(record.Data);

                    if (temperature.HasValue)
                        device.Temperature = temperature.Value;
                    if (humidity.HasValue)
                        device.Humidity = humidity.Value;

                    if (battery > 0)
                        device.Battery = battery;
                }
            }

            if (newDevice)
            {
                Devices.Add(device);
            }
        }

        private (double battery, double? temperature, double? humidity) ReadServiceData(byte[] data)
        {
            if (data.Length < 14) return (-1, null, null);

            double battery = -1;
            double? temp = null;
            double? humidity = null;

            if (data[13] == 0x04) //temp
            {
                temp = BitConverter.ToUInt16(new byte[] { data[16], data[17] }, 0) / 10.0;
            }
            else if (data[13] == 0x06) //humidity
            {
                humidity = BitConverter.ToUInt16(new byte[] { data[16], data[17] }, 0) / 10.0;
            }
            else if (data[13] == 0x0A) //battery
            {
                battery = data[16];
            }
            else if (data[13] == 0x0D) //temp + humidity
            {
                temp = BitConverter.ToUInt16(new byte[] { data[16], data[17] }, 0) / 10.0;
                humidity = BitConverter.ToUInt16(new byte[] { data[18], data[19] }, 0) / 10.0;
            }

            if (temp.HasValue)
                System.Diagnostics.Debug.WriteLine($"Temp: {temp.Value}");
            if (humidity.HasValue)
                System.Diagnostics.Debug.WriteLine($"Humidity: {humidity.Value}");

            System.Diagnostics.Debug.WriteLine($"Battery: {battery}");

            return (battery, temp, humidity);
        }

        private bool DeviceFilter(IDevice device)
        {
            if (device.Name?.StartsWith("MJ_HT_V1") ?? false)
                return true;

            return false;
        }

        public void StopScanning()
        {
            if (_scanningTokenSource != null)
            {
                _scanningTokenSource.Cancel();
                _scanningTokenSource.Dispose();
                _scanningTokenSource = null;
            }
        }

        private async Task<bool> RequestPermissions()
        {
            if (DeviceInfo.Platform != DevicePlatform.Android)
                return true;

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    var requestStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (requestStatus.ContainsKey(Permission.Location))
                    {
                        return requestStatus[Permission.Location] == PermissionStatus.Granted;
                    }
                }
            }
            catch (Exception ex)
            {
                //Something went wrong
            }

            return false;
        }
    }
}