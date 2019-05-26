using MijiaScanner.ViewModels;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace MijiaScanner
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<MainViewModel>();
        }
    }
}
