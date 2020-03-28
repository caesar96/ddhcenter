using DDHCenter.Core.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DDHCenter.Ui.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterClientRecommendedMedsView.xaml
    /// </summary>
    public partial class RegisterClientRecommendedMedsView : UserControl
    {
        public RegisterClientRecommendedMedsView()
        {
            InitializeComponent();
            this.Loaded += RegisterClientRecommendedMedsView_Loaded;
        }

        void RegisterClientRecommendedMedsView_Loaded(object sender, RoutedEventArgs e)
        {
            ((RegisterClientRecommendedMedsViewModel)(this.DataContext)).GetMedicines();
        }
    }
}
