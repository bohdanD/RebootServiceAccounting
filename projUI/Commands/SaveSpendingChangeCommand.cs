using System;
using System.Windows.Input;

namespace projUI.Commands
{
    class SaveSpendingChangeCommand : ICommand
    {
        private Func<bool> when;
        private Action what;

        public event EventHandler CanExecuteChanged;


        public SaveSpendingChangeCommand(Func<bool> whenToExecute, Action whatToExecute)
        {
            when = whenToExecute;
            what = whatToExecute;
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
