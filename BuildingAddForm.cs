using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace SkyTree
{
    public partial class BuildingAddForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int BuildingId;
        public BuildingAddForm()
        {
            InitializeComponent();
        }
        private int GetCompanyId()
        {
            int FindCompanyId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select company_id from b2_company_info where company_name='"+companyNameTxt.Text+"'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                FindCompanyId = int.Parse(rd["company_id"].ToString());
            }
            dBConn.Close();
            return FindCompanyId;
        }
        private void LoadData()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select b2_building_info.building_id as Id, b2_company_info.company_name as Company, b2_building_info.building_name as 'Unit/Building' from b2_building_info inner join b2_company_info on b2_company_info.company_id = b2_building_info.company", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;            
        }
        private void LoadCompanyDetails()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select company_name from b2_company_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                companyNameTxt.Text = rd["company_name"].ToString();
            }            
            dBConn.Close();            
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string b_name = unitNameTxt.Text;

            int FindCompanyId;
            FindCompanyId = GetCompanyId();
          
            try
            {
                
                dBConn.Open();
                string query = "exec b2_building_insert_update_sp @id, @name, @com";
                SqlCommand command = new SqlCommand(query, dBConn);
                command.Parameters.AddWithValue("@id", BuildingId  == 0 ? 0 : BuildingId);
                command.Parameters.AddWithValue("@name", b_name);
                command.Parameters.AddWithValue("@com", FindCompanyId);
                command.ExecuteNonQuery();
                dBConn.Close();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BuildingAddForm_Load(object sender, EventArgs e)
        {
            LoadCompanyDetails();
            LoadData();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {            
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "select b2_building_info.building_id as Id, b2_company_info.company_name as Company, b2_building_info.building_name as Building from b2_building_info inner join b2_company_info on b2_company_info.company_id = b2_building_info.company where b2_building_info.building_id = '" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        BuildingId = int.Parse(reader["Id"].ToString());
                        companyNameTxt.Text = reader["Company"].ToString();
                        unitNameTxt.Text = reader["Building"].ToString();
                    }
                }
            }
            dBConn.Close();
        }
    }
}
