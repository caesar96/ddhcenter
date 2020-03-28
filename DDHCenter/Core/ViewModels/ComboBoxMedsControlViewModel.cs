using DDHCenter.Core.Models;
using DDHCenter.Core.Utils.Mediator;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class ComboBoxMedsControlViewModel : ViewModel
    {
        public ObservableCollection<Medicamento> Medicamentos { get; set; }

        public Medicamento Medicamento { get; set; }

        public ICommand VerifySelectedElementCommand { get; set; }

        public ComboBoxMedsControlViewModel()
        {
            Medicamentos = null;
            Medicamento = null;
            this.VerifySelectedElementCommand = new RelayParameterizedCommand((parameter) => this.VerifySelectedElement(parameter));
        }

        private void VerifySelectedElement(object parameter)
        {
            Mediator.NotifyColleagues("VerifySelectedElement", (ComboBoxMedsControlViewModel)this);
        }
    }
}
