using projUI.Commands;
using projUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace projUI.ViewModels
{
    class RegistrationWindowViewModel : INotifyPropertyChanged
    {
        private User user;
        private RegistrationConfirmButtonCommand _regConfirmCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public RegistrationWindowViewModel()
        {
            user = new User();
            _regConfirmCommand = new RegistrationConfirmButtonCommand(AddUser, user.IsUserValid);
        }
        private void AddUser()
        {
            if (user.AddUser())
            {
                MessageBox.Show("Користувача додано.", "Інформація",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    
            }
        }
        public ICommand regConfirmCommand
        {
            get
            {
                return _regConfirmCommand;
            }
        }
        public string txtLogin
        {
            set
            {
                user.Name = value;
                OnPropertyChanged("loginBrush");
                OnPropertyChanged("ErorrNameMsg");
                _regConfirmCommand.OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public string txtPassword
        {
            set
            {
                user.Password = value;
                OnPropertyChanged("passwordBrush");
                OnPropertyChanged("ErorrPasswordMsg");
                _regConfirmCommand.OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public Brush loginBrush
        {
            get
            {
                if (user.IsNameValid())
                {
                    return new SolidColorBrush(Color.FromRgb(171, 173, 179));
                }
                return new SolidColorBrush(Color.FromRgb(118, 19, 4));
            }
        }

        public Brush passwordBrush
        {
            get
            {
                if (user.IsPasswordValid())
                {
                    return new SolidColorBrush(Color.FromRgb(171, 173, 179));
                }
                return new SolidColorBrush(Color.FromRgb(118, 19, 4));
            }
        }
        public Visibility ErorrNameMsg
        {
            get
            {
                return user.IsNameValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public Visibility ErorrPasswordMsg
        {
            get
            {
                return user.IsPasswordValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public bool isAdmin
        {
            set
            {
                user.IsAdmin = value;
            }
        }
    }
}
