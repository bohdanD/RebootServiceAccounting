using projUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using projUI.Models;

namespace projUI.Commands
{
    class EnterButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action WhatToExecute;
        
        
      
        public EnterButtonCommand(Action What)
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
