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
    public partial class BuyerStyleAddForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int StyleId = 0;
        public BuyerStyleAddForm()
        {
            InitializeComponent();
        }
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select si.style_id as Id, bi.buyer as Buyer,sec.section as Section,si.style as Style,si.status as isActive from b2_style_info as si inner join b2_section_info as sec on sec.section_id=si.section inner join b2_buyer_info as bi on bi.buyer_id=si.buyer", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }
        private void BuyerLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select buyer from b2_buyer_info order by buyer", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                buyerComboBox.Items.Add(rd["buyer"].ToString());
            }
            dBConn.Close();
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
        private int GetBuyerId()
        {
            int BuyerId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select buyer_id from b2_buyer_info where buyer='" + buyerComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                BuyerId = int.Parse(rd["buyer_id"].ToString());
            }
            dBConn.Close();
            return BuyerId;
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
        private void BuyerStyleAddForm_Load(object sender, EventArgs e)
        {
            BuyerLoad();
            SectionLoad();
            GridDataLoad();
        }
        // save database
        private void button1_Click(object sender, EventArgs e)
        {
            int s_id = StyleId;
            int b_id = GetBuyerId();
            int sec_id = GetSectionId();

            string style = styleTxtBox.Text.ToUpper();
            int isActive = 1;

            int StyleCheked = HasStyle();
            if(StyleCheked == 1)
            {
                MessageBox.Show("This style already saved by user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                try
                {
                    if (b_id != 0 && sec_id != 0 && style != "")
                    {
                        dBConn.Open();
                        string query = "exec [b2_buyer_style_insert_update_sp] @s_id,@buyer_id,@section_id,@style,@isActive,@time";
                        SqlCommand command = new SqlCommand(query, dBConn);
                        command.Parameters.AddWithValue("@s_id", s_id);
                        command.Parameters.AddWithValue("@buyer_id", b_id);
                        command.Parameters.AddWithValue("@section_id", sec_id);
                        command.Parameters.AddWithValue("@style", style);
                        command.Parameters.AddWithValue("@isActive", isActive);
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
                {  MessageBox.Show(ex.Message.ToString()); }
            }            
        }
        private int HasStyle()
        {
            int HasStyle=0;

            int b_id = GetBuyerId();
            int sec_id = GetSectionId();
            string style = styleTxtBox.Text.ToUpper();


            dBConn.Open();
            SqlCommand command = new SqlCommand("select style_id from b2_style_info  where buyer='"+ b_id + "' and section='"+ sec_id + "' and style='"+ style + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                int style_id = int.Parse(rd["style_id"].ToString());
                if(style_id > 0)
                {
                    HasStyle = 1;  // style already has
                } else
                {
                    HasStyle = 2;   // style not entry yet
                }
            }
            dBConn.Close();
            return HasStyle;
        }
        private void ClearData()
        {
            buyerComboBox.Text = "";
            sectionComboBox.Text = "";
            styleTxtBox.Clear();            
        }

        //select database
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "exec [b2_buyer_style_select_sp] '" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        StyleId = int.Parse(reader["style_id"].ToString());
                        buyerComboBox.Text = reader["buyer"].ToString();
                        sectionComboBox.Text = reader["section"].ToString();
                        styleTxtBox.Text = reader["style"].ToString();                                          
                    }
                }
            }
            dBConn.Close();
        }
    }
}
