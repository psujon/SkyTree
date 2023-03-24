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
    public partial class BuyerStyleRateAddForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int PieceRateId = 0;
        public BuyerStyleRateAddForm()
        {
            InitializeComponent();
        }
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select top(50) spr.rate_id as Id,sec.section as Section, sii.style as Style, pii.process_name as Process, spr.rate as 'Rate(Pcs)' from b2_style_piece_rate as spr " +
                                                    "inner join b2_process_info as pii on pii.process_id=spr.process " +
                                                    "inner join b2_style_info as sii on sii.style_id=spr.style inner " +
                                                    "join b2_section_info as sec on sec.section_id=spr.section", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
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
            int sec_id = GetSectionId();

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

            int sec_id = GetSectionId();
            int style_id = GetStyleId();
            string process = processComboBox.Text;

            dBConn.Open();
            SqlCommand command = new SqlCommand("select process_id from b2_process_info where section='" + sec_id + "' and style='" + style_id + "' and process_name='"+process+"'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                ProcessId = int.Parse(rd["process_id"].ToString());
            }
            dBConn.Close();
            return ProcessId;
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
            styleComboBox.Text = "";
            styleComboBox.Items.Clear();

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
            processComboBox.Text = "";
            processComboBox.Items.Clear();

            int sec_id = GetSectionId();
            int style_id = GetStyleId();           

            dBConn.Open();
            SqlCommand command = new SqlCommand("select process_name from b2_process_info where section='"+ sec_id + "' and style='" + style_id + "' group by process_name order by process_name", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                processComboBox.Items.Add(rd["process_name"].ToString());
            }
            dBConn.Close();
        }

        private void BuyerStyleRateAddForm_Load(object sender, EventArgs e)
        {
            GridDataLoad();
            SectionLoad();
        }

        private void sectionComboBox_Leave(object sender, EventArgs e)
        {
            StyleLoad();
        }

        private void styleComboBox_Leave(object sender, EventArgs e)
        {
            ProcessLoad();
        }
        // save information to database
        private void button1_Click(object sender, EventArgs e)
        {
            int P_RateID = PieceRateId;

            int sec_id = GetSectionId();
            int style_id = GetStyleId();
            int procee_id = GetProcessId();

            double piece_rate = rateTextBox.Text == "" ? 0 : Convert.ToDouble(rateTextBox.Text);

            int hasRate = HasPieceRate();
            if(hasRate == 1)
            {
                MessageBox.Show("This style & process rate already saved by user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                try
                {
                    if (sec_id != 0 && style_id != 0 && procee_id != 0 && piece_rate != 0)
                    {
                        dBConn.Open();
                        string query = "exec [b2_piece_rate_insert_update_sp] @rate_id, @section, @style, @process, @rate, @time";
                        SqlCommand command = new SqlCommand(query, dBConn);
                        command.Parameters.AddWithValue("@rate_id", P_RateID);
                        command.Parameters.AddWithValue("@section", sec_id);
                        command.Parameters.AddWithValue("@style", style_id);
                        command.Parameters.AddWithValue("@process", procee_id);
                        command.Parameters.AddWithValue("@rate", piece_rate);
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


        }
        private void ClearData()
        {            
            sectionComboBox.Text = "";
            styleComboBox.Text = "";
            processComboBox.Text = "";
            rateTextBox.Clear();
        }

        private int HasPieceRate()
        {
            int HasPieceRate = 0;

            int sec_id = GetSectionId();
            int style_id = GetStyleId();
            int process_id = GetProcessId();
            //double pieceRate = rateTextBox.Text == "" ? 0 : Convert.ToDouble(rateTextBox.Text);

            dBConn.Open();
            SqlCommand command = new SqlCommand("select rate_id from b2_style_piece_rate where " +
                "section='"+ sec_id + "' and style='"+ style_id + "' and process='"+ process_id + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                int rateId = int.Parse(rd["rate_id"].ToString());
                if (rateId > 0)
                {
                    HasPieceRate = 1;  // style already has
                }
                else
                {
                    HasPieceRate = 2;   // style not entry yet
                }
            }
            dBConn.Close();
            return HasPieceRate;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "exec b2_piece_rate_select_sp @rate_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        PieceRateId = int.Parse(reader["Id"].ToString());
                        sectionComboBox.Text = reader["Section"].ToString();                        
                        styleComboBox.Text = reader["Style"].ToString();
                        processComboBox.Text = reader["Process"].ToString();
                        rateTextBox.Text = reader["Rate"].ToString();
                    }
                }
            }
            dBConn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select top(50) spr.rate_id as Id,sec.section as Section, sii.style as Style, pii.process_name as Process, spr.rate as 'Rate(Pcs)' from b2_style_piece_rate as spr  " +
                                                "inner join b2_process_info as pii on pii.process_id=spr.process inner " +
                                                "join b2_style_info as sii on sii.style_id=spr.style " +
                                                "inner join b2_section_info as sec on sec.section_id=spr.section " +
                                                "where sii.style like '%"+searchTextBox.Text+"%'", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "delete from b2_style_piece_rate where rate_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    int result = command2.ExecuteNonQuery();
                    if(result > 0)
                    {
                        MessageBox.Show("Data delete successfully.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    GridDataLoad();
                }
            }
            dBConn.Close();
        }
    }
}
