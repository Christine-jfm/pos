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
    public partial class frmEmployee : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmEmployee()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void selectData()
        {
            try
            {
                string query = "Select EmpId, Concat(Concat(EmpFname, ' '), EmpLname), EmpAdd, EmpPosition, EmpDateReg, EmpActive from tblemployee";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dGridAccout.DataSource = dt;
                con.Close();

                this.dGridAccout.Columns[0].HeaderText = "Id No.";
                this.dGridAccout.Columns[1].HeaderText = "Name";
                this.dGridAccout.Columns[2].HeaderText = "Address";
                this.dGridAccout.Columns[3].HeaderText = "Position";
                this.dGridAccout.Columns[4].HeaderText = "Date Registration";
                this.dGridAccout.Columns[5].HeaderText = "Status";

                this.dGridAccout.Columns[0].Width = 50;

                dGridAccout.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void reset()
        {
            selectData();
            txtSearchName.Clear();
            btnNew.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            //selectData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchName != null && !string.IsNullOrWhiteSpace(txtSearchName.Text))
            {
                try
                {
                    string query = "Select EmpId, Concat(Concat(EmpFname, ' '), EmpLname), EmpAdd, EmpPosition, EmpDateReg, EmpActive from tblemployee where EmpFname like '" + txtSearchName.Text + "%' OR EmpLname like '" + txtSearchName.Text + "%'";
                    cmd = new MySqlCommand(query, con);
                    con.Open();
                    adpt = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    dGridAccout.DataSource = dt;
                    con.Close();

                    this.dGridAccout.Columns[0].HeaderText = "Id No.";
                    this.dGridAccout.Columns[1].HeaderText = "Name";
                    this.dGridAccout.Columns[2].HeaderText = "Address";
                    this.dGridAccout.Columns[3].HeaderText = "Position";
                    this.dGridAccout.Columns[4].HeaderText = "Date Registration";
                    this.dGridAccout.Columns[5].HeaderText = "Status";

                    this.dGridAccout.Columns[0].Width = 50;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
            else
            {
                this.dGridAccout.DataSource = null;
                this.dGridAccout.Rows.Clear();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmAddEmployee frm = new frmAddEmployee();
            account.IsEmpAdd = true;
            timer1.Start();
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (account.IsEmpAdd == false && account.IsEmpEdit == false)
            {
                timer1.Stop();
                reset();
            }
        }

        private void dGridAccout_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dGridAccout.Rows[e.RowIndex];
                string temp = row.Cells[0].Value.ToString();
                account.SelectEmpId = int.Parse(temp);
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmAddEmployee frm = new frmAddEmployee();
            account.IsEmpEdit = true;
            timer1.Start();
            frm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Delete?", "Delete Employee Information",
                  MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    String query = "Delete from tblemployee where EmpId = " + account.SelectEmpId;
                    cmd = new MySqlCommand(query, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Delete Successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void dGridAccout_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
