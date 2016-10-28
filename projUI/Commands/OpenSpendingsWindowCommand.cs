using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    public class OpenSpendingsWindowCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Func<bool> when;
        private Action what;
        public OpenSpendingsWindowCommand(Func<bool> WhenToExecute, Action WhatToExecute) 
        {
            when = WhenToExecute;
            what = WhatToExecute;
        }
        public bool CanExecute(object parameter)
        {
            return when();
        }

        public void Execute(object parameter)
        {
            what();
        }
    }
}
