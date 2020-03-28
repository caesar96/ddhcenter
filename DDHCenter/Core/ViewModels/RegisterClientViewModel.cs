using DDHCenter.Core.Exceptions;
using DDHCenter.Core.Interfaces;
using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class RegisterClientViewModel : ViewModel, IViewModel
    {
        #region Campos
        private List<ViewModel> _clientExtraViews = null;
        private ViewModel _clientExtraView = null;
        private string _imageUri = "";
        #endregion

        #region Propiedades
        public int SocioId { get; set; }
        //
        public string Name
        {
            get { return "Registrar Cliente"; }
        }
        //
        public Cliente Cliente { get; set; }
        public ObservableCollection<ComboBoxMedsControlViewModel> MedicamentosList { get; set; }
        //
        public bool IsUploadingImage { get; set; }
        public bool IsSendingRequest { get; set; }
        public string ImageUri { get; set; }
        //
        public bool IsMale { get; set; }
        public bool IsFemale { get; set; }
        public bool IsOtherGenre { get; set; }

        public List<ViewModel> ClientExtraViews
        {
            get
            {
                return _clientExtraViews;
            }
            set
            {
                if (_clientExtraViews != value)
                    _clientExtraViews = value;
            }
        }

        public ViewModel ClientExtraView
        {
            get 
            {
                return _clientExtraView;
            }
            set 
            {
                if (_clientExtraView != value)
                    _clientExtraView = value;
            } 
        }
        #endregion

        #region Constructor
        public RegisterClientViewModel()
        {
            // SUBSCRIBE TO EVENT IN ORDER TO RECEIVE OR COMUNICATE WITH OTHER VIEWS
            Mediator.Register("RequestRecommendedMeds", this.OnRequestRecommendedMeds);
            // Set Bindings for the form
            this.SetClienteForm();
            // Comandos
            this.RegisterClientCommand = new RelayParameterizedCommand(async (parameter) => await this.RegisterClient(parameter));
            this.UploadClientImageCommand = new RelayParameterizedCommand(async (parameter) => await this.UploadClientImage(parameter));
            this.ShowCreateDateControlViewCommand = new RelayParameterizedCommand((parameter) => this.ShowCreateDateControlView(parameter));
            this.ShowRegisterClientRecommendedMedsViewCommand = new RelayParameterizedCommand((parameter) => this.ShowRegisterClientRecommendedMedsView(parameter));
            this.ShowRegisterClientBoughtMedsViewCommand = new RelayParameterizedCommand((parameter) => this.ShowRegisterClientBoughtMedsView(parameter));
            this.ShowDeliverDateControlCommand = new RelayParameterizedCommand((parameter) => this.ShowDeliverDateControl(parameter));
        }

        #endregion

        #region Comandos
        public ICommand ShowRegisterClientRecommendedMedsViewCommand { get; set; }

        public ICommand ShowCreateDateControlViewCommand { get; set; }

        public ICommand ShowRegisterClientBoughtMedsViewCommand { get; set; }
        public ICommand ShowDeliverDateControlCommand { get; set; }
        public ICommand RegisterClientCommand { get; set; }
        public ICommand UploadClientImageCommand { get; set; }   
        #endregion

        #region Metodos

        private void SetClienteForm()
        {
            this.Cliente = new Cliente();
            this.Cliente.BirthDay = DateTime.Parse("1990-01-01");
            this.IsFemale = false;
            this.IsMale = true;
            this.IsOtherGenre = false;
            this.ImageUri = null;
            this._imageUri = "";
            //
            this.ClientExtraViews = new List<ViewModel>();
            this.ClientExtraViews.Add(new CreateDateControlViewModel());
            this.ClientExtraViews.Add(new RegisterClientRecommendedMedsViewModel());
            this.ClientExtraViews.Add(new RegisterClientBoughtMedsViewModel());
            this.ClientExtraViews.Add(new DeliveredDateControlViewModel());
            //
            this.ClientExtraView = null;
            //
            this.MedicamentosList = new ObservableCollection<ComboBoxMedsControlViewModel>();
        }

        private void OnRequestRecommendedMeds(object obj)
        {
            List<Medicamento> _medicamentos = new List<Medicamento>();
            foreach (ComboBoxMedsControlViewModel _ViewModel in ((RegisterClientRecommendedMedsViewModel)this.ClientExtraViews[1]).MedicinesList)
            {
                if (_ViewModel.Medicamento != null)
                {
                    _medicamentos.Add(_ViewModel.Medicamento);
                }
            }
            Mediator.NotifyColleagues("ResponseOfRecommendedMeds", _medicamentos);
        }

        public void SetSocio(Socio _socio)
        {
            Cliente.Socios = new List<Socio>();
            Cliente.Socios.Add(_socio);
        }

        private void ShowDeliverDateControl(object parameter)
        {
            if (this.ClientExtraView is DeliveredDateControlViewModel)
            {
                this.ClientExtraView = null;
                return;
            }
            this.ClientExtraView = this.ClientExtraViews[3];
        }

        private void ShowRegisterClientBoughtMedsView(object parameter)
        {
            if (this.ClientExtraView is RegisterClientBoughtMedsViewModel)
            {
                this.ClientExtraView = null;
                return;
            }
            this.ClientExtraView = this.ClientExtraViews[2];
        }

        private void ShowRegisterClientRecommendedMedsView(object parameter)
        {
            if (this.ClientExtraView is RegisterClientRecommendedMedsViewModel)
            {
                this.ClientExtraView = null;
                return;
            }
            this.ClientExtraView = this.ClientExtraViews[1];
        }
        private void ShowCreateDateControlView(object parameter)
        {
            if (this.ClientExtraView is CreateDateControlViewModel)
            {
                this.ClientExtraView = null;
                return;
            }
            this.ClientExtraView = this.ClientExtraViews[0];
        }

        private async Task UploadClientImage(object parameter)
        {
            if (this.IsUploadingImage)
                return;
            try
            {
                string nombreImagen = "";
                // Se crea un cuadro de dialogo.
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                //  Filtro y extensión por default
                dlg.Filter = "Archivos JPG (*.jpg)|*.jpg|Archivos PNG (*.png)|*.png|Archivos JPEG (*.jpeg)|*.jpeg";
                dlg.DefaultExt = ".jpg";

                Nullable<bool> result = dlg.ShowDialog();
                // Recupera el nombre del archivo.
                if (result == true)
                {
                    nombreImagen = dlg.FileName;


                    // this.SocioImageUrl = nombreImagen;

                    using (var fileStream = new FileStream(nombreImagen, FileMode.Open, FileAccess.Read))
                    {
                        var apiClient = ApiClient<ImageResultModel>.Instance;

                        apiClient.ResourceUrl = "https://malaa.ga/upload_image.php";

                        //
                        this.IsUploadingImage = true;

                        //
                        this.ImageUri = nombreImagen;
                        fileStream.Position = 0;
                        var response_ = await apiClient.PostImageAsync(fileStream);

                        if (response_ != null && response_.Msg != null && response_.Msg.Link != null && response_.Msg.Link != "")
                        {
                            this._imageUri = response_.Msg.Link;
                        }
                        else
                        {
                            this._imageUri = "";
                            ViewMessage.Send("Ocurrió un error al subir la imágen. Intentalo de nuevo.");
                        }
                    }
                }
                this.IsUploadingImage = false;
            }

            catch (ApiException e)
            {
                ViewMessage.Send(e.Content);
            }
            finally
            {
                if (this.IsUploadingImage)
                    this.IsUploadingImage = false;
            }
        }

        private async Task RegisterClient(object parameter)
        {

            Cliente.NextProgrammedDates = new List<Cita>();

            Cliente.RecommendedMeds = new List<Medicamento>();

            Cliente.BoughtMeds = new List<Venta>();

            List<Medicamento> boughtMeds = new List<Medicamento>();

            if (((CreateDateControlViewModel)this.ClientExtraViews[0]).Cita.CitaDate != null && ((CreateDateControlViewModel)this.ClientExtraViews[0]).Cita.CitaDate > DateTime.Today)
                Cliente.NextProgrammedDates.Add(((CreateDateControlViewModel)this.ClientExtraViews[0]).Cita);

            foreach (ComboBoxMedsControlViewModel _ViewModel in ((RegisterClientRecommendedMedsViewModel)this.ClientExtraViews[1]).MedicinesList)
            {
                if (_ViewModel.Medicamento != null)
                {
                    Cliente.RecommendedMeds.Add(_ViewModel.Medicamento);
                }
            }

            if (Cliente.RecommendedMeds.Count == 0)
                Cliente.RecommendedMeds = null;

            if (Cliente.NextProgrammedDates.Count == 0)
                Cliente.NextProgrammedDates = null;

            if (((DeliveredDateControlViewModel)this.ClientExtraViews[3]).Cita.CitaDate > DateTime.Today)
            {
                Cliente.BoughtMeds.Add(new Venta() {
                    DeliveredDate = ((DeliveredDateControlViewModel)this.ClientExtraViews[3]).Cita.CitaDate,
                    IsSold = false
                });
            }
            else
            {
                foreach (BoughtMedControlViewModel _ViewModel in ((RegisterClientBoughtMedsViewModel)this.ClientExtraViews[2]).MedicinesList)
                {
                    if (_ViewModel.Medicamento != null)
                    {
                        boughtMeds.Add(_ViewModel.Medicamento);
                    }
                }
                Cliente.BoughtMeds.Add(new Venta()
                {
                    SoldMeds = boughtMeds,
                    IsSold = true,
                    Total = ((RegisterClientBoughtMedsViewModel)this.ClientExtraViews[2]).TotalPrice
                });
            }
            //
            Cliente.ImageUrl = this._imageUri != "" ? this._imageUri : null;

            try
            {
                IsSendingRequest = true;
                //
                var apiClient = ApiClient<ResponseModel>.Instance;
                apiClient.ResourceUrl = "/clientes/add";
                var result = await apiClient.PostModelAsync(Cliente);

                if (result.Error)
                {
                    ViewMessage.Send(result.Message);
                    return;
                }

                if (result != null && result.Cliente != null)
                {
                    this.SetClienteForm();
                    ViewMessage.Send("Cliente registrado con éxitooo!");
                    return;
                }
                //
            }
            catch (Exception e)
            {
                ViewMessage.Send(e.Message);
            }
            finally 
            {
                IsSendingRequest = false;
            }
              
        }
        #endregion
    }
}
