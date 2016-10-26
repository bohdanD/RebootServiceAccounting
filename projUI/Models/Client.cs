using projUI.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace projUI.Models
{
    public class Client
    {
        #region Properties
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
        #endregion

        #region ctor & Equals
        public override bool Equals(object obj)
        {
            var arg = obj as Client;
            if (arg == null) return false;
            bool result = this.Id == arg.Id
                && this.Name == arg.Name
                && this.MasterName == arg.MasterName
                && this.Cost == arg.Cost
                && this.Income == arg.Income
                && this.ReceptionDate.Equals(arg.ReceptionDate)
                && this.GivingDate.Equals(arg.GivingDate)
                && this.Problem == arg.Problem
                && this.IsDone == arg.IsDone;
            return result;
        }
        public Client()
        {
            this.IsDone = false;
        }
        #endregion

        #region Clients
        public List<Client> GetLastClients(int clientsCount)
        {
            using (DataContext db = new DataContext())
            {
                int maxId = db.Clients.Select(i => i.Id).Max();
                return db.Clients.Where(i => (maxId - i.Id) <= clientsCount).ToList();
            }
            
        }
        public List<Client> GetClientsByIds(int[] Id)
        {
            using (DataContext db = new DataContext())
                return db.Clients.Where(i => Id.Contains(i.Id)).AsEnumerable().ToList();
        }
        public List<Client> GetSearchedFromDB(string searchString)
        {
            using (DataContext db = new DataContext())
                return db.Clients.Where(i => i.Id.ToString().Contains(searchString)
                || i.Name.ToLower().Contains(searchString.ToLower())
                || i.PhoneNumber.Contains(searchString)).ToList();
        }
        public int GetNextClientId()
        {
            using (DataContext db = new DataContext())
                return db.Clients != null ? db.Clients.Select(i => i.Id).Max() + 1 : 10001;
        }
        #endregion

        #region IQueryable requests
        private IQueryable<Client> GetDoneOrders(User user, DataContext innerdb)
        {
            return innerdb.Clients.Where(i => i.MasterName.Equals(user.Name) && i.IsDone
            && (DateTime.Today.Month == i.GivingDate.Value.Month));   
        }

        private IQueryable<Client> GetDoneOrders(DataContext innerdb)
        {
            return innerdb.Clients.Where(i => i.IsDone
            && (DateTime.Today.Month == i.GivingDate.Value.Month));
        }

        private IQueryable<Client> GetDoneOrdersForTime(User user, DateTime from, DateTime to, DataContext innerdb)
        {
            return innerdb.Clients.Where(i => i.MasterName.Equals(user.Name) && i.IsDone
                && ((DateTime.Compare(from, i.GivingDate.Value) <= 0)
                && (DateTime.Compare(to, i.GivingDate.Value) >= 0)));
        }

        private IQueryable<Client> GetDoneOrdersForTime(DateTime from, DateTime to, DataContext innerdb)
        {
            return innerdb.Clients.Where(i => i.IsDone
                && ((DateTime.Compare(from, i.GivingDate.Value) <= 0)
                && (DateTime.Compare(to, i.GivingDate.Value) >= 0)));
        }
        #endregion

        #region Orders count
        public int GetDoneOrdersCount(User user)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrders(user, db).Count();
        }


        public int GetOrdersCountForTime(User user, DateTime from, DateTime to)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrdersForTime(user, from, to, db).Count();
        }

        public int GetOrdersCountForTime(DateTime from, DateTime to)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrdersForTime(from, to, db).Count();
        }

        #endregion

        #region Incomes
        public int GetIncome(User user)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrders(user, db).ToList().Sum(i => i.Income.Value);
        }
        public int GetIncome()
        {
            using (DataContext db = new DataContext())
                return GetDoneOrders(db).ToList().Sum(i => i.Income.Value);
        }

        public int GetIncomeForTime(User user, DateTime from, DateTime to)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrdersForTime(user, from, to, db).AsEnumerable().Select(i => i.Income.Value).Sum();
        }
        public int GetIncomeForTime(DateTime from, DateTime to)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrdersForTime(from, to, db).AsEnumerable().Select(i => i.Income.Value).Sum();
        }
        #endregion

        #region Costs
        public int GetCostsForTime(User user, DateTime from, DateTime to)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrdersForTime(user, from, to, db).AsEnumerable().Select(i => i.Cost).Sum(); 
        }
        public int GetCostsForTime(DateTime from, DateTime to)
        {
            using (DataContext db = new DataContext())
                return GetDoneOrdersForTime(from, to, db).AsEnumerable().Select(i => i.Cost).Sum();
        }
        #endregion

        #region Validation
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
        #endregion

        #region Add or Upd
        public void AddClient()
        {
            using (DataContext db = new DataContext())
            {
                db.Clients.Add(this);
                db.SaveChanges();
            }

        }

        public void UpdClient(Client client)
        {
            using (DataContext db = new DataContext())
            {
                var old = db.Clients.Where(i => i.Id == client.Id).ToArray()[0];
                old.MasterName = client.MasterName;
                old.Problem = client.Problem;
                old.Cost = client.Cost;
                old.Income = client.Income;
                old.GivingDate = client.GivingDate;
                old.IsDone = client.IsDone;
                db.SaveChanges();
            }     
        }
        #endregion

    }
}
