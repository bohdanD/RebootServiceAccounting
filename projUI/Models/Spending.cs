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

        public int GetLastId()
        {
            using (var db = new DataContext())
            {
                return db.Spendings != null ? db.Spendings.Select(i => i.Id).Last() : 0;
            }
        }


        public void Add()
        {
            using (var db = new DataContext())
            {
                if (this!=null)
                {
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

        public int GetSpendingCostsByDate(DateTime from, DateTime to)
        {
            using (var db = new DataContext())
            {
                return GetSpendingsByDate(from, to, db).AsEnumerable().Select(i => i.Cost).Sum();
            }
        }
    }
}
