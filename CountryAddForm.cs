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
    public partial class CountryAddForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int GetCountryId = 0;
        public CountryAddForm()
        {
            InitializeComponent();
        }

        // save button
        private void button1_Click(object sender, EventArgs e)
        {            
            string country = countryNameTxt.Text;
            string country_bangla = countryNameBanglaTxt.Text;

            try
            {
                if (country != "" && country_bangla != "")
                {
                    dBConn.Open();
                    string query = "exec [b2_country_insert_update_sp] @c_id, @c_name, @c_name_bangla";
                    SqlCommand command = new SqlCommand(query, dBConn);
                    command.Parameters.AddWithValue("@c_id", GetCountryId);
                    command.Parameters.AddWithValue("@c_name", country);
                    command.Parameters.AddWithValue("@c_name_bangla", country_bangla);
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
        // grid data load
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("SELECT *  FROM [dbo].[b2_country]", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }

        private void CountryAddForm_Load(object sender, EventArgs e)
        {
            GridDataLoad();
        }
        private void ClearData()
        {
            countryNameTxt.Clear();
            countryNameBanglaTxt.Clear();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "select * from b2_country where country_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        GetCountryId = int.Parse(reader["country_id"].ToString());
                        countryNameTxt.Text = reader["country"].ToString();
                        countryNameBanglaTxt.Text = reader["country_bangla"].ToString();
                    }
                }
            }
            dBConn.Close();
        }
    }
}
