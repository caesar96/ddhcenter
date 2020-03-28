using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class RegisterClientRecommendedMedsViewModel : ViewModel
    {
        //
        public ObservableCollection<ComboBoxMedsControlViewModel> MedicinesList { get; set; }

        public ICommand AddNewMedControlCommand { get; set; }

        public RegisterClientRecommendedMedsViewModel()
        {
            Mediator.Register("VerifySelectedElement", this.OnVerifySelectedElement);
            this.MedicinesList = new ObservableCollection<ComboBoxMedsControlViewModel>();
            // Add View Models to the list
            this.MedicinesList.Add(new ComboBoxMedsControlViewModel());
            //
            this.AddNewMedControlCommand = new RelayParameterizedCommand((parameter) => this.AddNewMedControl(parameter));
        }

        private void OnVerifySelectedElement(object obj)
        {
            if (obj == null)
                  return;
            ComboBoxMedsControlViewModel SelectedViewModelMed = (ComboBoxMedsControlViewModel)obj;
            //MessageBox.Show("Funciona cabrón: " + SelectedMed.Name);
            //
            if (this.MedicinesList.Count > 1)
            {
                foreach (ComboBoxMedsControlViewModel ViewMeds in this.MedicinesList)
                {
                    if (ViewMeds.Medicamento != null && SelectedViewModelMed.Medicamento != null)
                    {
                        if (ViewMeds != SelectedViewModelMed && ViewMeds.Medicamento.Name == SelectedViewModelMed.Medicamento.Name)
                        {
                            ViewMessage.Send("Wey, ya has seleccionado el medicamento " + ViewMeds.Medicamento.Name);
                            SelectedViewModelMed.Medicamento = null;
                            return;
                        }
                    }
                }
            }
            //

        }


        private void AddNewMedControl(object parameter)
        {
            if (parameter != null)
            {
                ComboBoxMedsControlViewModel _viewModel = (ComboBoxMedsControlViewModel)parameter;
                this.MedicinesList.Remove(_viewModel);
                return;
            }
                
            //
            this.MedicinesList.Add(new ComboBoxMedsControlViewModel() {
                Medicamentos = (this.MedicinesList.Count > 0) ? this.MedicinesList[MedicinesList.Count - 1].Medicamentos : null
            });

            if (this.MedicinesList[0].Medicamentos == null)
            {
                this.GetMedicines();
                return;
            }
        }

        public async void GetMedicines()
        {
            if (MedicinesList == null || MedicinesList.Count == 0) return;
            //
            ApiClient<ResponseModel> apiClient;
            ResponseModel result;
            IEnumerable<Medicamento> newList;
            //
            apiClient = ApiClient<ResponseModel>.Instance;
            apiClient.ResourceUrl = "/medicamentos/";
            //
            result = await apiClient.GetModelAsync();

            if (result.Error && result.Message != "")
                ViewMessage.Send(result.Message);

            if (result != null && result.Medicamentos != null)
            {
                foreach (ComboBoxMedsControlViewModel ViewMeds in this.MedicinesList)
                {
                    if (ViewMeds.Medicamentos == null) ViewMeds.Medicamentos = new ObservableCollection<Medicamento>();
                    //
                    newList = result.Medicamentos.Except(ViewMeds.Medicamentos, new MedicamentoComparer() );
                    foreach (Medicamento med in newList) ViewMeds.Medicamentos.Add(med);
                }
            }
        }
    }
}
