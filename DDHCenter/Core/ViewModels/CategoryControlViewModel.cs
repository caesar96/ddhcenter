using DDHCenter.Core.Interfaces;
using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class CategoryControlViewModel : ViewModel, IViewModel
    {
        public string Name
        {
            get { return "Agregar Categoría"; }
        }

        public Categoria Categoria { get; set; }

        public CategoryControlViewModel()
        {
            Categoria = new Categoria();
            Categoria.CategoryName = "";
            IsSendingRequest = false;
            this.AddCategoryCommand = new RelayParameterizedCommand(async (parameter) => await this.AddCategory(parameter));
        }

        public bool IsSendingRequest { get; set; }

        public ICommand AddCategoryCommand { get; set; }


        private async Task AddCategory(object parameter)
        {
            //
            if (Categoria.CategoryName.Length > 0)
            {
                IsSendingRequest = true;
                var apiClient = ApiClient<ResponseModel>.Instance;
                apiClient.ResourceUrl = "/categorias/add";
                var result = await apiClient.PostModelAsync(Categoria);

                if (result.Error && result.Message != "")
                    ViewMessage.Send(result.Message);

                if (result != null && result.Categoria != null)
                {
                    Mediator.NotifyColleagues("UpdateCategoriesList", result.Categoria);
                }
                //
                IsSendingRequest = false;
                return;
            }
            //
            ViewMessage.Send("¡El nombre de la categoría está vacío!");
 
        }


    }
}
