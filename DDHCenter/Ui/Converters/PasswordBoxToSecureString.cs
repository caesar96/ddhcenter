using System.Security;
using System.Windows.Controls;
using DDHCenter.Core.Helpers;

namespace DDHCenter.Ui.Converters
{
    public class PasswordBoxToSecureString
    {
        private PasswordBox _ElementBox { get; set; }

        public PasswordBoxToSecureString(object parameter)
        {
            this._ElementBox = (PasswordBox)parameter;
        }

        public override string ToString()
        {
            SecureString _unsecuredString = (SecureString)this._ElementBox.SecurePassword;
            return _unsecuredString.SecureStringToString();
        }

    }
}
