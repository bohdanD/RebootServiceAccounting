using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    class RegistrationButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action WhatToExecute;

        public RegistrationButtonCommand(Action What)
        {
            WhatToExecute = What;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            WhatToExecute();
        }
    }
}
