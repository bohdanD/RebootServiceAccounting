using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projUI.Commands
{
    class SaveChangesButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Func<bool> _when;
        private Action _what;
        public SaveChangesButtonCommand(Func<bool> WhenToExecute, Action WhatToExecute)
        {
            _when = WhenToExecute;
            _what = WhatToExecute;
        }
        public bool CanExecute(object parameter)
        {
           return _when();
        }

        public void Execute(object parameter)
        {
            _what();
        }
    }
}
