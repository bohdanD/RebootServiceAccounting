using projUI.Commands;
using projUI.Constants;
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

namespace projUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// User object that using for comparing with User`s from db
        /// </summary>
        private User user;
        /// <summary>
        /// Command object for EnterButton.
        /// </summary>
        private EnterButtonCommand entBtnCommand;
        /// <summary>
        /// Command object for RegistrationButton.
        /// </summary>
        private RegistrationButtonCommand regBtnCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// To occur PropertyChanged.
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        /// <summary>
        /// Run`s RegistrationWindow.
        /// </summary>
        private void RunRegistrationWindow()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            user = new User();
            entBtnCommand = new EnterButtonCommand(Login);
            regBtnCommand = new RegistrationButtonCommand(RunRegistrationWindow);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Login()
        {
            //TODO: finish login func
            if (user.IsUserExist())
            {
                GlobalData.CurrentUser = user.GetUsers().Where(i => i.Name.Equals(user.Name)).First();
                UserWindow win = new UserWindow();
                win.Show();
                App.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Неправильний логін або пароль!", "Помилка входу!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
                

        }
        /// <summary>
        /// Property to bind command for EnterButton.
        /// </summary>
        public ICommand btnClick
        {
            get
            {
                return entBtnCommand;
            }
        }
        /// <summary>
        /// Property to bind command for RegistrationButton.
        /// </summary>
        public ICommand regBtnClick
        {
            get
            {
                return regBtnCommand;
            }
        }
        /// <summary>
        /// Property to bind login textBox.
        /// </summary>
        public string txtLogin
        {
            set
            {
                Debug.WriteLine(value);
                user.Name = value;
                
            }
           
        }

       
        /// <summary>
        /// Property to bind password textBox.
        /// </summary>
        public string txtPassword
        {
            set
            {
                user.Password = value;
            }
        }
    }
}
