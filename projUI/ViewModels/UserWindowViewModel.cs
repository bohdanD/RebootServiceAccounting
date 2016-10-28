using projUI.Commands;
using projUI.Constants;
using projUI.Models;
using System.Windows.Input;

namespace projUI.ViewModels
{
    class UserWindowViewModel
    {
        private Client _client;
        private StatisticButtonCommand _btStatistic;
        private AddWinButtonCommand _btAddWin;
        private OpenClientsWindowCommand _btClients;
        private OpenSpendingsWindowCommand _btSpendings;
        public UserWindowViewModel()
        {
            _client = new Client();
            _btStatistic = new StatisticButtonCommand(StatisticWindowShow);
            _btAddWin = new AddWinButtonCommand(AddWindowShow);
            _btClients = new OpenClientsWindowCommand(ClientsWindowShow);
            _btSpendings = new OpenSpendingsWindowCommand(() => { return true; }, SpendingWindowShow);
        }

        public ICommand btnStatic
        {
            get
            {
                return _btStatistic;
            }
        }
        public ICommand btnAddWin
        {
            get
            {
                return _btAddWin;
            }
        }
        public ICommand btnClients
        {
            get
            {
                return _btClients;
            }
        }
        public ICommand btnSpendings
        {
            get
            {
                return _btSpendings;
            }
        }
        public string lblHeader
        {
            get
            {
                return GlobalData.CurrentUser.Name.ToUpper();
            }
        }
        public int lblDoneOrdersCount
        {
            get
            {
                return _client.GetDoneOrdersCount(GlobalData.CurrentUser);
            }
        }
        public int lblIncome
        {
            get
            {
                return _client.GetIncome(GlobalData.CurrentUser);
            }
        }
        private void StatisticWindowShow()
        {
            StatisticWindow win = new StatisticWindow();
            win.ShowDialog();
        }
        private void AddWindowShow()
        {
            AddClientWindow win = new AddClientWindow();
            win.ShowDialog();
        }
        private void ClientsWindowShow()
        {
            ClientsWindow win = new ClientsWindow();
            win.ShowDialog();
        }

        private void SpendingWindowShow()
        {
            SpendingWindow win = new SpendingWindow();
            win.ShowDialog();
        }
    }
}
