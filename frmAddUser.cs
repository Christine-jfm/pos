using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace POSFM
{
    public partial class frmAddUser : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        bool isExist = false;
        string strOper = "";
        public frmAddUser()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reset()
        {
            txtFull.Clear();
            txtUname.Clear();
            txtPassword.Clear();
            txtRePass.Clear();
            cboxType.SelectedIndex = -1;
            checkBox1.Checked = false;
        }

        private void addAccount()
        {
            try
            {
                string query = "Insert into tblAccount(userName, userPass, userType, userFullName, DateReg, accQuestion, accAnswer) values (@d1, @d2, @d3, @d4, @d5, @d6, @d7)";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtUname.Text);
                cmd.Parameters.AddWithValue("@d2", txtPassword.Text);
                cmd.Parameters.AddWithValue("@d3", cboxType.Text);
                cmd.Parameters.AddWithValue("@d4", txtFull.Text);
                cmd.Parameters.AddWithValue("@d5", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@d6", comboBox1.Text);
                cmd.Parameters.AddWithValue("@d7", textBox1.Text);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("User account added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                strOper = "Add new Account: " + txtUname.Text;
                Logs();

                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void updAccount()
        {
            try
            {
                string query = "update tblaccount set userName = @d1, userPass = @d2, userType = @d3, userFullName = @d4, accQuestion = @d5, accAnswer = @d6 where userId = " + account.SelectUserId;
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtUname.Text);
                cmd.Parameters.AddWithValue("@d2", txtPassword.Text);
                cmd.Parameters.AddWithValue("@d3", cboxType.Text);
                cmd.Parameters.AddWithValue("@d4", txtFull.Text);
                cmd.Parameters.AddWithValue("@d5", comboBox1.Text);
                cmd.Parameters.AddWithValue("@d6", textBox1.Text);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("User Account Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                strOper = "Update Account: " + txtUname.Text;
                Logs();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void Logs()
        {
            string query = "Insert into tblLogs(userId, LogsOper, LogsDate) values (@d1, @d2, @d3)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@d1", account.UserAccountId);
            cmd.Parameters.AddWithValue("@d2", strOper);
            cmd.Parameters.AddWithValue("@d3", DateTime.Now.ToString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFull.Text == "")
            {
                MessageBox.Show("Please enter Fullname!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFull.Focus();
                return;
            }

            if (txtUname.Text == "")
            {
                MessageBox.Show("Please Select Username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUname.Focus();
                return;
            }

            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please enter Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }

            if (account.IsUserAdd)
            {
                if (txtRePass.Text == "")
                {
                    MessageBox.Show("Please enter Retype Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRePass.Focus();
                    return;
                }
            }
                
            if(comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Please select secret question", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please input secret answer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }

            if (account.IsUserAdd)
            {
                if (txtPassword.Text != txtRePass.Text)
                {
                    MessageBox.Show("Please enter Password Not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRePass.Focus();
                    return;
                }
            }
            

            if (cboxType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Usertype!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboxType.Focus();
                return;
            }

            if (account.IsUserAdd)
            {
                chk();
            }
            if (isExist)
            {
                MessageBox.Show("Username already exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isExist = false;
            }
            else
            {
                if (account.IsUserAdd)
                {
                    addAccount();
                }
                else
                {
                    updAccount();
                }
            }            
        }

        private void chk()
        {
            string query = "select * from tblAccount where userName = '" + txtUname.Text + "'";
            cmd = new MySqlCommand(query, con);
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            con.Close();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                isExist = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                txtPassword.PasswordChar = '\0';
            else
                txtPassword.PasswordChar = '●';
        }

        private void frmAddUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            account.IsUserAdd = false;
            account.IsUserEdit = false;
            this.Hide();
            e.Cancel = true;
        }

        private void getData()
        {
            try
            {
                string query = "Select * from tblaccount where userId = " + account.SelectUserId;
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    txtFull.Text = dt.Rows[0]["userFullName"].ToString();
                    txtUname.Text = dt.Rows[0]["userName"].ToString();
                    txtPassword.Text = dt.Rows[0]["userPass"].ToString();
                    cboxType.Text = dt.Rows[0]["userType"].ToString();
                    comboBox1.Text = dt.Rows[0]["accQuestion"].ToString();
                    textBox1.Text = dt.Rows[0]["accAnswer"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            if(account.IsUserAdd == false)
            {
                getData();
                txtRePass.Visible = false;
                label2.Visible = false;
                cboxType.Location = new Point(115, 96);
                label1.Location = new Point(3, 100);
                pane_Info.Size = new Size(457, 127);
                panel1.Location = new Point(10, 145);
                //panelBtn.Location = new Point(280, 145);
            }
        }
    }
}
