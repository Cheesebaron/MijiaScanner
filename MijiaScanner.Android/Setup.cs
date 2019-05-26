using System.Collections.Generic;
using System.Reflection;
using MijiaScanner.Droid.Services;
using MijiaScanner.Services;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Logging;

namespace MijiaScanner.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override IEnumerable<Assembly> AndroidViewAssemblies =>
            new List<Assembly>(base.AndroidViewAssemblies)
            {
                typeof(MvxRecyclerView).Assembly
            };

        public override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Console;

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.IoCProvider.RegisterSingleton<IMijiaScannerService>(new MijiaScannerService());
        }
    }
}