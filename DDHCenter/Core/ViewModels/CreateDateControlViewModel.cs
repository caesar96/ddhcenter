using DDHCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class CreateDateControlViewModel : ViewModel
    {
        public Cita Cita { get; set; }

        public CreateDateControlViewModel()
        {
            Cita = new Cita();
            Cita.CitaDate = DateTime.Today;
            Cita.IsPending = true;
        }

        #region Comandos
        #endregion

        #region Metodos 
        #endregion
    }

}
