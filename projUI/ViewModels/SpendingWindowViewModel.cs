using projUI.Commands;
using projUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace projUI.ViewModels
{
    class SpendingWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Spending _spend = new Spending();
        private SaveSpendingCommand _btSaveSpending;

        public SpendingWindowViewModel()
        {
            _spend.Date = DateTime.Today;
            _btSaveSpending = new SaveSpendingCommand(()=> { return true; }, SaveSpending);
        }

        public string SpendingName
        {
            get
            {
                return _spend.Name;
            }
            set
            {
                _spend.Name = value;
            }
        }

        public string SpendingCost
        {
            get
            {
                return _spend.Cost.ToString();
            }
            set
            {
                int res;
                if (!int.TryParse(value, out res))
                    _spend.Cost = 0;
                else
                    _spend.Cost = res;
                OnPropertyChanged("showCostError");
            }
        }

        public DateTime SpendingDate
        {
            get
            {
                return _spend.Date;
            }
            set
            {
                _spend.Date = value;
                OnPropertyChanged("showDateError");
            }
        }

        public ICommand btnSaveSpendings
        {
            get
            {
                return _btSaveSpending;
            }
        }


        public Visibility showCostError
        {
            get
            {
                return isCostValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public Visibility showDateError
        {
            get
            {
                return isDateValid() ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private bool isDateValid()
        {
            return _spend.Date.CompareTo(DateTime.Today) <= 0;
        }
        private bool isNameValid()
        {
            return _spend.Name != null || _spend.Name != "";
        }
        private bool isCostValid()
        {
            return _spend.Cost > 0;
        }

        private void SaveSpending()
        {
            if (isCostValid() && isDateValid())
            {
                _spend.Add();
                MessageBox.Show("Витрату успішно додано", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBox.Show("Деякі поля заповненні не правильно!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
