using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace POSFM
{
    public partial class frmLogin : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        int temp = 3;
        public frmLogin()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void login()
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please input your username/password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string query = "Select * from tblAccount where userName = '" + txtUsername.Text + "' And userPass = '" + txtPassword.Text + "'";
                adpt = new MySqlDataAdapter(query, con);
                con.Open();
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    if(dr["status"].ToString() == "Inactive")
                    {
                        MessageBox.Show("Your account in Inactive, Please contact System administrator.", "Message");
                        return;
                    }

                    account.UserAccountId = (int)dr["userId"];
                    account.Username = dr["userName"].ToString();
                    account.Usertype = dr["userType"].ToString();
                    account.UserFullName = dr["userFullName"].ToString();

                    frmMenu frm = new frmMenu();
                    this.Hide();
                    frm.Show();
                }
                else
                {
                    temp -= 1;
                    if (temp == 0)
                    {
                        MessageBox.Show("Too many invalid login attempts. System Restart!", "Application Close");
                        Application.Exit();
                    }
                    MessageBox.Show("Incorrect Password!" + "\n" + "Remaining login attempt: " + temp, "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
            Logs();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Logs()
        {
            string query = "Insert into tblLogs(userId, LogsOper, LogsDate) values (@d1, @d2, @d3)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@d1", account.UserAccountId);
            cmd.Parameters.AddWithValue("@d2", "System Login");
            cmd.Parameters.AddWithValue("@d3", DateTime.Now.ToString());
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            con.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmforgetPass frm2 = new frmforgetPass();
            this.Hide();
            frm2.Show(); ;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                txtPassword.PasswordChar = '\0';
            else
                txtPassword.PasswordChar = '●';
        }
    }
}
