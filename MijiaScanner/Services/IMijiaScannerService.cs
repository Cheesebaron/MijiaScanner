using MijiaScanner.Models;
using MvvmCross.ViewModels;
using System;

namespace MijiaScanner.Services
{
    public interface IMijiaScannerService
    {
        MvxObservableCollection<MijiaDevice> Devices { get; }
        DateTime LastFetchTime { get; }
        void StartScanning(TimeSpan scanInterval);
        void StopScanning();
    }
}
