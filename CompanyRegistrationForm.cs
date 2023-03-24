using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyTree
{
    public partial class CompanyRegistrationForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public int CompanyId = 0;
        public CompanyRegistrationForm()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select * from b2_company_info", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["execution_time"].Visible = false;
        }

        private void CompanyRegistrationForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string c_name = cNameEng.Text;
            string c_name_ban = cNameBangla.Text;
            string c_add = cAddressEng.Text;
            string c_add_ban = cAddressBangla.Text;

            // IMAGE SAVE TO DATABASE 
            byte[] imgByte;                        
            try
            {
                imgByte = GetImage();// get image as bytearray

                dBConn.Open();
                string query = "exec [b2_company_insert_update_sp] @CompanyId,@c_name,@c_name_bangla,@c_add,@c_add_bangla,@c_logo,@c_time";
                SqlCommand command = new SqlCommand(query, dBConn);
                command.Parameters.AddWithValue("@CompanyId", CompanyId == 0 ? 0 : CompanyId);
                command.Parameters.AddWithValue("@c_name", c_name);
                command.Parameters.AddWithValue("@c_name_bangla", c_name_ban);
                command.Parameters.AddWithValue("@c_add", c_add);
                command.Parameters.AddWithValue("@c_add_bangla", c_add_ban);
                command.Parameters.AddWithValue("@c_logo", imgByte);
                command.Parameters.AddWithValue("@c_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                dBConn.Close();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private byte[] GetImage()
        {

            //string sFile;
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            byte[] imgByte = null;
            //openFileDialog1.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
                //sFile = openFileDialog1.FileName;
                //logoBox.Image = System.Drawing.Bitmap.FromFile(sFile);
            //    logoBox.SizeMode = PictureBoxSizeMode.StretchImage;
            using (MemoryStream mStream = new MemoryStream())
            {
                logoBox.Image.Save(mStream, logoBox.Image.RawFormat);
                imgByte = mStream.ToArray();
            }
            //}
            return imgByte;
        }

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            string sFile;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            byte[] imgByte = null;
            openFileDialog1.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sFile = openFileDialog1.FileName;
                logoBox.Image = System.Drawing.Bitmap.FromFile(sFile);
                logoBox.SizeMode = PictureBoxSizeMode.StretchImage;
                using (MemoryStream mStream = new MemoryStream())
                {
                    logoBox.Image.Save(mStream, logoBox.Image.RawFormat);
                    imgByte = mStream.ToArray();
                }
            }
        }
        

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            byte[] imageBtye;
            dBConn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow delRow = dataGridView1.Rows[i];
                if (delRow.Selected == true)
                {
                    string query1 = "select * FROM b2_company_info where company_id = '" + dataGridView1.Rows[i].Cells[0].Value + "'";
                    SqlCommand command2 = new SqlCommand(query1, dBConn);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        CompanyId = int.Parse(reader["company_id"].ToString());
                        cNameEng.Text = reader["company_name"].ToString();
                        cNameBangla.Text = reader["company_name_bangla"].ToString();
                        cAddressEng.Text = reader["company_address"].ToString();
                        cAddressBangla.Text = reader["company_address_bangla"].ToString();

                        imageBtye = (byte[])reader["company_logo"];
                        MemoryStream ms = new MemoryStream(imageBtye);
                        logoBox.Image = new Bitmap(ms);
                        logoBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            dBConn.Close();
        }
    }
}
