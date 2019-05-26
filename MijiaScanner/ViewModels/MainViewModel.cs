using System;
using MijiaScanner.Models;
using MijiaScanner.Services;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MijiaScanner.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IMijiaScannerService _scannerService;

        public MvxObservableCollection<MijiaDevice> Devices { get; }

        public MainViewModel(IMijiaScannerService scannerService)
        {
            _scannerService = scannerService;
            Devices = _scannerService.Devices;
            StartScanningCommand = new MvxCommand(DoStartScanningCommand);
            StopScanningCommand = new MvxCommand(DoStopScanningCommand);
        }

        private void DoStopScanningCommand()
        {
            _scannerService.StopScanning();
        }

        private void DoStartScanningCommand()
        {
            _scannerService.StartScanning(TimeSpan.FromSeconds(20));
        }

        public MvxCommand StartScanningCommand { get; }
        public MvxCommand StopScanningCommand { get; }
    }
}