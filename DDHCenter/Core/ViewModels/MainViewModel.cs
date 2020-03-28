using DDHCenter.Core.Interfaces;
using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using DDHCenter.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace DDHCenter.Core.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Campos
            private ICommand _changePageCommand = null;
            private IViewModel _currentPageViewModel = null;
            private List<IViewModel> _pageViewModels;
            private IClosable _view = null;
            private Socio _socio = null;
        #endregion

        #region Propiedades
        public Socio Socio 
        {
            get
            {
                return this._socio;
            }
            set
            {
                if (this._socio != value)
                {
                    this._socio = value;
                    ((RegisterClientViewModel)this.PageViewModels[4]).SetSocio(this._socio);
                }
            }
        }
        public string MainHeaderText { get; set; }
        public bool HasErrors { get; set; }
        public string Message { get; set; }
        //
        public List<IViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IViewModel>();

                return _pageViewModels;
            }
        }

        public IViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                }
            }
        }
        #endregion

        #region Constructor
        public MainViewModel(IClosable view)
        {
            // Susbscribe to events
            Mediator.Register("APageHashErrors", OnAPageHasErrors);

            //
            this.SetViewModels();
            //
            this.HasErrors = false;
            this.MainHeaderText = "Bienvenido ";
            
            //
            this.CloseViewCommand = new RelayCommand<IClosable>(this.CloseView);
            this.LogoutCommand = new RelayParameterizedCommand((parameter) => this.Logout(parameter));
            this._view = view;
        }
        #endregion

        #region Comandos
        public ICommand LogoutCommand { get; set; }
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IViewModel)p),
                        p => p is IViewModel);
                }

                return _changePageCommand;
            }
        }
        public RelayCommand<IClosable> CloseViewCommand { get; private set; }
        #endregion

        #region Metodos
        private async void OnAPageHasErrors(object obj)
        {
            PageErrorsAndMessage PageErrors = (PageErrorsAndMessage)obj;
            this.HasErrors = PageErrors.HasErrors;
            this.Message = PageErrors.Message;
            await Task.Delay(2 * 1000);
            this.HasErrors = false;
        }

        private void SetViewModels()
        {
            // Add available pages
            PageViewModels.Add(new HomeViewModel());
            PageViewModels.Add(new ConfigurationViewModel());
            PageViewModels.Add(new RegisterPartnerViewModel());
            PageViewModels.Add(new RegisterMedicineViewModel());
            PageViewModels.Add(new RegisterClientViewModel());

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        public async void GetUserDataLogin()
        {
            var apiClient = ApiClient<ResponseModel>.Instance;
            apiClient.ResourceUrl = "/socios/verify/token";
            var result = await apiClient.PostModelAsync(null);

            if (result != null && result.Socio != null)
            {
                this.Socio = result.Socio;
                return;
            }

            this.Logout(_view);
        }

        private void Logout(object parameter)
        {
            Properties.Settings.Default.UserToken = "";
            Properties.Settings.Default.Save();
            Mediator.Unregister("APageHashErrors", OnAPageHasErrors);
            Views.LoginView LoginView = new Views.LoginView();
            LoginView.DataContext = new LoginViewModel();
            LoginView.Show();
            this.CloseViewCommand.Execute(parameter);
        }

        private void CloseView(IClosable view)
        {
            if (view != null)
            {
                view.Close();
            }
        }

        private void ChangeViewModel(IViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
            this.MainHeaderText = CurrentPageViewModel.Name;
        }
        #endregion

    }
}
