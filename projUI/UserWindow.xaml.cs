using Microsoft.Win32;
using projUI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projUI
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            foreach (var w in App.Current.Windows)
            {
                try
                {
                    UserWindow t = w as UserWindow;
                    t.Close();
                }
                catch
                { }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           System.Windows.Forms.SaveFileDialog saveDb = new System.Windows.Forms.SaveFileDialog();
            saveDb.Filter = "SQL Server database backup files|*.bak";
            saveDb.Title = "Create Database Backup";
            try
            {
               
                saveDb.ShowDialog();
                using (DataContext db = new DataContext())
                {
                    string backupQuery = @"BACKUP DATABASE ""{0}"" TO DISK = N'{1}' WITH INIT";
                    backupQuery = string.Format(backupQuery,@"projUI.DataModels.DataContext", saveDb.FileName);
                    db.Database.SqlQuery<object>(backupQuery).ToList().FirstOrDefault();
                }
                System.Windows.MessageBox.Show($"Данні збережено у {saveDb.FileName}.", "succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch 
            {
                System.Windows.MessageBox.Show("Щось пішло не так, спробуйте ще раз.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog opf = new System.Windows.Forms.OpenFileDialog();
            opf.Filter = "SQL Server database backup files|*.bak";
            opf.Title = "Restore database backup";

            try
            {
                opf.ShowDialog();
                using (DataContext db = new DataModels.DataContext())
                {
                    string restoreQuery = @"USE [Master]; 
                                                ALTER DATABASE ""{0}"" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                                RESTORE DATABASE ""{0}"" FROM DISK='{1}' WITH RECOVERY;
                                                ALTER DATABASE ""{0}"" SET MULTI_USER;";
                    restoreQuery = string.Format(restoreQuery, @"projUI.DataModels.DataContext", opf.FileName);
                    var list = db.Database.SqlQuery<object>(restoreQuery).ToList();
                    var resut = list.FirstOrDefault();
                    System.Windows.MessageBox.Show($"Данні відновленно.", "succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch 
            {
                System.Windows.MessageBox.Show("Щось пішло не так, спробуйте ще раз.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
