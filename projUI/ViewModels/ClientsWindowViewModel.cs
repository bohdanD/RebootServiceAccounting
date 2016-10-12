using projUI.Commands;
using projUI.Constants;
using projUI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace projUI.ViewModels
{
    class ClientsWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Client _client;
        private string quickSearchRequest;
        private string globalSearchRequest;
        private bool isActive;
        private bool isCurrentUser;
        private SaveChangesButtonCommand btnSave;
        private void OnProrertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ClientsWindowViewModel()
        {
            //---btnSave initialization --- 
            btnSave = new SaveChangesButtonCommand(()=> { return true; }, SaveChanges);
            //-----

           // quickSearchRequest = "";
            globalSearchRequest = "";
            isActive = true;
            isCurrentUser = false;
            _client = new Client();
            GlobalData.LastClients = _client.GetLastClients(20);
            Clients = CollectionViewSource.GetDefaultView(GlobalData.LastClients);
            Clients.Filter = Filter;
          
        }
        
        private void SaveChanges()
        {
            var items = Clients.OfType<Client>();
            var Ids = items.Select(i => i.Id).ToArray();
            var dbItems = items.First().GetClientsByIds(Ids);
            int index = 0;
            string infoMsg = "Зміни успішно внесено до замовлень(ня) під номером:";
            foreach (var item in items)
            {
                if (!item.Equals(dbItems[index]))
                {
                    if(item.IsDone == true)
                    {
                        string erorrMsg = $"Проблеми зі збереженням замовлення під номером {item.Id}. Перевірте поля:\n";
                        if (item.GivingDate.Equals(null))
                            item.GivingDate = DateTime.Today;
                        if (item.Income==null || item.Income.Value == 0)
                        {
                            erorrMsg += "Прибуток\n";
                            MessageBox.Show(erorrMsg, "Помилка збереження", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                         
                    }
                    item.UpdClient(item);
                    infoMsg += $" {item.Id},";
                }
                index ++;
            }
            infoMsg.Remove(infoMsg.LastIndexOf(','));
            MessageBox.Show(infoMsg, "Зміни збережено", MessageBoxButton.OK, MessageBoxImage.Information);
            Clients.Refresh();
            
        }
  
        public bool Filter(object obj)
        {
            bool result = true;
            var i = obj as Client;
            string searchString = quickSearch;
            if (searchString != null && searchString != "")
                result = i.Id.ToString().Contains(searchString)
                || i.Name.ToLower().Contains(searchString.ToLower())
                || i.PhoneNumber.Contains(searchString)
                || i.Model.ToLower().Contains(searchString)
                || i.MasterName.Contains(searchString.ToLower());
            return result;
        }

        public ICommand SaveChangesCommand { get { return btnSave; } }
        #region commented
        //public string txtQuickSearch
        //{
        //    get
        //    {
        //        return quickSearchRequest;
        //    }
        //    set
        //    {
        //        quickSearchRequest = value;
        //        OnProrertyChanged("Clients");
        //    }
        //}
        //public string txtGlobalSearch
        //{
        //    get
        //    {
        //        return globalSearchRequest;
        //    }
        //    set
        //    {
        //        globalSearchRequest = value;
        //    }
        //}

        //public bool cbIsActiveOnly
        //{
        //    get
        //    {
        //        return isActive;
        //    }
        //    set
        //    {
        //        isActive = value;
        //        OnProrertyChanged("clients");
        //    }
        //}
        //public bool cbIsCurrentUserOnly
        //{
        //    get
        //    {
        //        return isCurrentUser;
        //    }
        //    set
        //    {
        //        isCurrentUser = value;
        //        OnProrertyChanged("clients");
        //    }
        //}

        //public IEnumerable<Client> clients
        //{
        //    get
        //    {
        //        return _client.GetSearchedFromLastClients(quickSearchRequest, isCurrentUser, isActive);
        //    }   
        //}

        //public IEnumerable<string> txtProblem
        //{
        //    get
        //    {
        //        return clients.Select(i => i.Problem);
        //    }
        //    set
        //    {
        //        DataGrid.SelectedItemProperty
        //    }
        //}
        #endregion


        public string quickSearch
        {
            get { return (string)GetValue(quickSearchProperty); }
            set { SetValue(quickSearchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for quickSerch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty quickSearchProperty =
            DependencyProperty.Register("quickSearch", typeof(string), typeof(ClientsWindowViewModel), new PropertyMetadata("", 
                OnQuickSearchChanged));

        private static void OnQuickSearchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ClientsWindowViewModel;
            if(current != null)
            {
                current.Clients.Filter = null;
                current.Clients.Filter = current.Filter;
            }
        }

        public ICollectionView Clients
        {
            get { return (ICollectionView)GetValue(clientsProperty); }
            set { SetValue(clientsProperty, value); }
        }

        private static readonly DependencyProperty clientsProperty =
            DependencyProperty.Register("Clients", typeof(ICollectionView), typeof(DataGrid),
                new PropertyMetadata(null));

      

        //    private static void OnProblemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //    {
        //        Debug.Write("funcking shit ;sfdlankl");
        //        var grid = d as DataGrid;
        //        var item = grid.CurrentCell.Item as Client;
        //        item.UpdClient(item);
        //    }
    }
}
