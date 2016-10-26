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
        private string globalSearchRequest;
        private bool isActive;
        private SaveChangesButtonCommand btnSave;
        private GlobalSearchCommand btnSearch;
        private List<Client> MyCollection { get; set; }
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

            btnSearch = new GlobalSearchCommand(() => { return true; }, GlobalSearching);


            globalSearchRequest = "";
            isActive = false;
            _client = new Client();

            myCollectionViewSource = new CollectionViewSource();
            myCollectionViewSource.Source = GlobalData.LastClients;
           
           
            Clients = myCollectionViewSource.View;
            Clients.Filter = Filter;
            
        }
        
        private void SaveChanges()
        {
            var items = Clients.OfType<Client>();
            var Ids = items.Select(i => i.Id).ToArray();
            var dbItems = _client.GetClientsByIds(Ids);
            int index = 0;
            string infoMsg = "Зміни успішно внесено до замовлень(ня) під номером:";
            bool show = false;
            foreach (var item in items)
            {
                if (!item.Equals(dbItems[index]))
                {
                    show = true;
                    string erorrMsg = $"Проблеми зі збереженням замовлення під номером {item.Id}. Перевірте поля:\n";
                    if (item.IsDone == true)
                    {
                        
                        if (item.GivingDate.Equals(null))
                            item.GivingDate = DateTime.Today;
                        if (item.Income==null || item.Income.Value == 0)
                        {
                            erorrMsg += "Прибуток\n";
                            MessageBox.Show(erorrMsg, "Помилка збереження", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                         
                    }
                    _client.UpdClient(item);
                    infoMsg += $" {item.Id},";
                }
                index ++;
            }
            if (show)
            {
                MessageBox.Show(infoMsg, "Зміни збережено", MessageBoxButton.OK, MessageBoxImage.Information);
                Clients.Refresh();
            }
        }
        public CollectionViewSource myCollectionViewSource;
        private void GlobalSearching()
        {
            if (globalSearchRequest == "")
                return;
            List<Client> searched = _client.GetSearchedFromDB(globalSearchRequest);
            if (searched == null || searched.Count == 0)
            {
                MessageBox.Show("Нажаль, замовлення не знайдено, перевірте пошуковий запит.", "Не знайдено",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            myCollectionViewSource.Source = searched;
            Clients = myCollectionViewSource.View;
            Clients.Filter = Filter;
            OnProrertyChanged("Clients");

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
            return isActive ? result && isActive != i.IsDone : result;
        }

        public ICommand SaveChangesCommand { get { return btnSave; } }
        public ICommand GlobalSearchCommand { get { return btnSearch; } }
        
        public bool IsActiveOnly
        {
            set
            {
                isActive = value;
                OnQuickSearchChanged(this, new DependencyPropertyChangedEventArgs());
            }
            get { return isActive; }
        }

        public string GlobalSearch
        {
            get { return globalSearchRequest; }
            set { globalSearchRequest = value; }
        }

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

     
    }
}
