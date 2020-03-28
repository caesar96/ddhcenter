using DDHCenter.Core.Interfaces;
using DDHCenter.Core.Models;
using DDHCenter.Core.Helpers;
using DDHCenter.Ui.Converters;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using DDHCenter.Core.Exceptions;

using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows;

namespace DDHCenter.Core.ViewModels
{
    public class LoginViewModel : ViewModel, IViewModel
    {

        #region Campos
        private Socio _socio = null;
        private ApiClient<ResponseModel> apiClient = null;
        #endregion

        #region Propiedades
        public string Name
        {
            get 
            {
                return "Iniciar Sesión";
            }
        }

        //
        public bool HasErrors { get; set; }

        public string Message { get; set; }

        public bool IsSendingRequest { get; set; }

        public string Phone { get; set; }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            this.HasErrors = false;
            this.IsSendingRequest = false;
            this.LoginCommand = new RelayParameterizedCommand(async (parameter) => await this.Login(parameter));
            this.CloseViewCommand = new RelayCommand<IClosable>(this.CloseView);
        }

        #endregion

        #region Comandos
        public ICommand LoginCommand { get; set; }
        //
        public RelayCommand<IClosable> CloseViewCommand { get; private set; }
        #endregion

        #region Metodos

        public async Task Login(object parameter)
        {
            if (IsSendingRequest == true)
                return;

            try
            {
                //
                var parameters = (object[])parameter;
                this.IsSendingRequest = true;
                //
                //
                var socio = new Socio
                {
                    Phone = this.Phone,
                    Password = (new PasswordBoxToSecureString(parameters[1])).ToString()
                };

                apiClient = ApiClient<ResponseModel>.Instance;

                apiClient.ResourceUrl = "/socios/login";

                var result = await apiClient.PostModelAsync(socio);
                if (result.Error)
                {
                    this.HasErrors = true;
                    this.Message = result.Message;

                    this.IsSendingRequest = false;
                    await Task.Delay(2 * 1000);
                    this.HasErrors = false;
                    return;
                }

                if (result.Token != null)
                {
                    Properties.Settings.Default.UserToken = result.Token;
                    Properties.Settings.Default.Save();

                    if (result.Socio != null)
                    {
                        this._socio = result.Socio;
                        this.CloseViewCommand.Execute(parameters[0]);
                    }
                }

            }
            catch (ApiException e)
            {
                this.HasErrors = true;
                this.Message = e.Message;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
            finally
            {
                if (IsSendingRequest == true)
                    this.IsSendingRequest = false;
            }
        }        
        //
        private void CloseView(IClosable view)
        {
            if (view != null)
            {
                
                Views.MainView MainView = new Views.MainView();
                if (this._socio != null) 
                {
                    MainView.DataContext = new MainViewModel(null)
                    {
                        Socio = this._socio
                    };
                }

                else
                {
                    this.HasErrors = true;
                    this.Message = "Hubo en error al iniciar sesión. Intentalo más tarde...";
                    return;
                }
                MainView.Show();
                view.Close();
            }
        }
        #endregion
    }
}
