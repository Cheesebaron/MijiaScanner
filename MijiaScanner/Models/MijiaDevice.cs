using MvvmCross.ViewModels;
using System;

namespace MijiaScanner.Models
{
    public class MijiaDevice : MvxNotifyPropertyChanged
    {
        private Guid _deviceId;
        private string _name;
        private double _temperature;
        private double _humidity;
        private double _battery;

        public Guid DeviceId
        {
            get => _deviceId;
            set => SetProperty(ref _deviceId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        public double Humidity
        {
            get => _humidity;
            set => SetProperty(ref _humidity, value);
        }

        public double Battery
        {
            get => _battery;
            set => SetProperty(ref _battery, value);
        }
    }
}
