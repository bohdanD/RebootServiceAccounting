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
    class AddWindowViewModel : INotifyPropertyChanged
    {
        private Client _client;
        private User _user;
        private AddButtonCommand _btAdd;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void AddClient()
        {
            _client.AddClient();
            MessageBox.Show("Клієнт доданий успішно!", "Додано!", MessageBoxButton.OK);
            _client = new Client();
            this.dpReceptionDateSet = DateTime.Today;
            _client.Id = _client.GetNextClientId();
            this.txtPhoneNumber = "+380";
            _user = new User();
            _user.Name = GlobalData.CurrentUser.Name;
            OnPropertyChanged("lblId");
            OnPropertyChanged("txtName");
            OnPropertyChanged("txtPhoneNumber");
            OnPropertyChanged("txtModel");
            OnPropertyChanged("txtProblem");
            OnPropertyChanged("cbMaster");
            OnPropertyChanged("txtCost");
            OnPropertyChanged("txtIncome");
            OnPropertyChanged("NameErorrMsg");
            OnPropertyChanged("phoneErorrMsg");
            OnPropertyChanged("problemErorrMsg");
            OnPropertyChanged("costErorrMsg");
            OnPropertyChanged("incomeErorrMsg");
            _btAdd = new AddButtonCommand(AddClient, _client.IsClientValid);
            _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            OnPropertyChanged("btnAdd");
        }

        public AddWindowViewModel()
        {
            _client = new Client();
            _client.ReceptionDate = DateTime.Today;
            _client.Id = _client.GetNextClientId();
            _client.PhoneNumber = "+380";
            _user = new User();
            _user.Name = GlobalData.CurrentUser.Name;
            _btAdd = new AddButtonCommand(AddClient, _client.IsClientValid);
        }

        public ICommand btnAdd
        {
            get
            {
                return _btAdd;
            }
        }
        public int lblId
        {
            get
            {
                return _client.Id;
            }
        }

        public string txtName
        {
            get
            {
                return _client.Name;
            }
            set
            {
                Debug.WriteLine(value);
                _client.Name = value;
                OnPropertyChanged("NameErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        public Visibility NameErorrMsg
        {
            get
            {
                return _client.IsNameValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public string txtPhoneNumber
        {
            get
            {
                return _client.PhoneNumber;
            }
            set
            {
                _client.PhoneNumber = value;
                OnPropertyChanged("phoneErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public Visibility phoneErorrMsg
        {
            get
            {
                return _client.IsPhoneNumberValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public string txtModel
        {
            get
            {
                return _client.Model;
            }
            set
            {
                _client.Model = value;
                OnPropertyChanged("modelErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        public Visibility modelErorrMsg
        {
            get
            {
                return _client.IsModelvalid() ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public string txtProblem
        {
            get
            {
                return _client.Problem;
            }
            set
            {
                _client.Problem = value;
                OnPropertyChanged("problemErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public Visibility problemErorrMsg
        {
            get
            {
                return _client.IsProblemValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public List<string> cbMasters
        {
            get
            {
                return GlobalData.UsersList.Select(i => i.Name).ToList();
            }
        }
        public string cbSelectedmaster
        {
            get
            {
                _client.MasterName = _user.Name;
                return _client.MasterName;
            }
            set
            {
                _client.MasterName = value;
                
            }
        }

        public string txtCost
        {
            get
            {
                return _client.Cost.ToString();
            }
            set
            {
                int val;
                _client.Cost = int.TryParse(value, out val) ? val : 0;
                OnPropertyChanged("costErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        public Visibility costErorrMsg
        {
            get
            {
                return _client.IsCostValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public string txtIncome
        {
            get
            {
                return _client.Income.ToString();
            }
            set
            {
                int val;
                if (int.TryParse(value, out val))
                {
                    _client.Income = val;
                    _client.IsDone = true;
                    _client.GivingDate = DateTime.Today;
                }
                else
                    _client.Income = null;
                OnPropertyChanged("incomeErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        public Visibility incomeErorrMsg
        {
            get
            {
                return _client.IsIncomeValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public DateTime dpReceptionDateSet
        {
            get
            {
               
                return _client.ReceptionDate;
            }
            set
            {
                _client.ReceptionDate = value;
                OnPropertyChanged("recDateErorrMsg");
                _btAdd.OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public Visibility recDateErorrMsg
        {
            get
            {
                return _client.IsReceptionDateValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
    }
}
