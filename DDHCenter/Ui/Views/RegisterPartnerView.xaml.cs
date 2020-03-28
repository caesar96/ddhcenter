using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DDHCenter.Ui.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterPartner.xaml
    /// </summary>
    public partial class RegisterPartnerView : UserControl
    {
        public RegisterPartnerView()
        {
            InitializeComponent();
            this.Loaded += RegisterPartnerView_Loaded;
        }

        void RegisterPartnerView_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxFirstName.Focus();
        }
    }
}
