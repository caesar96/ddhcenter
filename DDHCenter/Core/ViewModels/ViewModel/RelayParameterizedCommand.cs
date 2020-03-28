using System;
using System.Windows.Input;

namespace DDHCenter.Core.ViewModels
{
    public class RelayParameterizedCommand : ICommand
    {

        #region Public Events

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Private members

        private Action<object> _action;

        #endregion

        #region Constructor

        public RelayParameterizedCommand(Action<object> action)
        {
            _action = action;

        }
        #endregion

        #region Command Methods

        public bool CanExecute(object parameter)
        {
            return true;
        }



        public void Execute(object parameter)
        {
            _action(parameter);
        }

        #endregion
    }
}
