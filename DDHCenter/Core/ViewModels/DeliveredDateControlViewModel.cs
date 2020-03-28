using DDHCenter.Core.Models;
using System;

namespace DDHCenter.Core.ViewModels
{
    public class DeliveredDateControlViewModel : ViewModel
    {
        public Cita Cita { get; set; }
        public DeliveredDateControlViewModel()
        {
            Cita = new Cita();
            Cita.CitaDate = DateTime.Today;
        }
    }
}
