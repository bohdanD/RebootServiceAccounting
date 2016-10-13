using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    class GlobalSearchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action What;
        private Func<bool> When;
        public GlobalSearchCommand(Func<bool> WhenToExecute, Action WhatToExecute)
        {
            When = WhenToExecute;
            What = WhatToExecute;
        }
        public bool CanExecute(object parameter)
        {
            return When();
        }

        public void Execute(object parameter)
        {
            What();
        }
    }
}
