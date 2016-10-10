using projUI.Constants;
using projUI.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projUI.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Model { get; set; }
        public string Problem { get; set; }
        public int Cost { get; set; }
        public int? Income { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime? GivingDate { get; set; }
        public string MasterName { get; set; }
        public bool IsDone { get; set; }

        public virtual User Master { get; set; }
        private DataContext db;
        public override bool Equals(object obj)
        {
            var arg = obj as Client;
            if (arg == null) return false;
            bool result = this.Id == arg.Id
                && this.Name == arg.Name
                && this.MasterName == arg.MasterName
                && this.Income == arg.Income
                && this.ReceptionDate == arg.ReceptionDate
                && this.GivingDate.Equals(arg.GivingDate)
                && this.Problem == arg.Problem
                && this.IsDone == arg.IsDone;
            return result;
        }
        public Client()
        {
            this.IsDone = false;
            db = new DataContext();
        }

        public List<Client> GetLastClients(int clientsCount)
        {
            int maxId = db.Clients.Select(i => i.Id).Max();
            return db.Clients.Where(i => (maxId - i.Id) <= clientsCount).ToList();
        }
        /// <summary>
        /// Gets done orders for a current month.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IQueryable<Client> GetDoneOrders(User user)
        {
            return db.Clients.Where(i => i.MasterName.Equals(user.Name) && i.IsDone
            && (DateTime.Today.Month == i.GivingDate.Value.Month));
            
        }

        public Client GetClientById(int Id)
        {
           return db.Clients.Where(i => i.Id == Id).AsEnumerable().First();
        }
        public int GetDoneOrdersCount(User user)
        {
            return GetDoneOrders(user).Count();
        }

        private IQueryable<Client> GetDoneOrdersForTime(User user, DateTime from, DateTime to)
        {
            return db.Clients.Where(i => i.MasterName.Equals(user.Name) && i.IsDone
                && ((DateTime.Compare(from, i.GivingDate.Value) < 0 || DateTime.Compare(from, i.GivingDate.Value) == 0)
                && (DateTime.Compare(to, i.GivingDate.Value) > 0 || DateTime.Compare(to, i.GivingDate.Value) == 0)));

        }

        public int GetOrdersCountForTime(User user, DateTime from, DateTime to)
        {
            return GetDoneOrdersForTime(user, from, to).Count();
        }
        

        public List<Client> GetSearchedFromLastClients(string searchString, bool isUserOnly, bool isActiveOnly)
        {
            var currentClients = GlobalData.LastClients;
            if (searchString != null && searchString != "")
                currentClients = currentClients.Where(i => i.Id.ToString().Contains(searchString)
                || i.Name.ToLower().Contains(searchString.ToLower())
                || i.PhoneNumber.Contains(searchString)
                || i.Model.ToLower().Contains(searchString)
                || i.MasterName.Contains(searchString.ToLower())).ToList();
            if (isUserOnly)
                currentClients = currentClients.Where(i => i.MasterName.Equals(GlobalData.CurrentUser.Name)).ToList();
            if (isActiveOnly)
                currentClients = currentClients.Where(i => i.IsDone != true).ToList();
            return currentClients;
        }

        

        public List<Client> GetSearchedFromDB(string searchString)
        {
            return db.Clients.Where(i => i.Id.ToString().Contains(searchString)
            || i.Name.ToLower().Contains(searchString.ToLower())
            || i.PhoneNumber.Contains(searchString)).ToList();
        }

        public int GetIncome(User user)
        {
            return GetDoneOrders(user).ToList().Sum(i => i.Income.Value);
        }

        public int GetCostsForTime(User user, DateTime from, DateTime to)
        {
            return GetDoneOrdersForTime(user, from, to).AsEnumerable().Select(i => i.Cost).Sum(); 
        }

        public int GetIncomeForTime(User user, DateTime from, DateTime to)
        {
            return GetDoneOrdersForTime(user, from, to).AsEnumerable().Select(i => i.Income.Value).Sum();
        }

        public int GetNextClientId()
        {
            return db.Clients != null ? db.Clients.Select(i => i.Id).Max() + 1 : 10001;
        }

        public bool IsNameValid()
        {
            if(this.Name != null && this.Name.Trim().Length > 3 && this.Name.Trim().Length < 26)
            {
                return true;
            }
            return false;
        }
        public bool IsPhoneNumberValid()
        {
            string PhonePattern = @"^\+[1-9]{1}[0-9]{3,14}$";
            return Regex.IsMatch(this.PhoneNumber, PhonePattern);
        }

        public bool IsModelvalid()
        {
            return this.Model != null && this.Model.Trim() != "";
        }
        public bool IsProblemValid()
        {
            return this.Problem != null && this.Problem.Trim() != "";
        }

        public bool IsCostValid()
        {
            string CostPattern = @"^[1-9]{1}[0-9]{1,6}$";
            return Regex.IsMatch(this.Cost.ToString(), CostPattern);
        }

        public bool IsIncomeValid()
        {
            string IncomePattern = @"^[1-9]{1}[0-9]{1,6}$";
            return this.Income == null || Regex.IsMatch(this.Income.Value.ToString(), IncomePattern);
        }
        public bool IsReceptionDateValid()
        {
            return DateTime.Compare(this.ReceptionDate, DateTime.Today) == 0 
                || DateTime.Compare(this.ReceptionDate, DateTime.Today) < 0;
        }
        public bool IsClientValid()
        {
            return IsNameValid() && IsModelvalid() && IsIncomeValid()
                && IsCostValid() && IsPhoneNumberValid() && IsProblemValid() && IsReceptionDateValid();
        }
        public void AddClient()
        {
            db.Clients.Add(this);
            db.SaveChanges();
            Debug.WriteLine("succes");
        }

        public void UpdClient(Client client)
        {
            var old = db.Clients.Where(i => i.Id == client.Id).ToArray()[0];
            old.MasterName = client.MasterName;
            old.Problem = client.Problem;
            old.Income = client.Income;
            old.GivingDate = client.GivingDate.Value;
            old.IsDone = client.IsDone;
            db.SaveChanges();
        }
        
    }
}
