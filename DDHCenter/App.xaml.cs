using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.ViewModels;
using DDHCenter.Properties;
using DDHCenter.Views;
using NamedPipeWrapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DDHCenter
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App
    {
        public MainView MainView = null;
        public LoginView LoginView = null;

        [STAThread]
        public static void Main(string[] args)
        {
            if (SingleInstance.IsSingleInstance())
            {
                var application = new App();
                application.InitializeComponent();
                SingleInstance.MyApp = application;
                if (args.Length > 0 && args[0] == "--nowindow")
                    application.Run();
                else
                    application.Run(application.GetMainWindow());
                //
                SingleInstance.Clean();
            }
        }

        public App()
        {
            // Set shutdown mode to manual
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private Window GetMainWindow()
        {
            //
            if (Settings.Default.UserToken != "")
            {
                //
                MainView = new MainView();
                MainView.DataContext = new MainViewModel(MainView);
                return MainView;
            }
            //
            LoginView = new LoginView();
            LoginView.DataContext = new LoginViewModel();
            return LoginView;
        }

        public void GetArguments(string[] args)
        {
            Current.Dispatcher.Invoke(() =>
            {
                if (Current.MainWindow == null)
                {
                    Current.MainWindow = GetMainWindow();
                }
                Current.MainWindow.GlobalActivate();
            });
        }
    }
    public class SingleInstance
    {
        private static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        private static NamedPipeServer<string> _server;
        private static NamedPipeClient<string> _client;
        public static App MyApp;

        public static void Clean()
        {
            mutex.ReleaseMutex();
        }

        public static bool IsSingleInstance()
		{
			bool isSingleInstance;
			if(mutex.WaitOne(TimeSpan.Zero, true)) {
				isSingleInstance = true;
				_server = new NamedPipeServer<string>("DDHCenterSingleApplicationV1");
				_server.ClientConnected += delegate(NamedPipeConnection<string, string> conn)
				{
					MyApp.GetArguments(new string[] {});
				};
				_server.Start();
			}
			else
			{
				isSingleInstance = false;
				_client = new NamedPipeClient<string>("DDHCenterSingleApplicationV1");
				_client.Start();
				Task.Delay(500).Wait();
			}
			
			return isSingleInstance;
		}
    }
}
