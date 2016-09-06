using projUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projUI.Constants
{
    public struct GlobalData
    {
        public static User CurrentUser { get; set; }
        public static List<User> UsersList { get; set; }
        public static List<Client> LastClients { get; set; }
    }
}
