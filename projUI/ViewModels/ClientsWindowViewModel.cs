using projUI.Constants;
using projUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projUI.ViewModels
{
    class ClientsWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Client _client;
        private string quickSearchRequest;
        private string globalSearchRequest;
        private bool isActive;
        private bool isCurrentUser;

        private void OnProrertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ClientsWindowViewModel()
        {
            quickSearchRequest = "";
            globalSearchRequest = "";
            isActive = true;
            isCurrentUser = false;
            _client = new Client();
            GlobalData.LastClients = _client.GetLastClients(20);

        }



        public string txtQuickSearch
        {
            get
            {
                return quickSearchRequest;
            }
            set
            {
                quickSearchRequest = value;
                OnProrertyChanged("clients");
            }
        }
        public string txtGlobalSearch
        {
            get
            {
                return globalSearchRequest;
            }
            set
            {
                globalSearchRequest = value;
            }
        }

        public bool cbIsActiveOnly
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                OnProrertyChanged("clients");
            }
        }
        public bool cbIsCurrentUserOnly
        {
            get
            {
                return isCurrentUser;
            }
            set
            {
                isCurrentUser = value;
                OnProrertyChanged("clients");
            }
        }

        public IEnumerable<Client> clients
        {
            get
            {
                return _client.GetSearchedFromLastClients(quickSearchRequest, isCurrentUser, isActive);
            }
        }




    }
}
