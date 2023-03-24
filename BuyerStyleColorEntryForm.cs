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
    public partial class BuyerStyleColorEntryForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);

        int global_section = 5; // LoginForm.global_section;
        string global_user = LoginForm.global_user;
        int global_userRole = LoginForm.global_userRole;
        int global_unit = LoginForm.global_unit;

        private int StyleColorId = 0;
        public BuyerStyleColorEntryForm()
        {
            InitializeComponent();
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

        private void styleComboBox2_Leave(object sender, EventArgs e)
        {
            ProcessLoad();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            int style_color_id = StyleColorId;

            int section = global_section;            
            int style = GetStyleId();
            int process = GetProcessId() == 0 ? 0 : GetProcessId();            
            string color = colorTextBox.Text.ToUpper();

            if (style == 0) { MessageBox.Show("Select style"); }
            else if (color == "")
            {
                MessageBox.Show("Input Color Code");
            }
            else
            {
                string query = "exec b2_style_color_save_sp @color_id,@section,@style,@process,@color";
                SqlCommand cmdd = new SqlCommand(query, dBConn);
                cmdd.Parameters.AddWithValue("@color_id", style_color_id);
                cmdd.Parameters.AddWithValue("@section", section);
                cmdd.Parameters.AddWithValue("@style", style);
                cmdd.Parameters.AddWithValue("@process", process);
                cmdd.Parameters.AddWithValue("@color", color);
                dBConn.Open();
                cmdd.ExecuteNonQuery();
                dBConn.Close();
                GridDataBind();
                colorTextBox.Text = "";
            }
        }

        private void GridDataBind()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("SELECT TOP(100) b2_style_color.color_id, b2_section_info.section, b2_style_info.style, b2_process_info.process_name, b2_style_color.color, b2_style_color.c_time FROM b2_style_color INNER JOIN b2_section_info ON b2_style_color.section = b2_section_info.section_id INNER JOIN b2_style_info ON b2_style_color.style = b2_style_info.style_id AND b2_section_info.section_id = b2_style_info.section INNER JOIN b2_process_info ON b2_style_color.process = b2_process_info.process_id AND b2_section_info.section_id = b2_process_info.section AND b2_style_info.style_id = b2_process_info.style ORDER BY b2_style_color.color_id DESC", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }

        private void BuyerStyleColorEntryForm_Load(object sender, EventArgs e)
        {
            StyleLoad();
            GridDataBind();
        }
    }
}
