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
            //Make a request to Github
            var cliente = DDHCenter.Core.Utils.ApiClient<DDHCenter.Core.Models.GithubApiModel>.Instance;
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var result = await cliente.GetModelAsync("https://api.github.com/repos/caesar96/DDHCenter/releases/latest");
                if (result != null && result.Assets != null && result.Assets[0] != null && result.Assets[0].DownloadUrl != null)
                {
                    var currentVersion =  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    var updateVersion = new Version(result.TagName.Replace("v", ""));
                    var comparedVersions = updateVersion.CompareTo(currentVersion);
                    updateFileName = System.IO.Path.GetTempPath() + result.Assets[0].Name;

                    if (comparedVersions > 0)
                    {
                        LookForUpdatesText.Text = "Descargando...";
                        await cliente.DownloadFile(result.Assets[0].DownloadUrl, updateFileName);
                        HasFoundUpdate = true;
                        LookForUpdatesText.Text = "¡Actualizar ahora!";
                        LookForUpdatesText.FontWeight = System.Windows.FontWeights.Bold;
                    }
                    else
                    {
                        LookForUpdatesText.Text = "Buscar actualizaciones";
                        MessageBox.Show("¡No hay actualizaciones disponibles!");
                    }
                        

                    //
                    LookForUpdatesProgressBar.Visibility = System.Windows.Visibility.Collapsed;

                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
    }
}
