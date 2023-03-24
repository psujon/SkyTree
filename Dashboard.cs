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
    public partial class Dashboard : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);

        int global_section = LoginForm.global_section;
        int global_userId = LoginForm.global_userId;
        string global_user = LoginForm.global_user;
        int global_userRole = LoginForm.global_userRole;
        int global_unit = LoginForm.global_unit;
        public Dashboard()
        {
            InitializeComponent();
            LoadUserPermission();
            label1.Text = "Company: '"+GetCompanyName()+ "'    |    Unit/Building: '" + GetBuildingName() + "'   |   User: " + global_user + "  |  UserType: '" + GetUserRoleType() + "'  |   Section: '"+GetSectionName()+"'";
        }

        private string GetCompanyName()
        {
            string company = null;
            dBConn.Open();
            string query = "select company_name from b2_company_info";
            SqlCommand cmd = new SqlCommand(query, dBConn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                company = rd["company_name"].ToString();
            }
            dBConn.Close();
            return company;
        }
        private string GetBuildingName()
        {
            string building = null;
            dBConn.Open();
            string query = "select building_name from b2_building_info where building_id='"+ global_unit + "'";
            SqlCommand cmd = new SqlCommand(query, dBConn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                building = rd["building_name"].ToString();
            }
            dBConn.Close();
            return building;
        }
        private string GetUserRoleType()
        {
            string roleType = null;
            dBConn.Open();
            string query = "select role_name from b2_user_role where role_id='" + global_userRole + "'";
            SqlCommand cmd = new SqlCommand(query, dBConn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                roleType = rd["role_name"].ToString();
            }
            dBConn.Close();
            return roleType;
        }
        private string GetSectionName()
        {
            string section = null;
            dBConn.Open();
            string query = "select section from b2_section_info where section_id='" + global_section + "'";
            SqlCommand cmd = new SqlCommand(query, dBConn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                section = rd["section"].ToString();
            }
            dBConn.Close();
            return section;
        }

        private Form currentChildForm;
        private void OpenChildForm(Form childForm)
        {
            if(currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            // this.Text = childForm.Text;
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void companyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CompanyRegistrationForm crf = new CompanyRegistrationForm();
            //crf.MdiParent = this.ActiveMdiChild;
            //crf.Show();
            OpenChildForm(new CompanyRegistrationForm());
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new UserRegisterForm());
        }

        private void unitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuildingAddForm());
        }

        private void buyerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuyerAddForm());
        }

        private void countryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CountryAddForm());
        }

        private void styleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuyerStyleAddForm());
        }

        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuyerStyleProcessAddForm());
        }

        private void pieceRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuyerStyleRateAddForm());
        }

        private void productionEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FloorProductionEntryForm());
        }

        private void userRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new UsersPermissionForm());
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new EmployeeInformationForm());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ReportAllForm());
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            
        }
        private void LoadUserPermission()
        {
            int p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32, p33, p34, p35, p36, p37, p38, p39, p40, p41, p42, p43, p44, p45, p46, p47, p48, p49, p50;
            dBConn.Open();
            string query = "select * from b2_user_permission where userId='" + global_userId + "'";
            SqlCommand cmd = new SqlCommand(query, dBConn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                p1 = int.Parse(rd["p1"].ToString());
                p2 = int.Parse(rd["p2"].ToString());
                p3 = int.Parse(rd["p3"].ToString());
                p4 = int.Parse(rd["p4"].ToString());
                p5 = int.Parse(rd["p5"].ToString());
                p6 = int.Parse(rd["p6"].ToString());
                p7 = int.Parse(rd["p7"].ToString());
                p8 = int.Parse(rd["p8"].ToString());
                p9 = int.Parse(rd["p9"].ToString());
                p10 = int.Parse(rd["p10"].ToString());
                p11 = int.Parse(rd["p11"].ToString());
                p12 = int.Parse(rd["p12"].ToString());
                p13 = int.Parse(rd["p13"].ToString());
                p14 = int.Parse(rd["p14"].ToString());
                p15 = int.Parse(rd["p15"].ToString());
                p16 = int.Parse(rd["p16"].ToString());
                p17 = int.Parse(rd["p17"].ToString());
                p18 = int.Parse(rd["p18"].ToString());
                p19 = int.Parse(rd["p19"].ToString());
                p20 = int.Parse(rd["p20"].ToString());
                p21 = int.Parse(rd["p21"].ToString());
                p22 = int.Parse(rd["p22"].ToString());
                p23 = int.Parse(rd["p23"].ToString());
                p24 = int.Parse(rd["p24"].ToString());
                p25 = int.Parse(rd["p25"].ToString());
                p26 = int.Parse(rd["p26"].ToString());
                p27 = int.Parse(rd["p27"].ToString());
                p28 = int.Parse(rd["p28"].ToString());
                p29 = int.Parse(rd["p29"].ToString());
                p30 = int.Parse(rd["p30"].ToString());
                p31 = int.Parse(rd["p31"].ToString());
                p32 = int.Parse(rd["p32"].ToString());
                p33 = int.Parse(rd["p33"].ToString());
                p34 = int.Parse(rd["p34"].ToString());
                p35 = int.Parse(rd["p35"].ToString());
                p36 = int.Parse(rd["p36"].ToString());
                p37 = int.Parse(rd["p37"].ToString());
                p38 = int.Parse(rd["p38"].ToString());
                p39 = int.Parse(rd["p39"].ToString());
                p40 = int.Parse(rd["p40"].ToString());
                p41 = int.Parse(rd["p41"].ToString());
                p42 = int.Parse(rd["p42"].ToString());
                p43 = int.Parse(rd["p43"].ToString());
                p44 = int.Parse(rd["p44"].ToString());
                p45 = int.Parse(rd["p45"].ToString());
                p46 = int.Parse(rd["p46"].ToString());
                p47 = int.Parse(rd["p47"].ToString());
                p48 = int.Parse(rd["p48"].ToString());
                p49 = int.Parse(rd["p49"].ToString());
                p50 = int.Parse(rd["p50"].ToString());

                if (p1 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                if (p2 == 1) { unitToolStripMenuItem.Visible = true; } else { unitToolStripMenuItem.Visible = false; }
                if (p3 == 1) { usersToolStripMenuItem.Visible = true; } else { usersToolStripMenuItem.Visible = false; }
                if (p4 == 1) { userRoleToolStripMenuItem.Visible = true; } else { userRoleToolStripMenuItem.Visible = false; }
                if (p5 == 1) { departmentToolStripMenuItem.Visible = true; } else { departmentToolStripMenuItem.Visible = false; }
                if (p6 == 1) { sectionToolStripMenuItem.Visible = true; } else { sectionToolStripMenuItem.Visible = false; }
                if (p7 == 1) { blockToolStripMenuItem.Visible = true; } else { blockToolStripMenuItem.Visible = false; }
                if (p8 == 1) { countryToolStripMenuItem.Visible = true; } else { countryToolStripMenuItem.Visible = false; }
                if (p9 == 1) { designationToolStripMenuItem.Visible = true; } else { designationToolStripMenuItem.Visible = false; }
                if (p10 == 1) { shiftToolStripMenuItem.Visible = true; } else { shiftToolStripMenuItem.Visible = false; }
                if (p11 == 1) { buyerToolStripMenuItem.Visible = true; } else { buyerToolStripMenuItem.Visible = false; }
                if (p12 == 1) { styleToolStripMenuItem.Visible = true; } else { styleToolStripMenuItem.Visible = false; }
                if (p13 == 1) { processToolStripMenuItem.Visible = true; } else { processToolStripMenuItem.Visible = false; }
                if (p14 == 1) { colorSizeToolStripMenuItem.Visible = true; } else { colorSizeToolStripMenuItem.Visible = false; }
                if (p15 == 1) { pieceRateToolStripMenuItem.Visible = true; } else { pieceRateToolStripMenuItem.Visible = false; }
                if (p16 == 1) { styleCloseToolStripMenuItem.Visible = true; } else { styleCloseToolStripMenuItem.Visible = false; }
                //if (p17 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p18 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p19 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p20 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                if (p21 == 1) { employeeToolStripMenuItem.Visible = true; } else { employeeToolStripMenuItem.Visible = false; }
                if (p22 == 1) { resignToolStripMenuItem.Visible = true; } else { resignToolStripMenuItem.Visible = false; }
                //if (p23 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p24 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p25 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p26 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p27 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p28 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p29 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p30 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p31 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p32 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p33 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p34 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p35 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p36 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p37 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p38 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p39 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p40 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                if (p41 == 1) { productionEntryToolStripMenuItem.Visible = true; } else { productionEntryToolStripMenuItem.Visible = false; }
                if (p42 == 1) { goodsReceivedEntryToolStripMenuItem.Visible = true; } else { goodsReceivedEntryToolStripMenuItem.Visible = false; }
                //if (p43 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p44 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p45 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p46 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p47 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p48 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p49 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
                //if (p50 == 1) { companyToolStripMenuItem.Visible = true; } else { companyToolStripMenuItem.Visible = false; }
            }
            dBConn.Close();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuyerStyleColorEntryForm());
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BuyerStyleColorSizeEntryForm());            
        }
    }
}
