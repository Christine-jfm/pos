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
    public partial class frmMenu : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;

        string strOper = "";
        public frmMenu()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            lblaccount.Text = account.Username;
            lbltype.Text = account.Usertype;

            if(account.Usertype == "Cashier")
            {
                button7.Visible = false;
                button9.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button8.Visible = false;
                button2.Visible = false;
                button11.Visible = false;
                button10.Visible = false;


                button6.Location = new Point(12, 89);
                button5.Location = new Point(12, 154);
            }
            timerDate.Start();
        }

        private void timerDate_Tick(object sender, EventArgs e)
        {
            DateTime dtime = DateTime.Now;
            lblTime.Text = dtime.ToString("h:mm:ss tt");
            lblDate.Text = DateTime.Today.ToShortDateString();
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
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "System Logout",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Hide();
                frmLogin frml = new frmLogin();
                strOper = "System Logout";
                Logs();
                frml.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            strOper = "Visit User Module";
            Logs();
            frmUser frm = new frmUser();
            frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmEmployee frm = new frmEmployee();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strOper = "Visit Product Module";
            Logs();
            frmProduct frm = new frmProduct();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            strOper = "Visit POS Module";
            Logs();
            frmTransaction frm = new frmTransaction();
            frm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            strOper = "Visit Logs Module";
            Logs();
            frmLogs frm = new frmLogs();
            frm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            strOper = "Visit Utilities Module";
            Logs();
            frmUtilities frm = new frmUtilities();
            frm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            strOper = "Visit Reports Module";
            Logs();
            frmReports frm = new frmReports();
            frm.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            strOper = "Visit Suppliers Module";
            Logs();
            frmSupplier frm = new frmSupplier();
            frm.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            strOper = "Visit Inventory Module";
            Logs();
            frmInventory frm = new frmInventory();
            frm.ShowDialog();
        }
    }
}
