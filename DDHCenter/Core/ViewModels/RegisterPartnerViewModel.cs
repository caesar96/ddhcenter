using DDHCenter.Core.Exceptions;
using DDHCenter.Core.Interfaces;
using DDHCenter.Core.Models;
using DDHCenter.Core.Types;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using DDHCenter.Ui.Converters;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class RegisterPartnerViewModel : ViewModel, IViewModel
    {

        #region Campos
        private string _socioImageUrl = "";
        #endregion

        #region Propiedades
        public string Name
        {
            get 
            {
                return "Registrar Socio";
            }
        }

        //
        public Socio Socio { get; set; }
        //
        public bool IsMale { get; set; }
        public bool IsFemale { get; set; }
        public bool IsOtherGenre { get; set; }
        public bool IsSendingRequest { get; set; }
        public bool IsUploadingImage { get; set; }
        public string SocioImageUriLocal { get; set; }
        public bool IsSocioImageUrlSet { get; set; }
        public string StreamedImage { get; set; }
        #endregion

        #region Constructor
        public RegisterPartnerViewModel()
        {
            Socio = new Socio();
            Socio.BirthDate = DateTime.Parse("1990-01-01");
            this.IsSendingRequest = false;
            this.IsUploadingImage = false;
            this.RegisterPartnerCommand = new RelayParameterizedCommand(async (parameter) => await this.RegisterPartner(parameter));
            this.UploadSocioImageCommand = new RelayParameterizedCommand(async (parameter) => await this.UploadSocioImage(parameter));
            this.SocioImageUriLocal = "";
            this.IsSocioImageUrlSet = false;
            this.IsFemale = false;
            this.IsMale = true;
            this.IsOtherGenre = false;
        }

        #endregion

        #region Comandos
        public ICommand RegisterPartnerCommand { get; set; }
        public ICommand UploadSocioImageCommand { get; set; }        
        #endregion

        #region Metodos
        private GenderType ConverBooleansToGenderType(bool male, bool female, bool other)
        {
            if (male)
                return GenderType.Male;
            if (female)
                return GenderType.Female;
            return GenderType.Other;
        }
        //
        private int parseStringToInt(string _string_)
        {
            int stringToInt;
            if (int.TryParse(_string_, out stringToInt))
            {
                return stringToInt;
            }
            return 0;
        }
        //
        private async Task UploadSocioImage(object parameter)
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
                        this.StreamedImage = nombreImagen;
                        fileStream.Position = 0;
                        var response_ = await apiClient.PostImageAsync(fileStream);

                        if (response_ != null && response_.Msg != null && response_.Msg.Link != null && response_.Msg.Link != "")
                        {
                            this.IsSocioImageUrlSet = true;
                            this._socioImageUrl = response_.Msg.Link;
                        }
                        else
                        {
                            this.IsSocioImageUrlSet = false;
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
        //
        public async Task RegisterPartner(object parameter)
        {
            if (IsSendingRequest == true)
                return;

            try
            {
                this.IsSendingRequest = true;

                Socio.ImageUrl = (this.IsSocioImageUrlSet) ? this._socioImageUrl : null;

                Socio.Gender = this.ConverBooleansToGenderType(this.IsMale, this.IsFemale, this.IsOtherGenre);
                Socio.Password = (new PasswordBoxToSecureString(parameter)).ToString();

                var apiClient = ApiClient<ResponseModel>.Instance;

                apiClient.ResourceUrl = "/socios/register";

                var result = await apiClient.PostModelAsync(Socio);
                if (result.Error == true && result.Message != null && result.Message != "")
                {
                    ViewMessage.Send(result.Message);
                }
                if (result.Socio != null)
                    ViewMessage.Send("Socio registrado con éxito.");

                this.IsSendingRequest = false;

            }
            catch (ApiException e)
            {
                ViewMessage.Send(e.Content);
            }
            catch (Exception er)
            {
                ViewMessage.Send(er.Message);
            }
            finally
            {
                if (IsSendingRequest == true)
                    this.IsSendingRequest = false;
            }
        }
        #endregion
    }
}
