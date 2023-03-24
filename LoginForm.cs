using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyTree
{
    public partial class LoginForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);

        public static int global_section;
        public static int global_userId;
        public static string global_user;
        public static int global_userRole;
        public static int global_unit;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = usernameTextBox.Text;
            string pass = passwordTextBox.Text;
            
            if (name == "" && pass == "")
            {
                MessageBox.Show("Empty Username or Password");
            }
            else
            {
                dBConn.Open();
                string query = "select user_id,section,username,user_role,building from b2_users where username = '" + name + "' and password = '" + pass + "' ";
                SqlCommand command = new SqlCommand(query, dBConn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // set global username and section name and role and unit 
                    global_section = int.Parse(reader["section"].ToString());
                    global_user = reader["username"].ToString();
                    global_userId = int.Parse(reader["user_id"].ToString());
                    global_userRole = int.Parse(reader["user_role"].ToString());
                    global_unit = int.Parse(reader["building"].ToString());

                    // dashboard                    
                    this.Hide();
                    Dashboard hs2 = new Dashboard();
                    hs2.Show();
                }
                else
                {
                    MessageBox.Show("Invalid login infornation");
                }
                dBConn.Close();
            }
        }

        private void usernameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                passwordTextBox.Focus();
            }
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                loginBtn.Focus();
            }

        }

        //private void loginBtn_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == Convert.ToChar(Keys.Enter))
        //    {
                //loginBtn.Focus();
                
                //string name = usernameTextBox.Text;
                //string pass = passwordTextBox.Text;
                //if (name == "" && pass == "")
                //{
                //    MessageBox.Show("Empty Username or Password");
                //}
                //else
                //{
                //    dBConn.Open();
                //    string query = "select section,username,user_role,building from b2_users where username = '" + name + "' and password = '" + pass + "' ";
                //    SqlCommand command = new SqlCommand(query, dBConn);
                //    SqlDataReader reader = command.ExecuteReader();
                //    if (reader.Read())
                //    {
                //        // set global username and section name and role and unit 
                //        global_section = int.Parse(reader["section"].ToString());
                //        global_user = reader["username"].ToString();
                //        global_userRole = int.Parse(reader["user_role"].ToString());
                //        global_unit = int.Parse(reader["building"].ToString());

                //        // dashboard                    
                //        this.Hide();
                //        Dashboard hs2 = new Dashboard();
                //        hs2.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Invalid login infornation");
                //    }
                //    dBConn.Close();
                //}
        //    }
        //}
    }
}

