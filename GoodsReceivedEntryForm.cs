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
    public partial class GoodsReceivedEntryForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);

        int global_section = LoginForm.global_section;
        string global_user = LoginForm.global_user;
        int global_userRole = LoginForm.global_userRole;
        int global_unit = LoginForm.global_unit;

        private int GoodsReceivedId=0;
        public GoodsReceivedEntryForm()
        {
            InitializeComponent();
        }
        //private int GetUnit()
        //{
            //int UnitId = 0;
            //dBConn.Open();
            //SqlCommand command = new SqlCommand("select building_id from b2_building_info where building_name='" + unitComboBox.Text + "'", dBConn);
            //SqlDataReader rd = command.ExecuteReader();
            //while (rd.Read())
            //{
            //    UnitId = int.Parse(rd["building_id"].ToString());
            //}
            //dBConn.Close();
            //return UnitId;
        //}
        private void SectionLoad()
        {
            sectionComboBox.Items.Clear();
            dBConn.Open();
            SqlCommand command = new SqlCommand("select section from b2_section_info order by section", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            { sectionComboBox.Items.Add(rd["section"].ToString()); }
            dBConn.Close();
        }
        private int GetSection()
        {
            int section_id = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select section_id from b2_section_info where section='" + sectionComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                section_id = int.Parse(rd["section_id"].ToString());
            }
            dBConn.Close();
            return section_id;
        }
        private void StyleLoad()
        {
            int sec_id = global_section;
            styleComboBox2.Text = "";
            styleComboBox2.Items.Clear();

            dBConn.Open();
            SqlCommand command = new SqlCommand("select style from b2_style_info where section='" + sec_id + "' group by style order by style", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                styleComboBox2.Items.Add(rd["style"].ToString());
            }
            dBConn.Close();
        }
        private void ProcessLoad()
        {
            processComboBox3.Text = "";
            processComboBox3.Items.Clear();

            int sec_id = global_section;
            int style_id = GetStyleId();

            dBConn.Open();
            SqlCommand command = new SqlCommand("select process_name from b2_process_info where section='" + sec_id + "' and style='" + style_id + "' group by process_name order by process_name", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                processComboBox3.Items.Add(rd["process_name"].ToString());
            }
            dBConn.Close();
        }
        private int GetStyleId()
        {
            int StyleId = 0;
            int sec_id = global_section;

            dBConn.Open();
            SqlCommand command = new SqlCommand("select style_id from b2_style_info where section='" + sec_id + "' and style='" + styleComboBox2.Text + "'", dBConn);
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

            int sec_id = global_section;
            int style_id = GetStyleId();
            string process = processComboBox3.Text;

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
        private void LoadColor()
        {
            colorComboBox4.Items.Clear();
            int section = global_section;
            int style = GetStyleId();
            //int process = GetProcessId();
            dBConn.Open();
            SqlCommand command = new SqlCommand("select color from b2_style_color where style='"+style+"' and section='"+section+"' order by color", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            { colorComboBox4.Items.Add(rd["color"].ToString()); }
            dBConn.Close();
        }
        private int GetColorId()
        {
            int colorId = 0;
            int section = global_section;
            int style = GetStyleId();
            string color = colorComboBox4.Text;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select color_id from b2_style_color where section='" + section + "' and style='" + style + "' and color='"+color+"'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                colorId = int.Parse(rd["color_id"].ToString());
            } 
            dBConn.Close();
            return colorId;
        }
        private void LoadColorSize()
        {
            sizeComboBox5.Items.Clear();
            int color = GetColorId();
            //int style = GetStyleId();
            //int process = GetProcessId();
            dBConn.Open();
            SqlCommand command = new SqlCommand("select size from b2_style_color_size where color='"+color+"' order by size", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            { sizeComboBox5.Items.Add(rd["size"].ToString()); }
            dBConn.Close();
        }
        private int GetColorSizeId()
        {
            int size_id = 0;
            
            int color = GetColorId();
            string size = sizeComboBox5.Text;

            dBConn.Open();
            SqlCommand command = new SqlCommand("select size_id from b2_style_color_size where color='" + color + "' and size='" + size + "' ", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                size_id = int.Parse(rd["color_id"].ToString());
            }
            dBConn.Close();
            return size_id;
        }

        private void GoodsReceivedEntryForm_Load(object sender, EventArgs e)
        {
            SectionLoad();
            StyleLoad();
            GridDataBind();
        }

        private void styleComboBox2_Leave(object sender, EventArgs e)
        {
            ProcessLoad();
            LoadColor();
        }

        private void colorComboBox4_Leave(object sender, EventArgs e)
        {
            LoadColorSize();
        }

        private void saveBtn_Click(object sender, EventArgs e) // save/update button
        {
            int rcvId = GoodsReceivedId;
            int RcvBySection = global_section;
            int RcvFromSection = GetSection();
            int style = GetStyleId();
            int process = GetProcessId() == 0 ? 0 : GetProcessId();
            int color = GetColorId() == 0 ? 0 : GetColorId();
            int size = GetColorSizeId() == 0 ? 0 : GetColorSizeId();
            int qty = quantityTextBox.Text == "" ? 0 : int.Parse(quantityTextBox.Text);
            string comment = commentsTextBox.Text;

            if (RcvFromSection == 0)
            {
                MessageBox.Show("Select Section");
            }
            else if (style == 0)
            {
                MessageBox.Show("Select style");
            }
            else
            {
                string query = "exec b2_goods_received_sp @rcvId,@rcvDate,@rcvBySection,@rcvFromSection,@style,@process,@color,@size,@rcvQty,@rcvCmnt";
                SqlCommand cmdd = new SqlCommand(query, dBConn);
                cmdd.Parameters.AddWithValue("@rcvId", rcvId);
                cmdd.Parameters.AddWithValue("@rcvDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                cmdd.Parameters.AddWithValue("@rcvBySection", RcvBySection);
                cmdd.Parameters.AddWithValue("@rcvFromSection", RcvFromSection);
                cmdd.Parameters.AddWithValue("@style", style);
                cmdd.Parameters.AddWithValue("@process", process);
                cmdd.Parameters.AddWithValue("@color", color);
                cmdd.Parameters.AddWithValue("@size", size);
                cmdd.Parameters.AddWithValue("@rcvQty", qty);
                cmdd.Parameters.AddWithValue("@rcvCmnt", comment);
                dBConn.Open();
                cmdd.ExecuteNonQuery();
                dBConn.Close();
                GridDataBind();
                quantityTextBox.Text = "";
                commentsTextBox.Text = "";
            }
        }

        private void GridDataBind()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("SELECT TOP (10) b2_goods_received.rcv_id, b2_section_info.section, b2_goods_received.rcv_date, b2_buyer_info.buyer, b2_style_info.style, b2_process_info.process_name, b2_style_color.color, b2_style_color_size.size,  b2_goods_received.rcv_qty, b2_goods_received.rcv_cmnt, b2_goods_received.c_time FROM b2_goods_received INNER JOIN b2_section_info ON b2_goods_received.rcv_by_section = b2_section_info.section_id AND b2_goods_received.rcv_from_section = b2_section_info.section_id INNER JOIN  b2_style_info ON b2_goods_received.style = b2_style_info.style_id AND b2_section_info.section_id = b2_style_info.section INNER JOIN  b2_process_info ON b2_goods_received.process = b2_process_info.process_id AND b2_section_info.section_id = b2_process_info.section AND b2_style_info.style_id = b2_process_info.style INNER JOIN  b2_style_color ON b2_goods_received.color = b2_style_color.color_id AND b2_section_info.section_id = b2_style_color.section AND b2_style_info.style_id = b2_style_color.style AND b2_process_info.process_id = b2_style_color.process INNER JOIN b2_style_color_size ON b2_goods_received.size = b2_style_color_size.size_id INNER JOIN  b2_buyer_info ON b2_style_info.buyer = b2_buyer_info.buyer_id ORDER BY  b2_goods_received.rcv_id DESC", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "EXEC b2_goods_received_select_sp @rcvId='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        GoodsReceivedId = int.Parse(reader["rcv_id"].ToString());
                        sectionComboBox.Text = reader["Section"].ToString();
                        styleComboBox2.Text = reader["style"].ToString();
                        processComboBox3.Text = reader["process_name"].ToString();
                        colorComboBox4.Text = reader["color"].ToString();
                        sizeComboBox5.Text = reader["size"].ToString();
                        quantityTextBox.Text = reader["rcv_qty"].ToString();
                        commentsTextBox.Text = reader["rcv_cmnt"].ToString();
                    }
                }
            }
            dBConn.Close();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "delete from b2_goods_received where rcv_id='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    int result = command2.ExecuteNonQuery();
                    if(result > 0) { MessageBox.Show("Data deleted."); }
                }
            }
            dBConn.Close();
        }
    }
}
