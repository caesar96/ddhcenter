using DDHCenter.Core.Models;
using DDHCenter.Core.Utils;
using DDHCenter.Core.Utils.Mediator;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;


namespace DDHCenter.Core.ViewModels
{
    public class RegisterClientBoughtMedsViewModel : ViewModel
    {
        public ObservableCollection<BoughtMedControlViewModel> MedicinesList { get; set; }

        public ICommand AddNewMedControlCommand { get; set; }

        public double TotalPrice { get; set; }

        public RegisterClientBoughtMedsViewModel()
        {
            //
            Mediator.Register("VerifyBoughtSelectedElement", this.OnVerifyBoughtSelectedElement);
            Mediator.Register("ResponseOfRecommendedMeds", this.OnResponseOfRecommendedMeds);
            MedicinesList = new ObservableCollection<BoughtMedControlViewModel>();
            MedicinesList.Add(new BoughtMedControlViewModel());
            this.TotalPrice = 0;

            this.AddNewMedControlCommand = new RelayParameterizedCommand((parameter) => this.AddNewMedControl(parameter));
        }


        private void OnResponseOfRecommendedMeds(object obj)
        {
            if (obj == null) return;
            List<Medicamento> medicamentos = (List<Medicamento>)obj;
            this.TotalPrice = 0;
            foreach (BoughtMedControlViewModel ViewMeds in MedicinesList)
            {
                ViewMeds.Medicamentos = new ObservableCollection<Medicamento>(medicamentos);
                if (ViewMeds.Medicamento != null) this.TotalPrice += ViewMeds.Medicamento.Price;
            }
        }

        private void OnVerifyBoughtSelectedElement(object obj)
        {
            if (obj == null)
                return;
            BoughtMedControlViewModel SelectedViewModelMed = (BoughtMedControlViewModel)obj;
            //
            this.TotalPrice = 0;
            foreach (BoughtMedControlViewModel ViewMeds in this.MedicinesList)
            {
                if (ViewMeds.Medicamento != null) this.TotalPrice += ViewMeds.Medicamento.Price;
                //
                if (ViewMeds.Medicamento != null && SelectedViewModelMed.Medicamento != null)
                {
                    if (ViewMeds != SelectedViewModelMed && ViewMeds.Medicamento.Name == SelectedViewModelMed.Medicamento.Name)
                    {
                        ViewMessage.Send("Wey, ya has seleccionado el medicamento " + ViewMeds.Medicamento.Name);
                        this.TotalPrice -= SelectedViewModelMed.Medicamento.Price;
                        SelectedViewModelMed.Medicamento = null;
                        return;
                    }
                    //
                        
                }
            }
            
        }

        private void AddNewMedControl(object parameter)
        {
            if (parameter != null)
            {
                BoughtMedControlViewModel _viewModel = (BoughtMedControlViewModel)parameter;
                this.TotalPrice = 0;

                foreach (BoughtMedControlViewModel ViewMeds in this.MedicinesList)
                {
                    if (ViewMeds.Medicamento != null)
                        this.TotalPrice += ViewMeds.Medicamento.Price;
                }
                //
                MedicinesList.Remove(_viewModel);
                
                return;
            }

            //
            this.MedicinesList.Add(new BoughtMedControlViewModel()
            {
                Medicamentos = (this.MedicinesList.Count > 0) ? this.MedicinesList[MedicinesList.Count - 1].Medicamentos : null
            });

            if (this.MedicinesList[0].Medicamentos == null)
            {
                this.GetMedicines();
                return;
            }
        }

        public void GetMedicines()
        {
            /*if (MedicinesList == null || MedicinesList.Count == 0)
                return;
            //
             ApiClient<ResponseModel> apiClient = null;
             ResponseModel result = null;
            //
             if (MedicinesList.Count > 0 && MedicinesList[0].Medicamento != null)
                 return;
            apiClient = ApiClient<ResponseModel>.Instance;
            apiClient.ResourceUrl = "/medicamentos/";
            result = await apiClient.GetModelAsync();

            if (result.Error && result.Message != "")
                ViewMessage.Send(result.Message);

            if (result != null && result.Medicamentos != null)
            {
                MedicinesList[0].Medicamentos = new ObservableCollection<Medicamento>(result.Medicamentos);
            }*/
            Mediator.NotifyColleagues("RequestRecommendedMeds", null);
        }
    }
}
