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
    public partial class BuyerAddForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int BuyerId = 0;
        public BuyerAddForm()
        {
            InitializeComponent();
        }
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select * from b2_buyer_info", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["execution_time"].Visible = false;
        }
        private int GetCountryId()
        {
            int SectionId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select country_id from b2_country where country='" + countryComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                SectionId = int.Parse(rd["country_id"].ToString());
            }
            dBConn.Close();
            return SectionId;
        }
        private void CountryLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select country from b2_country order by country", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                countryComboBox.Items.Add(rd["country"].ToString());
            }
            dBConn.Close();
        }

        private void BuyerAddForm_Load(object sender, EventArgs e)
        {
            CountryLoad();
            GridDataLoad();
        }
        //save data
        private void button1_Click(object sender, EventArgs e)
        {
            int b_id = BuyerId;
            int country = GetCountryId();

            string buyer = buyerNameTxt.Text.ToUpper();            
            string email = emailTxt.Text;
            string phone = phoneTxt.Text;
            string address = addressTxt.Text;

            try
            {
                if (buyer != "" && email != "" && phone != "" && address != "")
                {
                    dBConn.Open();
                    string query = "exec [b2_buyer_insert_sp] @b_id,@buyer,@country,@email,@phone,@address,@time";
                    SqlCommand command = new SqlCommand(query, dBConn);
                    command.Parameters.AddWithValue("@b_id", b_id);
                    command.Parameters.AddWithValue("@buyer", buyer);
                    command.Parameters.AddWithValue("@country", country);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                    dBConn.Close();
                    GridDataLoad();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("Select all fields accordingly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void ClearData()
        {
            buyerNameTxt.Clear();
            phoneTxt.Clear();
            emailTxt.Clear();
            addressTxt.Clear();
            countryComboBox.Text = "";            
        }

        //select data for edit purpose
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "exec [b2_buyer_select_sp] @b_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        BuyerId = int.Parse(reader["buyer_id"].ToString());
                        buyerNameTxt.Text = reader["buyer"].ToString();
                        emailTxt.Text = reader["email"].ToString();
                        phoneTxt.Text = reader["phone"].ToString();
                        addressTxt.Text = reader["address"].ToString();
                        countryComboBox.Text = reader["country"].ToString();                        
                    }
                }
            }
            dBConn.Close();
        }
    }
}
