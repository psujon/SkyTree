using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
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
    public partial class ReportAllForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public ReportAllForm()
        {
            InitializeComponent();
        }
        private void ReportAllForm_Load(object sender, EventArgs e)
        {
            UnitLoad();
            DepartmentLoad();
            BlockLoad();
            GridDataLoad();
        }
        private int GetUnit()
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
        private void UnitLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select building_name from b2_building_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            { unitComboBox.Items.Add(rd["building_name"].ToString()); }
            dBConn.Close();
        }
        private void DepartmentLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select department from b2_department_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            { deptComboBox.Items.Add(rd["department"].ToString()); }
            dBConn.Close();
        }
        private int GetDepartment()
        {
            int deptId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select deptId from b2_department_info where department='" + deptComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                deptId = int.Parse(rd["deptId"].ToString());
            }
            dBConn.Close();
            return deptId;
        }
        private void deptComboBox_Leave(object sender, EventArgs e)
        {
            SectionLoad();
        }
        private void SectionLoad()
        {
            sectionComboBox.Items.Clear();
            dBConn.Open();
            SqlCommand command = new SqlCommand("select section from b2_section_info", dBConn);
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
        private void BlockLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select block from b2_block_info", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            { blockComboBox.Items.Add(rd["block"].ToString()); }
            dBConn.Close();
        }
        private int GetBlock()
        {
            int block = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select blockId from b2_block_info where block='" + blockComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                block = int.Parse(rd["blockId"].ToString());
            }
            dBConn.Close();
            return block;
        }
        private int GetEmpId()
        {
            int emp_id = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select emp_id from b2_emp_basic where emp_cardno='" + cardnoTxt.Text.ToUpper() + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                emp_id = int.Parse(rd["emp_id"].ToString());
            }
            dBConn.Close();
            return emp_id;
        }
        private void GridDataLoad()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select * from b2_emp_basic where emp_cardno like '%" + cardnoTxt.Text.ToUpper() + "%' or emp_cardno is null", dBConn);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dBConn.Close();
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e) // worker monthly jobcard
        {
            int unit = unitComboBox.Text == "" ? 0 : GetUnit();
            int department = deptComboBox.Text == "" ? 0 : GetDepartment();
            int section = sectionComboBox.Text == "" ? 0 : GetSection();
            int block = blockComboBox.Text == "" ? 0 : GetBlock();
            int empId = cardnoTxt.Text == "" ? 0 : GetEmpId();

            ReportViewerForm rvf = new ReportViewerForm();
            ReportDocument myDataReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            string workingDirectory = Application.StartupPath;
            string rptPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\reports\MonthlyWorkerProductionJobcardReport.rpt";
            myDataReport.Load(rptPath);
            rvf.crystalReportViewer1.ReportSource = myDataReport;
            myDataReport.SetParameterValue("@unit", unit);
            myDataReport.SetParameterValue("@department", department);
            myDataReport.SetParameterValue("@section", section);
            myDataReport.SetParameterValue("@block", block);
            myDataReport.SetParameterValue("@empId", empId);
            myDataReport.SetParameterValue("@fromdate", fromDatePicker.Value.ToString("yyyy-MM-dd"));
            myDataReport.SetParameterValue("@todate", toDatePicker.Value.ToString("yyyy-MM-dd"));
            myDataReport.SetDatabaseLogon("sa", "bwsl2022");
            rvf.Show();
        }

        private void cardnoTxt_TextChanged(object sender, EventArgs e)
        {
            GridDataLoad();
        }

        private void button1_Click(object sender, EventArgs e) // cardno search button
        {
            //int buildingId = GetUnit();
            //int department = GetDepartment();
            //int section = GetSection();
            //int block = GetBlock();
            //int emp_id = GetEmpId();

            //string query = "select * from b2_emp_basic where unit = '" + buildingId + "' or unit is null " +
            //    "and department = '" + department + "' or department is null " +
            //    "and section = '" + section + "' or section is null " +
            //    "and block = '" + block + "' or block is null ";

            //dBConn.Open();
            //SqlCommand command = new SqlCommand(query, dBConn);
            //SqlDataAdapter sd = new SqlDataAdapter(command);
            //DataTable dt = new DataTable();
            //sd.Fill(dt);
            //dBConn.Close();
            //dataGridView1.DataSource = dt;
        }

        // production piece rate sheet
        private void button8_Click(object sender, EventArgs e) 
        {
            ReportViewerForm rvf = new ReportViewerForm();
            ReportDocument myDataReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            string workingDirectory = Application.StartupPath;

            int section = sectionComboBox.Text == "" ? 0 : GetSection();

            if(pieceRateCheckBox.Checked == true)
            {
                
                string rptPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\reports\ProductionPieceRateSheetAll.rpt";
                myDataReport.Load(rptPath);
                rvf.crystalReportViewer1.ReportSource = myDataReport;
                myDataReport.SetParameterValue("@section", section);
                myDataReport.SetParameterValue("@fromdate", fromDatePicker.Value.ToString("yyyy-MM-dd"));
                myDataReport.SetParameterValue("@todate", toDatePicker.Value.ToString("yyyy-MM-dd"));
                myDataReport.SetDatabaseLogon("sa", "bwsl2022");
                rvf.ShowDialog();
                rvf.WindowState = FormWindowState.Maximized;
            }
            else
            {                
                string rptPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\reports\ProductionPieaceRateSheetBlankReport.rpt";
                myDataReport.Load(rptPath);
                rvf.crystalReportViewer1.ReportSource = myDataReport;
                myDataReport.SetParameterValue("@section", section);
                myDataReport.SetParameterValue("@fromdate", fromDatePicker.Value.ToString("yyyy-MM-dd"));
                myDataReport.SetParameterValue("@todate", toDatePicker.Value.ToString("yyyy-MM-dd"));
                myDataReport.SetDatabaseLogon("sa", "bwsl2022");
                rvf.ShowDialog();
                rvf.WindowState = FormWindowState.Maximized;
            }
        }


        // worker monthly production sheet
        private void button5_Click(object sender, EventArgs e) 
        {
            int unit = unitComboBox.Text == "" ? 0 : GetUnit();
            int department = deptComboBox.Text == "" ? 0 : GetDepartment();
            int section = sectionComboBox.Text == "" ? 0 : GetSection();
            int block = blockComboBox.Text == "" ? 0 : GetBlock();
            int empId = cardnoTxt.Text == "" ? 0 : GetEmpId();

            ReportViewerForm rvf = new ReportViewerForm();
            ReportDocument myDataReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            string workingDirectory = Application.StartupPath;
            string rptPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\reports\MonthlyWorkerProductionReport.rpt";
            myDataReport.Load(rptPath);            
            rvf.crystalReportViewer1.ReportSource = myDataReport;
            myDataReport.SetParameterValue("@unit", unit);
            myDataReport.SetParameterValue("@department", department);
            myDataReport.SetParameterValue("@section", section);
            myDataReport.SetParameterValue("@block", block);
            myDataReport.SetParameterValue("@empId", empId);
            myDataReport.SetParameterValue("@fromdate", fromDatePicker.Value.ToString("yyyy-MM-dd"));
            myDataReport.SetParameterValue("@todate", toDatePicker.Value.ToString("yyyy-MM-dd"));
            myDataReport.SetDatabaseLogon("sa", "bwsl2022");
            rvf.ShowDialog();
            rvf.WindowState = FormWindowState.Maximized;
            //crystalReportViewer1.Refresh();
        }
    }
}
