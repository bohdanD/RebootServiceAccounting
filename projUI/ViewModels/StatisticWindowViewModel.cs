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

namespace projUI.ViewModels
{
    class StatisticWindowViewModel : INotifyPropertyChanged
    {
        private User _user;
        private Client _client;
        private Spending _spending;
        private DateTime _fromDate;
        private DateTime _toDate;
        private bool isAll = false;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private bool IsDatesValid(DateTime fromDate, DateTime toDate)
        {
            bool result = DateTime.Compare(fromDate, DateTime.Today.AddDays(1)) < 0
                && DateTime.Compare(toDate, DateTime.Today.AddDays(1)) < 0
                && !(DateTime.Compare(fromDate, toDate) > 0);
            return result;
        }
        public StatisticWindowViewModel()
        {
            _client = new Client();
            _user = new User();
            _spending = new Spending();
            _user.Name = GlobalData.CurrentUser.Name;
            _fromDate = DateTime.Today;
            _toDate = DateTime.Today;
        }

        public List<string> cbUsers
        {
            get
            {
                var res = GlobalData.UsersList.Select(i => i.Name).ToList();
                res.Add("All");
                return res;
            }
            
        }
        public string cbSelected
        {
            get
            {
                return _user.Name;
            }
            set
            {
                if (value != "All")
                {
                    _user.Name = value;
                    isAll = false;
                }
                else
                    isAll = true;
                OnPropertyChanged("lblDoneOrders");
            }
        }

       

        public DateTime fromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                Debug.WriteLine(value);
                _fromDate = value;
                OnPropertyChanged("lblDoneOrders");
                OnPropertyChanged("ErrorLblVisibility");
            }
        }
        public DateTime toDate
        {
            get
            {
                return _toDate;
            }
            set
            {
                Debug.WriteLine(value);
                _toDate = value;
                OnPropertyChanged("lblDoneOrders");
                OnPropertyChanged("ErrorLblVisibility");
            }
        }

        public int lblDoneOrders
        {
            get
            {
                OnPropertyChanged("lblTotalCost");
                if(isAll)
                    return _client.GetOrdersCountForTime(_fromDate, _toDate);
                return _client.GetOrdersCountForTime(_user, _fromDate, _toDate);
            }
        }
        public int lblTotalCost
        {
            get
            {
                OnPropertyChanged("lblIncome");
                if(isAll)
                    return _client.GetCostsForTime(_fromDate, _toDate);
                return _client.GetCostsForTime(_user, _fromDate, _toDate);
            }
        }
        public int lblIncome
        {
            get
            {
                OnPropertyChanged("lblSpendings");
                if(isAll)
                    return _client.GetIncomeForTime(_fromDate, _toDate);
                return _client.GetIncomeForTime(_user, _fromDate, _toDate);
            }
        }

        public int lblSpendings
        {
            get
            {
                return _spending.GetSpendingCostsByDate(_fromDate, _toDate);
            }
        }
        public Visibility ErrorLblVisibility
        {
            get
            {
                return IsDatesValid(_fromDate, _toDate) ? Visibility.Hidden : Visibility.Visible;
            }
        }
    }
}
