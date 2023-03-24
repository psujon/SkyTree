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
    public partial class UsersPermissionForm : Form
    {
        static string cs = ConfigurationManager.ConnectionStrings["dBConn"].ConnectionString;
        SqlConnection dBConn = new SqlConnection(cs);
        public UsersPermissionForm()
        {
            InitializeComponent();
        }
        private void LoadUser()
        {
            dBConn.Open();
            SqlCommand command = new SqlCommand("select username from b2_users", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                userListComboBox.Items.Add(rd["username"].ToString());
            }
            dBConn.Close();
        }
        private int GetUserId()
        {
            int UserId = 0;
            dBConn.Open();
            SqlCommand command = new SqlCommand("select user_id from b2_users where username ='" + userListComboBox.Text + "'", dBConn);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                UserId = int.Parse(rd["user_id"].ToString());
            }
            dBConn.Close();
            return UserId;
        }

        private void UsersPermissionForm_Load(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void permissionSaveBtn_Click(object sender, EventArgs e)
        {
            int userId = GetUserId();

            int ck1, ck2, ck3, ck4, ck5, ck6, ck7, ck8, ck9, ck10, ck11, ck12, ck13, ck14, ck15, ck16, ck17, ck18, ck19, ck20, ck21, ck22, ck23, ck24, ck25, ck26, ck27, ck28, ck29, ck30, ck31, ck32, ck33, ck34, ck35, ck36, ck37, ck38, ck39, ck40, ck41, ck42, ck43, ck44, ck45, ck46, ck47, ck48, ck49, ck50;

            // 1 for check // 0 for uncheck
            if (c1.Checked == true) { ck1 = 1; } else { ck1 = 0; }            if (c2.Checked == true) { ck2 = 1; } else { ck2 = 0; }            if (c3.Checked == true) { ck3 = 1; } else { ck3 = 0; }
            if (c4.Checked == true) { ck4 = 1; } else { ck4 = 0; }            if (c5.Checked == true) { ck5 = 1; } else { ck5 = 0; }            if (c6.Checked == true) { ck6 = 1; } else { ck6 = 0; }
            if (c7.Checked == true) { ck7 = 1; } else { ck7 = 0; }            if (c8.Checked == true) { ck8 = 1; } else { ck8 = 0; }            if (c9.Checked == true) { ck9 = 1; } else { ck9 = 0; }
            if (c10.Checked == true) { ck10 = 1; } else { ck10 = 0; }            if (c11.Checked == true) { ck11 = 1; } else { ck11 = 0; }            if (c12.Checked == true) { ck12 = 1; } else { ck12 = 0; }
            if (c13.Checked == true) { ck13 = 1; } else { ck13= 0; }            if (c14.Checked == true) { ck14 = 1; } else { ck14 = 0; }            if (c15.Checked == true) { ck15 = 1; } else { ck15 = 0; }
            if (c16.Checked == true) { ck16 = 1; } else { ck16 = 0; }            if (c17.Checked == true) { ck17 = 1; } else { ck17 = 0; }            if (c18.Checked == true) { ck18 = 1; } else { ck18 = 0; }
            if (c19.Checked == true) { ck19 = 1; } else { ck19 = 0; }            if (c20.Checked == true) { ck20 = 1; } else { ck20 = 0; }            if (c21.Checked == true) { ck21 = 1; } else { ck21 = 0; }
            if (c22.Checked == true) { ck22 = 1; } else { ck22 = 0; }            if (c23.Checked == true) { ck23 = 1; } else { ck23 = 0; }            if (c24.Checked == true) { ck24 = 1; } else { ck24 = 0; }
            if (c25.Checked == true) { ck25 = 1; } else { ck25 = 0; }            if (c26.Checked == true) { ck26 = 1; } else { ck26 = 0; }            if (c27.Checked == true) { ck27 = 1; } else { ck27 = 0; }
            if (c28.Checked == true) { ck28 = 1; } else { ck28 = 0; }            if (c29.Checked == true) { ck29 = 1; } else { ck29 = 0; }            if (c30.Checked == true) { ck30 = 1; } else { ck30 = 0; }
            if (c31.Checked == true) { ck31 = 1; } else { ck31 = 0; }            if (c32.Checked == true) { ck32 = 1; } else { ck32 = 0; }            if (c33.Checked == true) { ck33 = 1; } else { ck33 = 0; }
            if (c34.Checked == true) { ck34 = 1; } else { ck34 = 0; }            if (c35.Checked == true) { ck35 = 1; } else { ck35 = 0; }            if (c36.Checked == true) { ck36 = 1; } else { ck36 = 0; }
            if (c37.Checked == true) { ck37 = 1; } else { ck37 = 0; }            if (c38.Checked == true) { ck38 = 1; } else { ck38 = 0; }            if (c39.Checked == true) { ck39 = 1; } else { ck39 = 0; }
            if (c40.Checked == true) { ck40 = 1; } else { ck40 = 0; }            if (c41.Checked == true) { ck41 = 1; } else { ck41 = 0; }            if (c42.Checked == true) { ck42 = 1; } else { ck42 = 0; }
            if (c43.Checked == true) { ck43 = 1; } else { ck43 = 0; }            if (c44.Checked == true) { ck44 = 1; } else { ck44 = 0; }            if (c45.Checked == true) { ck45 = 1; } else { ck45 = 0; }
            if (c46.Checked == true) { ck46 = 1; } else { ck46 = 0; }            if (c47.Checked == true) { ck47 = 1; } else { ck47 = 0; }            if (c48.Checked == true) { ck48 = 1; } else { ck48 = 0; }
            if (c49.Checked == true) { ck49 = 1; } else { ck49 = 0; }            if (c50.Checked == true) { ck50 = 1; } else { ck50 = 0; }

            try
            {

                    string query = "exec b2_user_permission_insert_update @userId,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28,@p29,@p30,@p31,@p32,@p33,@p34,@p35,@p36,@p37,@p38,@p39,@p40,@p41,@p42,@p43,@p44,@p45,@p46,@p47,@p48,@p49,@p50";
                    SqlCommand cmd = new SqlCommand(query, dBConn);                    
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@p1", ck1);                    cmd.Parameters.AddWithValue("@p2", ck2);                    cmd.Parameters.AddWithValue("@p3", ck3);
                    cmd.Parameters.AddWithValue("@p4", ck4);                    cmd.Parameters.AddWithValue("@p5", ck5);                    cmd.Parameters.AddWithValue("@p6", ck6);
                    cmd.Parameters.AddWithValue("@p7", ck7);                    cmd.Parameters.AddWithValue("@p8", ck8);                    cmd.Parameters.AddWithValue("@p9", ck9);
                    cmd.Parameters.AddWithValue("@p10", ck10);                    cmd.Parameters.AddWithValue("@p11", ck11);                    cmd.Parameters.AddWithValue("@p12", ck12);
                    cmd.Parameters.AddWithValue("@p13", ck13);                    cmd.Parameters.AddWithValue("@p14", ck14);                    cmd.Parameters.AddWithValue("@p15", ck15);
                    cmd.Parameters.AddWithValue("@p16", ck16);                    cmd.Parameters.AddWithValue("@p17", ck17);                    cmd.Parameters.AddWithValue("@p18", ck18);
                    cmd.Parameters.AddWithValue("@p19", ck19);                    cmd.Parameters.AddWithValue("@p20", ck20);                    cmd.Parameters.AddWithValue("@p21", ck21);
                    cmd.Parameters.AddWithValue("@p22", ck22);                    cmd.Parameters.AddWithValue("@p23", ck23);                    cmd.Parameters.AddWithValue("@p24", ck24);
                    cmd.Parameters.AddWithValue("@p25", ck25);                    cmd.Parameters.AddWithValue("@p26", ck26);                    cmd.Parameters.AddWithValue("@p27", ck27);
                    cmd.Parameters.AddWithValue("@p28", ck28);                    cmd.Parameters.AddWithValue("@p29", ck29);                    cmd.Parameters.AddWithValue("@p30", ck30);
                    cmd.Parameters.AddWithValue("@p31", ck31);                    cmd.Parameters.AddWithValue("@p32", ck32);                    cmd.Parameters.AddWithValue("@p33", ck33);
                    cmd.Parameters.AddWithValue("@p34", ck34);                    cmd.Parameters.AddWithValue("@p35", ck35);                    cmd.Parameters.AddWithValue("@p36", ck36);
                    cmd.Parameters.AddWithValue("@p37", ck37);                    cmd.Parameters.AddWithValue("@p38", ck38);                    cmd.Parameters.AddWithValue("@p39", ck39);
                    cmd.Parameters.AddWithValue("@p40", ck40);                    cmd.Parameters.AddWithValue("@p41", ck41);                    cmd.Parameters.AddWithValue("@p42", ck42);
                    cmd.Parameters.AddWithValue("@p43", ck43);                    cmd.Parameters.AddWithValue("@p44", ck44);                    cmd.Parameters.AddWithValue("@p45", ck45);
                    cmd.Parameters.AddWithValue("@p46", ck46);                    cmd.Parameters.AddWithValue("@p47", ck47);                    cmd.Parameters.AddWithValue("@p48", ck48);
                    cmd.Parameters.AddWithValue("@p49", ck49);                    cmd.Parameters.AddWithValue("@p50", ck50);
                    dBConn.Open();
                    int result = cmd.ExecuteNonQuery();
                    dBConn.Close();
                    if(result > 0)
                    {
                        MessageBox.Show("Permission saved success");
                    }
                    ClearAllCheckBox();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void ClearAllCheckBox()
        {
            c1.Checked = false;            c2.Checked = false;            c3.Checked = false;            c4.Checked = false;            c5.Checked = false;
            c6.Checked = false;            c7.Checked = false;            c8.Checked = false;            c9.Checked = false;            c10.Checked = false;
            c11.Checked = false;            c12.Checked = false;            c13.Checked = false;            c14.Checked = false;            c15.Checked = false;
            c16.Checked = false;            c17.Checked = false;            c18.Checked = false;            c19.Checked = false;            c20.Checked = false;
            c21.Checked = false;            c22.Checked = false;            c23.Checked = false;            c24.Checked = false;
            c25.Checked = false;            c26.Checked = false;            c27.Checked = false;            c28.Checked = false;
            c29.Checked = false;            c30.Checked = false;            c31.Checked = false;           c32.Checked = false;
            c33.Checked = false;            c34.Checked = false;            c35.Checked = false;            c36.Checked = false;
            c37.Checked = false;            c38.Checked = false;            c39.Checked = false;            c40.Checked = false;
            c41.Checked = false;            c42.Checked = false;            c43.Checked = false;            c44.Checked = false;
            c45.Checked = false;            c46.Checked = false;            c47.Checked = false;            c48.Checked = false;
            c49.Checked = false;            c50.Checked = false; 
        }

        private void findUsersBtn_Click(object sender, EventArgs e)
        {
            int user_id = GetUserId();
            int ck1, ck2, ck3, ck4, ck5, ck6, ck7, ck8, ck9, ck10, ck11, ck12, ck13, ck14, ck15, ck16, ck17, ck18, ck19, ck20, ck21, ck22, ck23, ck24, ck25, ck26, ck27, ck28, ck29, ck30, ck31, ck32, ck33, ck34, ck35, ck36, ck37, ck38, ck39, ck40, ck41, ck42, ck43, ck44, ck45, ck46, ck47, ck48, ck49, ck50;

                string query1 = "select * from b2_user_permission where userId='"+user_id+"'";
                SqlCommand command2 = new SqlCommand(query1, dBConn);
                dBConn.Open();
                SqlDataReader reader = command2.ExecuteReader();
                while (reader.Read())
                {
                    //fetch data
                    ck1 = int.Parse(reader["P1"].ToString());                    ck2 = int.Parse(reader["P2"].ToString());                    ck3 = int.Parse(reader["P3"].ToString());
                    ck4 = int.Parse(reader["P4"].ToString());                    ck5 = int.Parse(reader["P5"].ToString());                    ck6 = int.Parse(reader["P6"].ToString());
                    ck7 = int.Parse(reader["P7"].ToString());                    ck8 = int.Parse(reader["P8"].ToString());                    ck9 = int.Parse(reader["P9"].ToString());
                    ck10 = int.Parse(reader["P10"].ToString());                    ck11 = int.Parse(reader["P11"].ToString());                    ck12 = int.Parse(reader["P12"].ToString());
                    ck13 = int.Parse(reader["P13"].ToString());                    ck14 = int.Parse(reader["P14"].ToString());                    ck15 = int.Parse(reader["P15"].ToString());
                    ck16 = int.Parse(reader["P16"].ToString());                    ck17 = int.Parse(reader["P17"].ToString());                    ck18 = int.Parse(reader["P18"].ToString());
                    ck19 = int.Parse(reader["P19"].ToString());                    ck20 = int.Parse(reader["P20"].ToString());                    ck21 = int.Parse(reader["P21"].ToString());
                    ck22 = int.Parse(reader["P22"].ToString());                    ck23 = int.Parse(reader["P23"].ToString());                    ck24 = int.Parse(reader["P24"].ToString());
                    ck25 = int.Parse(reader["P25"].ToString());                    ck26 = int.Parse(reader["P26"].ToString());                    ck27 = int.Parse(reader["P27"].ToString());
                    ck28 = int.Parse(reader["P28"].ToString());                    ck29 = int.Parse(reader["P29"].ToString());                    ck30 = int.Parse(reader["P30"].ToString());
                    ck31 = int.Parse(reader["P31"].ToString());                    ck32 = int.Parse(reader["P32"].ToString());                    ck33 = int.Parse(reader["P33"].ToString());
                    ck34 = int.Parse(reader["P34"].ToString());                    ck35 = int.Parse(reader["P35"].ToString());                    ck36 = int.Parse(reader["P36"].ToString());
                    ck37 = int.Parse(reader["P37"].ToString());                    ck38 = int.Parse(reader["P38"].ToString());                    ck39 = int.Parse(reader["P39"].ToString());
                    ck40 = int.Parse(reader["P40"].ToString());                    ck41 = int.Parse(reader["P41"].ToString());                    ck42 = int.Parse(reader["P42"].ToString());
                    ck43 = int.Parse(reader["P43"].ToString());                    ck44 = int.Parse(reader["P44"].ToString());                    ck45 = int.Parse(reader["P45"].ToString());
                    ck46 = int.Parse(reader["P46"].ToString());                    ck47 = int.Parse(reader["P47"].ToString());                    ck48 = int.Parse(reader["P48"].ToString());
                    ck49 = int.Parse(reader["P49"].ToString());                    ck50 = int.Parse(reader["P50"].ToString());                    
                    //set data  // 1 for check // 0 for uncheck
                    if (ck1 == 1) { c1.Checked = true; } else { c1.Checked = false;}                    if (ck2 == 1) {c2.Checked = true;} else { c2.Checked = false;}
                    if (ck3 == 1) {c3.Checked = true;} else { c3.Checked = false;}                    if (ck4 == 1) {c4.Checked = true;} else { c4.Checked = false;}
                    if (ck5 == 1) {c5.Checked = true;} else { c5.Checked = false;}                    if (ck6 == 1) {c6.Checked = true;} else { c6.Checked = false;}
                    if (ck7 == 1) {c7.Checked = true;} else { c7.Checked = false;}                    if (ck8 == 1) {c8.Checked = true;} else { c8.Checked = false;}
                    if (ck9 == 1) {c9.Checked = true;} else { c9.Checked = false;}                    if (ck10 == 1) {c10.Checked = true;} else { c10.Checked = false;}
                    if (ck11 == 1) {c11.Checked = true;} else { c11.Checked = false;}                    if (ck12 == 1) {c12.Checked = true;} else { c12.Checked = false;}
                    if (ck13 == 1) {c13.Checked = true;} else { c13.Checked = false;}                    if (ck14 == 1) {c14.Checked = true;} else { c14.Checked = false;}
                    if (ck15 == 1) {c15.Checked = true;} else { c15.Checked = false;}                    if (ck16 == 1) {c16.Checked = true;} else { c16.Checked = false;}
                    if (ck17 == 1) {c17.Checked = true;} else { c17.Checked = false;}                    if (ck18 == 1) {c18.Checked = true;} else { c18.Checked = false;}
                    if (ck19 == 1) {c19.Checked = true;} else { c19.Checked = false;}                    if (ck20 == 1) {c20.Checked = true;} else { c20.Checked = false;}
                    if (ck21 == 1) {c21.Checked = true;} else { c21.Checked = false;}                    if (ck22 == 1) {c22.Checked = true;} else { c22.Checked = false;}
                    if (ck23 == 1) {c23.Checked = true;} else { c23.Checked = false;}                    if (ck24 == 1) {c24.Checked = true;} else { c24.Checked = false;}
                    if (ck25 == 1) {c25.Checked = true;} else { c25.Checked = false;}                    if (ck26 == 1) {c26.Checked = true;} else { c26.Checked = false;}
                    if (ck27 == 1) {c27.Checked = true;} else { c27.Checked = false;}                    if (ck28 == 1) {c28.Checked = true;} else { c28.Checked = false;}
                    if (ck29 == 1) {c29.Checked = true;} else { c29.Checked = false;}                    if (ck30 == 1) {c30.Checked = true;} else { c30.Checked = false;}
                    if (ck31 == 1) {c31.Checked = true;} else { c31.Checked = false;}                    if (ck32 == 1) {c32.Checked = true;} else { c32.Checked = false;}
                    if (ck33 == 1) {c33.Checked = true;} else { c33.Checked = false;}                    if (ck34 == 1) {c34.Checked = true;} else { c34.Checked = false;}
                    if (ck35 == 1) {c35.Checked = true;} else { c35.Checked = false;}                    if (ck36 == 1) {c36.Checked = true;} else { c36.Checked = false;}
                    if (ck37 == 1) {c37.Checked = true;} else { c37.Checked = false;}                    if (ck38 == 1) {c38.Checked = true;} else { c38.Checked = false;}
                    if (ck39 == 1) {c39.Checked = true;} else { c39.Checked = false;}                    if (ck40 == 1) {c40.Checked = true;} else { c40.Checked = false;}
                    if (ck41 == 1) {c41.Checked = true;} else { c41.Checked = false;}                    if (ck42 == 1) {c42.Checked = true;} else { c42.Checked = false;}
                    if (ck43 == 1) {c43.Checked = true;} else { c43.Checked = false;}                    if (ck44 == 1) {c44.Checked = true;} else { c44.Checked = false;}
                    if (ck45 == 1) {c45.Checked = true;} else { c45.Checked = false;}                    if (ck46 == 1) {c46.Checked = true;} else { c46.Checked = false;}
                    if (ck47 == 1) {c47.Checked = true;} else { c47.Checked = false;}                    if (ck48 == 1) {c48.Checked = true;} else { c48.Checked = false;}
                    if (ck49 == 1) {c49.Checked = true;} else { c49.Checked = false;}                    if (ck50 == 1) {c50.Checked = true;} else { c50.Checked = false;}
                }
                dBConn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearAllCheckBox();
        }
    }
}
