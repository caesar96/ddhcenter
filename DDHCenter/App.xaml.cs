using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.ViewModels;
using DDHCenter.Properties;
using DDHCenter.Views;
using Microsoft.Shell;
using System;
using System.Windows;

namespace DDHCenter
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : ISingleInstanceApp
    {

        private const string Unique = "DDHCenterSingleApplicationV1.0.0.0";
        private bool IsAnyWindowShowed { get; set; }

        public MainView MainView = null;
        public LoginView LoginView = null;

        [STAThread]
        public static void Main(string[] args)
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                InitApp(args.Length > 0 && args[0] == "--nowindow");
                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        private static void  InitApp(bool noWindow = false)
        {
            var application = new App();
            application.InitializeComponent();
            if (!noWindow)
                application.Run(application.GetMainWindow());
            else
                application.Run();
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


        public bool SignalExternalCommandLineArgs(System.Collections.Generic.IList<string> args)
        {
            if (Current.MainWindow == null)
            {
                Current.MainWindow = GetMainWindow();
            }
            Current.MainWindow.GlobalActivate();
            return true;
        }
    }
}
