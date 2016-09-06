using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    class AddButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action What;
        private Func<bool> When;

        public void OnCanExecuteChanged(EventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, e);
            }
        }

        public AddButtonCommand(Action WhatToExecute, Func<bool> WhenToExecute)
        {
            What = WhatToExecute;
            When = WhenToExecute;
            CanExecuteChanged += AddButtonCommand_CanExecuteChanged;
        }

        private void AddButtonCommand_CanExecuteChanged(object sender, EventArgs e)
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
