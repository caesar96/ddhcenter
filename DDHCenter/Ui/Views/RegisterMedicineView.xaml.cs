using DDHCenter.Core.ViewModels;
using System;
using System.Windows.Controls;

namespace DDHCenter.Ui.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterMedicineView.xaml
    /// </summary>
    public partial class RegisterMedicineView : UserControl
    {
        public RegisterMedicineView()
        {
            InitializeComponent();
            this.Loaded += RegisterMedicineView_Loaded;
        }

        void RegisterMedicineView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ((RegisterMedicineViewModel)(this.DataContext)).GetCategories();
        }
    }
}
