using DDHCenter.Core.Interfaces;
using DDHCenter.Core.ViewModels;
using System;
using System.Windows;

namespace DDHCenter.Views
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainView : Window, IClosable
    {
        public MainView()
        {
            InitializeComponent();
            this.ContentRendered += MainView_ContentRendered;
        }

        void MainView_ContentRendered(object sender, System.EventArgs e)
        {
            if (((MainViewModel)(this.DataContext)).Socio == null)
                ((MainViewModel)(this.DataContext)).GetUserDataLogin();

        }

        private WindowState oldState;
        private bool HasFoundUpdate = false;
        private string updateFileName = "";

        private void FullScreeenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized && this.WindowStyle == System.Windows.WindowStyle.SingleBorderWindow)
            {
                this.oldState = this.WindowState;
                this.WindowState = System.Windows.WindowState.Normal;
            }
            
            if (this.WindowState == System.Windows.WindowState.Normal && this.WindowStyle == System.Windows.WindowStyle.SingleBorderWindow)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.WindowState = System.Windows.WindowState.Maximized;
                this.FullScreeenBtn.Content = "Restaurar";
                return;
            }
   
            this.FullScreeenBtn.Content = "Pantalla Completa";
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.WindowState = this.oldState;
            this.oldState = 0;


        }

        private async void LookForUpdatesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.HasFoundUpdate && updateFileName != "")
            {
                System.Diagnostics.Process.Start(@updateFileName, @"/VERYSILENT /SUPPRESSMSGBOXES /CLOSEAPPLICATIONS");
                return;
            };
            // Change some text
            LookForUpdatesText.Text = "Buscando...";
            LookForUpdatesProgressBar.Visibility = System.Windows.Visibility.Visible;
            PopUpBoxM.IsPopupOpen = true;
            var systemArchitecture = IntPtr.Size == 8 ? 0 : 1;
            //Make a request to Github
            var cliente = DDHCenter.Core.Utils.ApiClient<DDHCenter.Core.Models.GithubApiModel>.Instance;
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var result = await cliente.GetModelAsync("https://api.github.com/repos/caesar96/DDHCenter/releases/latest");
                if (result != null && result.Assets != null && result.Assets[systemArchitecture] != null && result.Assets[systemArchitecture].DownloadUrl != null)
                {
                    var currentVersion =  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    var updateVersion = new Version(result.TagName.Replace("v", ""));
                    var comparedVersions = updateVersion.CompareTo(currentVersion);
                    updateFileName = System.IO.Path.GetTempPath() + result.Assets[systemArchitecture].Name;

                    if (comparedVersions > 0)
                    {
                        LookForUpdatesText.Text = "Descargando...";
                        await cliente.DownloadFile(result.Assets[systemArchitecture].DownloadUrl, updateFileName);
                        HasFoundUpdate = true;
                        LookForUpdatesText.Text = "¡Actualizar ahora!";
                        LookForUpdatesText.FontWeight = System.Windows.FontWeights.Bold;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
            finally
            {
                if (!HasFoundUpdate)
                {
                    MessageBox.Show("¡No hay actualizaciones disponibles!");
                    LookForUpdatesText.Text = "Buscar actualizaciones";
                }
                    
                LookForUpdatesProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
