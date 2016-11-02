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
using System.Windows;

namespace projUI.Models
{
    public class User
    {
        [Key]
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

       
        public virtual List<Client> Clients { get; set; }
        /// <summary>
        /// For geting user list
        /// </summary>
        /// <returns>full list of users</returns>

        private DataContext db = new DataContext();
        public List<User> GetUsers()
        {
            return db.Users.ToList();
        }
        /// <summary>
        /// Verify user.
        /// </summary>
        /// <returns></returns>
        public bool IsUserExist()
        {
            var users = GetUsers();
            foreach (var item in users)
            {
                if (item.Name.Equals(this.Name) && item.Password.Equals(this.Password))
                {
                    this.IsAdmin = item.IsAdmin;
                    return true;
                }
                    
            }
            Debug.WriteLine("fail");
            return false;
        }
        public bool IsNameValid()
        {
            string namePattern = @"^[a-z]{1,20}$";
            List<User> users = this.GetUsers();
            bool exist = users == null ? true : !users.Any(u => u.Name.Equals(this.Name));
            if (this.Name != "" && this.Name != null && exist
                && Regex.IsMatch(this.Name, namePattern))
            {
                return true;
            }
            return false;
        }

        public bool IsPasswordValid()
        {
            string passwordPattern = @"^[a-z0-9]{4,8}$";
            if (this.Password != "" && this.Password != null && Regex.IsMatch(this.Password, passwordPattern))
            {
                return true;
            }
            return false;
        }

        public bool IsUserValid()
        {
            return IsNameValid() && IsPasswordValid();
        }
        
        public bool AddUser()
        {
            if (IsUserValid())
            {
                db.Users.Add(this);
                db.SaveChanges();
                return true;
            }
            return false;
               
        }

    }
}
