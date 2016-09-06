using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    class OpenClientsWindowCommand : ICommand
    {
        private Action What;
        public event EventHandler CanExecuteChanged;

        public OpenClientsWindowCommand(Action WhatToExecute)
        {
            What = WhatToExecute;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            What();
        }
    }
}
