using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using TelemetryGroundStation.Services;
using TelemetryGroundStation.ViewModels;
using TelemetryGroundStation.Views;

namespace TelemetryGroundStation
{
    public partial class App : Application
    {
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ITelemetrySource source = new TelemetrySimulator();
                var hub = new TelemetryHub(source);
                var vm = new MainWindowViewModel(hub);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
