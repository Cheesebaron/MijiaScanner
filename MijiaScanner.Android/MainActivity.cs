using Android.App;
using Android.Runtime;
using Android.OS;
using Plugin.Permissions;
using MijiaScanner.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Support.V7.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace MijiaScanner.Droid
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/MainTheme")]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            Title = "Mijia Scanner";
        }

        protected override void OnResume()
        {
            base.OnResume();

            ViewModel.StartScanningCommand.Execute();
        }

        protected override void OnPause()
        {
            base.OnPause();

            ViewModel.StopScanningCommand.Execute();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}