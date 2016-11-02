using projUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (CheckDataBaseExist())
            {
                MessageBox.Show("");
            }
            else
                GenerateDataBase();

            DataContext = new MainWindowViewModel();
        }



        private bool CheckDataBaseExist()
        {
            SqlConnection Connection = new SqlConnection(@"data source = (LocalDb)\MSSQLLocalDB; initial catalog = projUI.DataModels.DataContext; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework");
            try
            {
                Connection.Open();
                return true;
            }
            catch 
            {
                return false;
            }
        }




        private void GenerateDataBase()
        {
            List<string> cmds = new List<string>();
            string script = System.AppDomain.CurrentDomain.BaseDirectory + "\\script.sql";
            if (File.Exists(script))
            {
                TextReader tr = new StreamReader(script);
                string line = "";
                string cmd = "";
                while ((line = tr.ReadLine()) != null)
                {
                    if (line.Trim().ToUpper() == "GO")
                    {
                        cmds.Add(cmd);
                        cmd = "";
                    }
                    else
                    {
                        cmd += line = "\r\n";
                    }
                }
                if (cmd.Length>0)
                {
                    cmds.Add(cmd);
                    cmd = "";
                }
                tr.Close();
            }
            if (cmds.Count > 0)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
                command.CommandType = System.Data.CommandType.Text;
                command.Connection.Open();
                for (int i = 0; i < cmds.Count; i++)
                {
                    command.CommandText = cmds[i];
                    command.ExecuteNonQuery();
                }
            }
        }









        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
