using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MijiaScanner.Droid
{
    [Activity(
        Label = "Mijia Scanner"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/MainTheme"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}