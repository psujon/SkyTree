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
    public partial class FloorProductionEntryForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int FloorProductionId = 0;

        int global_section = LoginForm.global_section;
        string global_user = LoginForm.global_user;
        int global_userRole = LoginForm.global_userRole;
        int global_unit = LoginForm.global_unit;
        public FloorProductionEntryForm()
        {
            InitializeComponent();
        }

        private void FloorProductionEntryForm_Load(object sender, EventArgs e)
        {
            GridDataLoad();
            if (global_userRole == 1) // admin
            {
                SectionLoad();
            } else // operator
            {
                sectionComboBox.Visible = false;
                StyleLoad();
            }            
        }
        private void GridDataLoad()
        {
            if(global_userRole == 1) // admin
            {
                dBConn.Open();
                SqlCommand command = new SqlCommand("select top(10) fpl.prod_id as Id, fpl.prod_date as Date, sii.section as Section, e.emp_cardno, si.style as Style, pii.process_name as 'Size/Process', fpl.quantity as Quantity from [b2_floor_production_list] as fpl inner join b2_emp_basic as e on e.emp_id=fpl.emp_id inner join b2_style_info as si on si.style_id= fpl.style inner join b2_process_info as pii on pii.process_id=fpl.process inner join b2_section_info as sii on sii.section_id=fpl.section", dBConn);
                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                dBConn.Close();
                dataGridView1.DataSource = dt;
            } else // operator
            {
                dBConn.Open();
                SqlCommand command = new SqlCommand("select top(10) fpl.prod_id as Id, fpl.prod_date as Date, sii.section as Section, e.emp_cardno, si.style as Style, pii.process_name as 'Size/Process', fpl.quantity as Quantity from [b2_floor_production_list] as fpl inner join b2_emp_basic as e on e.emp_id=fpl.emp_id inner join b2_style_info as si on si.style_id= fpl.style inner join b2_process_info as pii on pii.process_id=fpl.process inner join b2_section_info as sii on sii.section_id=fpl.section where fpl.section='" + global_section + "'", dBConn);
                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                dBConn.Close();
                dataGridView1.DataSource = dt;
            }
            
        }
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
        private void StyleLoad() // only for operator
        {
            int sec_id = global_section;                
            dBConn.Open();
            SqlCommand command = new SqlCommand("select style from b2_style_info where section='" + sec_id + "' group by style order by style", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                styleComboBox.Items.Add(rd["style"].ToString());
            }
            dBConn.Close();
        }
        private void AdminStyleLoad()
        {
            styleComboBox.Items.Clear();
            int sec_id = GetSectionId();
            dBConn.Open();
            SqlCommand command = new SqlCommand("select style from b2_style_info where section='" + sec_id + "' group by style order by style", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                styleComboBox.Items.Add(rd["style"].ToString());
            }
            dBConn.Close();
        }
        private void ProcessLoad()
        {
            processComboBox.Items.Clear();
            int sec_id;
            if (global_userRole == 1)
            {
                sec_id = GetSectionId();
            } else
            {
                sec_id = global_section;
            }
            
            int style_id = GetStyleId();

            dBConn.Open();
            SqlCommand command = new SqlCommand("select process_name from b2_process_info where section='" + sec_id + "' and style='" + style_id + "' group by process_name order by process_name", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                processComboBox.Items.Add(rd["process_name"].ToString());
            }
            dBConn.Close();
        }
        private int GetEmployeeId()
        {
            int empId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select emp_id from b2_emp_basic where emp_cardno='" + empIdTextBox.Text.ToUpper() + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                empId = int.Parse(rd["emp_id"].ToString());
            }
            dBConn.Close();
            return empId;
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
        private int GetStyleId()
        {
            int StyleId = 0;
            int sec_id;
            if(global_userRole == 1)
            {
                sec_id = GetSectionId();
            } else
            {
                sec_id = global_section;
            }
            dBConn.Open();
            SqlCommand command = new SqlCommand("select style_id from b2_style_info where section='"+ sec_id + "' and style='" + styleComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                StyleId = int.Parse(rd["style_id"].ToString());
            }
            dBConn.Close();
            return StyleId;
        }
        private int GetProcessId()
        {
            int ProcessId = 0;

            int sec_id;
            if (global_userRole == 1)
            {
                sec_id = GetSectionId();
            } else
            {
                sec_id = global_section;
            }
            int style_id = GetStyleId();
            string process = processComboBox.Text;

            dBConn.Open();
            SqlCommand command = new SqlCommand("select process_id from b2_process_info where section='" + sec_id + "' and style='" + style_id + "' and process_name='" + process + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                ProcessId = int.Parse(rd["process_id"].ToString());
            }
            dBConn.Close();
            return ProcessId;
        }
        private int GetUserId()
        {
            int UserId = 0;            
            string user = global_user;

            dBConn.Open();
            SqlCommand command = new SqlCommand("select user_id from b2_users where username='" + user + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                UserId = int.Parse(rd["user_id"].ToString());
            }
            dBConn.Close();
            return UserId;
        }

        private void empIdTextBox_Leave(object sender, EventArgs e)
        {
            if (empIdTextBox.Text != "")
            {
                int empId = GetEmployeeId();
                
                string query5 = "select emp_name from b2_emp_basic where emp_id = '" + empId + "'";
                dBConn.Open();
                SqlCommand command5 = new SqlCommand(query5, dBConn);
                SqlDataReader reader = command5.ExecuteReader();
                if (reader.Read() == true)
                {
                    textBox1.Text = reader["emp_name"].ToString();
                    //textBox2.Text = reader["designation"].ToString();
                }
                else
                {
                    MessageBox.Show("Invalid cardno");
                    empIdTextBox.Focus();
                }
                dBConn.Close();
            }
            else
            {

            }
        }

        private void sectionComboBox_Leave(object sender, EventArgs e)
        {
            AdminStyleLoad();
        }
        private void styleComboBox_Leave(object sender, EventArgs e)
        {
            ProcessLoad();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int prodId = FloorProductionId;

            int emp_id = GetEmployeeId();
            int sec_id = GetSectionId();
            int style_id = GetStyleId();
            int procee_id = GetProcessId();
            int quantity = quantityTextBox.Text == "" ? 0 : int.Parse(quantityTextBox.Text);
            int processby = GetUserId();

            try
            {
                if (emp_id != 0 && sec_id != 0 && style_id != 0 && procee_id != 0 && quantity != 0)
                {
                    dBConn.Open();
                    string query = "exec [b2_floor_production_insert_update_sp] @prodId,@section,@empId,@prodDate,@style,@process,@quantity,@processBy,@time";
                    SqlCommand command = new SqlCommand(query, dBConn);
                    command.Parameters.AddWithValue("@prodId", prodId);
                    command.Parameters.AddWithValue("@section", sec_id);
                    command.Parameters.AddWithValue("@empId", emp_id);
                    command.Parameters.AddWithValue("@prodDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@style", style_id);
                    command.Parameters.AddWithValue("@process", procee_id);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@processBy", processby);
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
            empIdTextBox.Clear();
            sectionComboBox.Text = "";
            styleComboBox.Text = "";
            processComboBox.Text = "";
            quantityTextBox.Clear();
            textBox1.Clear();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "exec [b2_floor_production_select_sp] @prodId='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        FloorProductionId = int.Parse(reader["Id"].ToString());
                        dateTimePicker1.Text = reader["Date"].ToString();
                        sectionComboBox.Text = reader["Section"].ToString();
                        styleComboBox.Text = reader["Style"].ToString();
                        processComboBox.Text = reader["Process"].ToString();
                        quantityTextBox.Text = reader["Quantity"].ToString();
                        empIdTextBox.Text = reader["EmpId"].ToString();
                    }
                }
            }
            dBConn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "delete from b2_floor_production_list where prod_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    int result = command2.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Deleted");
                    }
                }
            }
            dBConn.Close();
            GridDataLoad();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            int emp_id = SearchEmpId();
            
            SqlCommand command = new SqlCommand("select top(10) fpl.prod_id as Id, fpl.prod_date as Date, sii.section as Section, e.emp_cardno, si.style as Style, pii.process_name as 'Size/Process', fpl.quantity as Quantity from [b2_floor_production_list] as fpl " +
                "inner join b2_emp_basic as e on e.emp_id=fpl.emp_id " +
                "inner join b2_style_info as si on si.style_id= fpl.style " +
                "inner join b2_process_info as pii on pii.process_id=fpl.process " +
                "inner join b2_section_info as sii on sii.section_id=fpl.section " +
                "where fpl.emp_id='"+emp_id+"' and fpl.prod_date between '"+fromDateSearch.Value.ToString("yyyy-MM-dd")+"' and '"+toDateSearch.Value.ToString("yyyy-MM-dd") +"' order by fpl.prod_date", dBConn);
            dBConn.Open();
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }
        private int SearchEmpId()
        {
            int empId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select emp_id from b2_emp_basic where emp_cardno='" + empIdSearch.Text.ToUpper() + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                empId = int.Parse(rd["emp_id"].ToString());
            }
            dBConn.Close();
            return empId;
        }
    }
}
