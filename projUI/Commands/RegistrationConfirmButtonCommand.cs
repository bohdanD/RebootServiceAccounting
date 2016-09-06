using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    class RegistrationConfirmButtonCommand : ICommand
    {
        private Action What;
        private Func<bool> When;
        public event EventHandler CanExecuteChanged;
        public void OnCanExecuteChanged(EventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, e);
            }
        }
        public RegistrationConfirmButtonCommand(Action WhatToExecute, Func<bool> WhenToExecute)
        {
            When = WhenToExecute;
            What = WhatToExecute;
            CanExecuteChanged += RegistrationConfirmButtonCommand_CanExecuteChanged;
        }

        private void RegistrationConfirmButtonCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            var b = CanExecute(sender);
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
