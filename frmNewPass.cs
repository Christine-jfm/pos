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
    public partial class frmNewPass : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmNewPass()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                txtPassword.PasswordChar = '\0';
            else
                txtPassword.PasswordChar = '●';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.TextLength < 6)
            {
                MessageBox.Show("Password must be atleast 6 character", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            if (txtPassword.Text != txtRetype.Text)
            {
                MessageBox.Show("Password Not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }

            try
            {
                string query = "update tblAccount set userPass = @uPass where userName = '" + account.Get_strUsername + "'";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@uPass", txtPassword.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Account Password Successfully Recovered!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmLogin frm = new frmLogin();
                this.Hide();
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }
    }
}
