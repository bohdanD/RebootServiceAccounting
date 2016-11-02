using projUI.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace projUI.Models
{
    public class Spending
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }


        // override object.Equals
        public override bool Equals(object obj)
        {
            Spending spend = obj as Spending;
            return spend != null
                && spend.Id == Id
                && Name.Equals(spend.Name)
                && Cost == spend.Cost
                && Date.Equals(spend.Date);
            
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int GetLastId()
        {
            using (var db = new DataContext())
            {
                return db.Spendings.Count() != 0 ? db.Spendings.Select(i => i.Id).Max() : 0;
            }
        }


        public void Add()
        {
            using (var db = new DataContext())
            {
                if (this!=null)
                {
                    this.Id = this.GetLastId() + 1;
                    db.Spendings.Add(this);
                    db.SaveChanges();
                }
            }
        }

        private IQueryable<Spending> GetSpendingsByDate(DateTime from, DateTime to, DataContext db)
        {
            return db.Spendings.Where(i => i.Date.CompareTo(from) >= 0 && i.Date.CompareTo(to) <= 0);
        }

        public List<Spending> GetSpendingsByDate(DateTime from, DateTime to)
        {
            using (var db = new DataContext())
            {
               return GetSpendingsByDate(from, to, db).ToList();
            }
        }

        public List<Spending> GetCurrentMonthSpendings()
        {
            DateTime from = DateTime.Today.AddDays(1-DateTime.Today.Day);
            
            using (var db = new DataContext())
            {
                return GetSpendingsByDate(from, DateTime.Today, db).ToList();
            }
        }

        public int GetSpendingCostsByDate(DateTime from, DateTime to)
        {
            using (var db = new DataContext())
            {
                return GetSpendingsByDate(from, to, db).AsEnumerable().Select(i => i.Cost).Sum();
            }
        }

        public List<Spending> GetSpendingsByIds(int[] Ids)
        {
            using (DataContext db = new DataContext())
            {
                return db.Spendings.Where(i => Ids.Contains(i.Id)).ToList();
            }
        }

        public void Update()
        {
            using (var db = new DataContext())
            {
                var old = db.Spendings.Where(i => i.Id == this.Id).First();
                old.Name = this.Name;
                old.Cost = this.Cost;
                old.Date = this.Date;

                db.SaveChanges();
            }
        }

    }
}
