using System.Threading;
using System.Windows;
using TestWorkApp1.Services.SeederRunnerService;

namespace TestWorkApp1
{
    public class App : Application
    {
        readonly MainWindow _mainWindow;
        readonly SeederRunnerService _seederRunner;

        public App(
            MainWindow mainWindow,
            SeederRunnerService seederRunner
            )
        {
            _mainWindow = mainWindow;
            _seederRunner = seederRunner;
        }
        protected async override void OnStartup(StartupEventArgs e)
        {
            await _seederRunner.RunSeedersAsync();
            _mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
