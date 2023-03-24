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
    public partial class UserRegisterForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int GetUserId;
        public UserRegisterForm()
        {
            InitializeComponent();
        }
        // get building id 
        private int GetBuildingId()
        {
            int buildingId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select building_id from b2_building_info where building_name='" + buildingComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                buildingId = int.Parse(rd["building_id"].ToString());
            }
            dBConn.Close();
            return buildingId;
        }
        // get section id
        private int GetSectionId()
        {
            int SectionId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select section_id from b2_section_info where section='" + sectionComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                SectionId = int.Parse(rd["section_id"].ToString());
            }
            dBConn.Close();
            return SectionId;
        }
        // get user role type 
        private int GetUserRoleType()
        {
            int UserRoleType = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select role_id from b2_user_role where role_name='" + roleTypeComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                UserRoleType = int.Parse(rd["role_id"].ToString());
            }
            dBConn.Close();
            return UserRoleType;
        }
        // grid data load
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("SELECT *  FROM [dbo].[vw_user_data]", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }
        // building load
        private void BuildingLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select building_name from b2_building_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                buildingComboBox.Items.Add(rd["building_name"].ToString());
            }
            dBConn.Close();
        }
        // section load
        private void SectionLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select section from b2_section_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                sectionComboBox.Items.Add(rd["section"].ToString());
            }
            dBConn.Close();
        }
        // user role type load
        private void UserTypeLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select role_name from b2_user_role", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                roleTypeComboBox.Items.Add(rd["role_name"].ToString());
            }
            dBConn.Close();
        }

        private void ClearData()
        {
            nameTxtBox.Clear();
            usernameTxtBox.Clear();
            passwordTxtBox.Clear();
            roleTypeComboBox.Text = "";
            sectionComboBox.Text = "";
            buildingComboBox.Text = "";
        }
        private void UserRegisterForm_Load(object sender, EventArgs e)
        {
            GridDataLoad();
            BuildingLoad();
            SectionLoad();
            UserTypeLoad();
        }

        // edit button
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "exec [b2_user_select_sp] @UId='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        GetUserId = int.Parse(reader["Id"].ToString());
                        buildingComboBox.Text = reader["Building"].ToString();
                        sectionComboBox.Text = reader["Section"].ToString();
                        nameTxtBox.Text = reader["Name"].ToString();
                        usernameTxtBox.Text = reader["Username"].ToString();
                        passwordTxtBox.Text = reader["Password"].ToString();
                        roleTypeComboBox.Text = reader["Type"].ToString();
                    }
                }
            }
            dBConn.Close();
        }
        // save button
        private void button1_Click(object sender, EventArgs e) 
        {
            int B_Id, S_id, R_id;
            B_Id=GetBuildingId();
            S_id=GetSectionId();
            R_id=GetUserRoleType();

            try
            {
                if(nameTxtBox.Text != "" && usernameTxtBox.Text !="" && passwordTxtBox.Text != "")
                {
                    dBConn.Open();
                    string query = "exec [b2_user_insert_update_sp] @u_id,@b_id,@s_id,@name,@user,@pass,@role,@time";
                    SqlCommand command = new SqlCommand(query, dBConn);
                    command.Parameters.AddWithValue("@u_id", GetUserId == 0 ? 0 : GetUserId);
                    command.Parameters.AddWithValue("@b_id", B_Id);
                    command.Parameters.AddWithValue("@s_id", S_id);
                    command.Parameters.AddWithValue("@name", nameTxtBox.Text);
                    command.Parameters.AddWithValue("@user", usernameTxtBox.Text);
                    command.Parameters.AddWithValue("@pass", passwordTxtBox.Text);
                    command.Parameters.AddWithValue("@role", R_id);
                    command.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                    dBConn.Close();
                    GridDataLoad();
                    ClearData();
                } else
                {
                    MessageBox.Show("Select all fields accordingly","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
