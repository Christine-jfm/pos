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
    public partial class frmforgetPass : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        bool isExist = false;
        string strQuest = "";
        string strAns = "";
        public frmforgetPass()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void chkExistName()
        {
            try
            {
                string query = "Select * FROM tblAccount WHERE userName = '" + txtUname.Text + "'";
                adpt = new MySqlDataAdapter(query, con);
                con.Open();
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    isExist = true;
                    strQuest = dt.Rows[0]["accQuestion"].ToString();
                    strAns = dt.Rows[0]["accAnswer"].ToString();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void txtUname_Leave(object sender, EventArgs e)
        {
            chkExistName();
            if (isExist == true)
            {
                isExist = false;
                lblQuestion.Text = strQuest;
                return;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUname.Text == "")
            {
                MessageBox.Show("Please enter User Name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUname.Focus();
                return;
            }

            if (lblQuestion.Text == "----------------------------")
            {
                MessageBox.Show("Please enter Valid Username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUname.Focus();
                return;

            }

            if (txtAnswer.Text == "")
            {
                MessageBox.Show("Please enter Answer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAnswer.Focus();
                return;
            }

            if (!(strAns.Equals(txtAnswer.Text)))
            {
                MessageBox.Show("Secret answer did not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAnswer.Focus();
                return;
            }

            account.Get_strUsername = txtUname.Text;
            frmNewPass frm = new frmNewPass();
            this.Hide();
            frm.Show();
        }
    }
}
