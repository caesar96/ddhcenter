using DDHCenter.Core.Interfaces;
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
using System.Windows.Shapes;

namespace DDHCenter.Views
{
    /// <summary>
    /// Lógica de interacción para LoginWindowView.xaml
    /// </summary>
    public partial class LoginView : Window, IClosable
    {
        public LoginView()
        {
            InitializeComponent();
            this.Loaded += LoginView_Loaded;
        }

        void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxPhone.Focus();
        }
    }
}
