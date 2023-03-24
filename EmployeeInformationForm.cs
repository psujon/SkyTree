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
    public partial class EmployeeInformationForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int EmployeeId;
        public EmployeeInformationForm()
        {
            InitializeComponent();
        }
        private void UnitBuildingLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select building_name from b2_building_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                unitComboBox.Items.Add(rd["building_name"].ToString());
            }
            dBConn.Close();
        }
        private void DepartmentLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select department from b2_department_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                departComboBox.Items.Add(rd["department"].ToString());
            }
            dBConn.Close();
        }
        private int GetUnitId()
        {
            int UnitId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select building_id from b2_building_info where building_name='" + unitComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                UnitId = int.Parse(rd["building_id"].ToString());
            }
            dBConn.Close();
            return UnitId;
        }
        private int GetDepartmentId()
        {
            int DeptId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select deptId from b2_department_info where department='" + departComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                DeptId = int.Parse(rd["deptId"].ToString());
            }
            dBConn.Close();
            return DeptId;
        }
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
        private int GetBlockId()
        {
            int BlockId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select blockId from b2_block_info where block='" + blockComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                BlockId = int.Parse(rd["blockId"].ToString());
            }
            dBConn.Close();
            return BlockId;
        }
        private int GetDesignationId()
        {
            int desigId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select desigId from b2_designation_info where designation='" + designationComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();            
            while (rd.Read())
            {
                desigId = int.Parse(rd["desigId"].ToString());
            }
            dBConn.Close();
            return desigId;
        }
        private int GetShiftId()
        {
            int shiftId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select shiftId from b2_shift_info where shift='" + shiftComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                shiftId = int.Parse(rd["shiftId"].ToString());
            }
            dBConn.Close();
            return shiftId;
        }
        private void SectionLoad()
        {
            sectionComboBox.Items.Clear();

            int deptId = GetDepartmentId();

            dBConn.Open();
            SqlCommand command = new SqlCommand("select section from b2_section_info where department='"+deptId+"'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                sectionComboBox.Items.Add(rd["section"].ToString());
            }
            dBConn.Close();
        }
        private void BlockLoad()
        {           

            dBConn.Open();
            SqlCommand command = new SqlCommand("select block from b2_block_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                blockComboBox.Items.Add(rd["block"].ToString());
            }
            dBConn.Close();
        }
        private void DesignationLoad()
        {

            dBConn.Open();
            SqlCommand command = new SqlCommand("select designation from b2_designation_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                designationComboBox.Items.Add(rd["designation"].ToString());
            }
            dBConn.Close();
        }
        private void ShiftLoad()
        {

            dBConn.Open();
            SqlCommand command = new SqlCommand("select shift from b2_shift_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                shiftComboBox.Items.Add(rd["shift"].ToString());
            }
            dBConn.Close();
        }

        private void EmployeeInformationForm_Load(object sender, EventArgs e)
        {
            UnitBuildingLoad();
            DepartmentLoad();
            BlockLoad();
            DesignationLoad();
            ShiftLoad();
        }

        private void departComboBox_Leave(object sender, EventArgs e)
        {
            SectionLoad();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int EmployeeID = EmployeeId == 0 ? 0 : EmployeeId;
            int unitId = GetUnitId();
            string emp_cardno = empIdTxtBox.Text.ToUpper();
            string empName = empNameTxtBox.Text;
            string empNameBangla = empNameTextBangla.Text;
            int deptId = GetDepartmentId();
            int secId = GetSectionId();
            int lineId = GetBlockId();
            int empType;
            if(sromikTypeComboBox.Text == "Permanent") { empType = 0; } else { empType = 1; }
            int desigId = GetDesignationId();
            int shiftId = GetShiftId();
            int gradeId = gradeTextBox.Text == "" ? 0 : int.Parse(gradeTextBox.Text);
            int isActive = 0;
            
            if(unitId!=0 && emp_cardno!="" && empName!="" && deptId!=0 && secId!=0)
            {
                dBConn.Open();
                string query = "exec [b2_emp_insert_update_sp] @empId,@cardno,@empName,@empNameBan,@sec,@desig,@isActive,@unit,@dept,@block,@empType,@shift,@grade,@jd,@times";
                SqlCommand command = new SqlCommand(query, dBConn);
                command.Parameters.AddWithValue("@empId", EmployeeID);
                command.Parameters.AddWithValue("@cardno", emp_cardno);
                command.Parameters.AddWithValue("@empName", empName);
                command.Parameters.AddWithValue("@empNameBan", empNameBangla);
                command.Parameters.AddWithValue("@sec", secId);
                command.Parameters.AddWithValue("@desig", desigId);
                command.Parameters.AddWithValue("@isActive", isActive);
                command.Parameters.AddWithValue("@unit", unitId);
                command.Parameters.AddWithValue("@dept", deptId);
                command.Parameters.AddWithValue("@block", lineId);
                command.Parameters.AddWithValue("@empType", empType);
                command.Parameters.AddWithValue("@shift", shiftId);
                command.Parameters.AddWithValue("@grade", gradeId);
                command.Parameters.AddWithValue("@jd", joiningDate.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                dBConn.Close();
            } else
            {
                MessageBox.Show("All fields required");
            }
        }

        private void empIdTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                string empCardno = empIdTxtBox.Text.ToUpper();
                if (empCardno != "")
                {
                    dBConn.Open();
                    string query = "exec [b2_emp_select_sp] @cardno='" + empCardno + "'";
                    SqlCommand command = new SqlCommand(query, dBConn);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // set global username and section name and role and unit 
                        EmployeeId = int.Parse(reader["emp_id"].ToString());
                        unitComboBox.Text = reader["building_name"].ToString();
                        empNameTxtBox.Text = reader["emp_name"].ToString();
                        empNameTextBangla.Text = reader["emp_name_bangla"].ToString();
                        departComboBox.Text = reader["department"].ToString();
                        sectionComboBox.Text = reader["section"].ToString();
                        blockComboBox.Text = reader["block"].ToString();
                        string empType = reader["sromikType"].ToString();
                        if (empType == "1")
                        {
                            sromikTypeComboBox.Text = "Permanent";
                        } else
                        {
                            sromikTypeComboBox.Text = "Temporary";
                        }
                        designationComboBox.Text = reader["designation"].ToString();
                        shiftComboBox.Text = reader["shift"].ToString();
                        gradeTextBox.Text = reader["grade"].ToString();
                        joiningDate.Text = reader["joiningDate"].ToString();
                        string status = reader["IsActive"].ToString();
                        if (status == "0")
                        {
                            activeStatusText.Text = "Active";
                        } else if(status=="1")
                        {
                            activeStatusText.Text = "Inactive";
                        } else
                        {
                            activeStatusText.Text = "";
                        }
                    }
                    dBConn.Close();
                }
                else
                {
                    ClearFields();
                }

            }
        }
        private void ClearFields()
        {
            unitComboBox.Text = "";
            empNameTxtBox.Clear();
            empNameTextBangla.Clear();
            departComboBox.Text = "";
            sectionComboBox.Text = "";
            blockComboBox.Text = "";
            sromikTypeComboBox.Text = "";
            shiftComboBox.Text = "";
            designationComboBox.Text = "";
            gradeTextBox.Clear();
            joiningDate.ResetText();
        }

        private void designationComboBox_Leave(object sender, EventArgs e)
        {
            gradeTextBox.Clear();
            string desig = designationComboBox.Text;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select salary_grade from b2_designation_info where designation='"+desig+"'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                gradeTextBox.Text = rd["salary_grade"].ToString();
            }
            dBConn.Close();
        }
    }
}
