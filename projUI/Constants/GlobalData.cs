using projUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projUI.Constants
{
    public class GlobalData
    {
        private GlobalData() { }
        static GlobalData()
        {
            _users = new User().GetUsers();
            _lastClients = new Client().GetLastClients(10);
        }
        private static List<Client> _lastClients;
        private static List<User> _users;
        public static User CurrentUser { get; set; }
        public static List<User> UsersList { get { return _users; } }
        public static List<Client> LastClients { get { return _lastClients; } set { _lastClients = value; } }
    }
}
