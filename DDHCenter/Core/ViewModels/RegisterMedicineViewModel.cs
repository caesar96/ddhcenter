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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class RegisterMedicineViewModel : ViewModel, IViewModel
    {

        private string imageUrl = null;

        public string Name
        {
            get { return "Registrar Medicamento"; }
        }

        public ObservableCollection<CategoryComboBoxControlViewModel> CategoriesList { get; set; }

        public Medicamento Medicamento { get; set; }

        public string ImageUri { get; set; }

        public bool IsSendingRequest { get; set; }

        public bool IsUploadingImage { get; set; }

        public bool IsMedicineImageUrlSet { get; set; }


        public ICommand AddNewCategoryCommand { get; set; }
        public ICommand RegisterMedicineCommand { get; set; }
        public ICommand UploadMedicineImageCommand { get; set; }    

        public RegisterMedicineViewModel()
        {
            Mediator.Register("UpdateCategoriesList", this.OnUpdateCategoriesList);
            Medicamento = new Medicamento();
            Medicamento.ExpireDate = DateTime.Now;

            CategoriesList = new ObservableCollection<CategoryComboBoxControlViewModel>();
            CategoriesList.Add(new CategoryComboBoxControlViewModel());

            IsSendingRequest = false;
            ImageUri = null;
            this.AddNewCategoryCommand = new RelayParameterizedCommand((parameter) => this.AddNewCategory(parameter));
            this.RegisterMedicineCommand = new RelayParameterizedCommand(async (parameter) => await this.RegisterMedicine(parameter));
            this.UploadMedicineImageCommand = new RelayParameterizedCommand(async (parameter) => await this.UploadMedicineImage(parameter));
            //this.GetCategories();

        }

        private async Task UploadMedicineImage(object parameter)
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
                            this.IsMedicineImageUrlSet = true;
                            this.imageUrl = response_.Msg.Link;
                        }
                        else
                        {
                            this.imageUrl = null;
                            this.IsMedicineImageUrlSet = false;
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

        private async Task RegisterMedicine(object parameter)
        {
            if (IsSendingRequest == true)
                return;

            try
            {
                this.IsSendingRequest = true;

                var apiClient = ApiClient<ResponseModel>.Instance;

                apiClient.ResourceUrl = "/medicamentos/add";
                //
                Medicamento.ImageUrl = imageUrl;
                Medicamento.Categorias = new List<Categoria>();
                //
                foreach (CategoryComboBoxControlViewModel categoryViewModel in CategoriesList)
                {
                    if (categoryViewModel.Categoria != null)
                        Medicamento.Categorias.Add(categoryViewModel.Categoria);
                }
                //
                if (Medicamento.Categorias.Count == 0)
                    Medicamento.Categorias = null;
                //
                var result = await apiClient.PostModelAsync(Medicamento);
                this.IsSendingRequest = false;
                if (result.Error == true && result.Message != null && result.Message != "")
                {
                    ViewMessage.Send(result.Message);
                    return;
                }
                ViewMessage.Send("Medicamento registrado con éxito.");

                
                

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

        public async void GetCategories()
        {
            var apiClient = ApiClient<ResponseModel>.Instance;
            apiClient.ResourceUrl = "/categorias/";
            var result = await apiClient.GetModelAsync();

            if (result.Error && result.Message != "")
                ViewMessage.Send(result.Message);

            if (result != null && result.Categorias != null)
            {
                CategoriesList[0].Categories = new ObservableCollection<Categoria>(result.Categorias);
            }
            //
        }

        private void AddNewCategory(object parameter)
        {
            List<Categoria> _categories = new List<Categoria>(CategoriesList[0].Categories);
            CategoriesList.Add(new CategoryComboBoxControlViewModel(new ObservableCollection<Categoria>(_categories)));
        }

        private void OnUpdateCategoriesList(object obj)
        {
            Categoria _categoria = (Categoria)obj;
            if (_categoria != null)
            {
                foreach (CategoryComboBoxControlViewModel categoryViewModel in CategoriesList)
                {
                    if (categoryViewModel.Categories == null)
                        categoryViewModel.Categories = new ObservableCollection<Categoria>();
                    //
                    categoryViewModel.Categories.Add(new Categoria() {
                        Id = _categoria.Id,
                        CategoryName = _categoria.CategoryName
                    });
                    /*if (categoryViewModel.Categoria != null)
                        MessageBox.Show(categoryViewModel.Categoria.CategoryName);*/
                }
            }
            //Categories.FirstOrDefault(categoria => categoria == _categoria);
        }
    }
}
