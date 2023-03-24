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
    public partial class BuyerStyleProcessAddForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int ProcessId = 0;
        public BuyerStyleProcessAddForm()
        {
            InitializeComponent();
        }
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select top(50) pii.process_id as Id,sec.section as Section,sii.style as Style,pii.process_name as Process, pii.last_process as Last_Process from b2_process_info as pii inner join b2_style_info as sii on sii.style_id=pii.style inner join b2_section_info as sec on sec.section_id=pii.section", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }

        private void BuyerStyleProcessAddForm_Load(object sender, EventArgs e)
        {
            GridDataLoad();
            SectionLoad();
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
            dBConn.Open();
            SqlCommand command = new SqlCommand("select style_id from b2_style_info where style='" + styleComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                StyleId = int.Parse(rd["style_id"].ToString());
            }
            dBConn.Close();
            return StyleId;
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
        private void StyleLoad()
        {
            int sec_id = GetSectionId();

            styleComboBox.Items.Clear();

            dBConn.Open();
            SqlCommand command = new SqlCommand("select style from b2_style_info where section='"+sec_id+"'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                styleComboBox.Items.Add(rd["style"].ToString());
            }
            dBConn.Close();
        }

        // section combobox leave code
        private void sectionComboBox_Leave(object sender, EventArgs e)
        {
            StyleLoad();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int P_id = ProcessId;
            int sec_id = GetSectionId();
            int style_id = GetStyleId();

            string process = processTxtBox.Text.ToUpper();
            int lastProcess;

            if(lastProcessComboBox.Text == "Yes")
            {
                lastProcess = 1;    // 1 is last process
            } else
            {
                lastProcess = 0;   // 0 is not last process
            }

            int ProcessCheked = HasProcess();
            if(ProcessCheked == 1)
            {
                MessageBox.Show("This style & process already saved by user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                try
                {
                    if (sec_id != 0 && style_id != 0 && process != "")
                    {
                        dBConn.Open();
                        string query = "exec [b2_process_insert_update_sp] @P_id, @sec, @style, @process, @lastProcess";
                        SqlCommand command = new SqlCommand(query, dBConn);
                        command.Parameters.AddWithValue("@P_id", P_id);
                        command.Parameters.AddWithValue("@sec", sec_id);
                        command.Parameters.AddWithValue("@style", style_id);
                        command.Parameters.AddWithValue("@process", process);
                        command.Parameters.AddWithValue("@lastProcess", lastProcess);
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

            
        }
        private int HasProcess()
        {
            int HasProcess = 0;

            int sec_id = GetSectionId();
            int style_id = GetStyleId();
            string process = processTxtBox.Text.ToUpper();


            dBConn.Open();
            SqlCommand command = new SqlCommand("select process_id from b2_process_info where " +
                "section='" + sec_id + "' and style='" + style_id + "' and process_name='" + process + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                int process_id = int.Parse(rd["process_id"].ToString());
                if (process_id > 0)
                {
                    HasProcess = 1;  // style already has
                }
                else
                {
                    HasProcess = 2;   // style not entry yet
                }
            }
            dBConn.Close();
            return HasProcess;
        }
        private void ClearData()
        {
            sectionComboBox.Text = "";
            styleComboBox.Text = "";
            processTxtBox.Clear();
            lastProcessComboBox.Text = "";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "exec b2_process_select_sp @process_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        ProcessId = int.Parse(reader["Id"].ToString());
                        sectionComboBox.Text = reader["Section"].ToString();
                        styleComboBox.Text = reader["Style"].ToString();
                        processTxtBox.Text = reader["Process"].ToString();
                        string LP = reader["LastProcess"].ToString();

                        if (LP == "1")
                        {
                            lastProcessComboBox.Text = "Yes";
                        } else
                        {
                            lastProcessComboBox.Text = "No";
                        }
                    }
                }
            }
            dBConn.Close();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }
}
