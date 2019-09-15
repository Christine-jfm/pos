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
    public partial class frmLogs : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmLogs()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private string strLogs = "";
        private void Logs2()
        {
            string query = "Insert into tblLogs(userId, LogsOper, LogsDate) values (@d1, @d2, @d3)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@d1", account.UserAccountId);
            cmd.Parameters.AddWithValue("@d2", strLogs);
            cmd.Parameters.AddWithValue("@d3", DateTime.Now.ToString());
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
        }

        private void selectData()
        {
            String query = "Select tblLogs.LogsId, tblaccount.userName, tblaccount.userType, tblLogs.LogsOper, tblLogs.LogsDate from tblLogs inner join tblaccount on tblLogs.userId = tblaccount.userId ORDER BY tblLogs.LogsId DESC";
            cmd = new MySqlCommand(query, con);
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            this.dataGridView1.Columns[0].HeaderText = "Id No.";
            this.dataGridView1.Columns[1].HeaderText = "Username";
            this.dataGridView1.Columns[2].HeaderText = "Usertype";
            this.dataGridView1.Columns[3].HeaderText = "Operation";
            this.dataGridView1.Columns[4].HeaderText = "Date.";
        }

        private void frmLogs_Load(object sender, EventArgs e)
        {
            selectData();

            strLogs = "View Transaction Logs";
            //Logs2();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
