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
    public partial class frmSupplier : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmSupplier()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmAddSupplier frm = new frmAddSupplier();
            account.IsSupplierAdd = true;
            timer1.Start();
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (account.IsSupplierAdd == false && account.IsSupplierEdit == false)
            {
                timer1.Stop();
                reset();
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

        private void selectData()
        {
            try
            {
                string query = "Select Concat(supplierId), supplierName, supplierAddress, supplierProduct from tblSupplier";
                //string query = "Select Concat(LEFT(ProdDesc, 1), ProdId), ProdCat, ProdDesc, ProdPrice, ProdDescInfo from tblProduct WHERE Status = 'Active'";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dGridAccout.DataSource = dt;
                con.Close();

                this.dGridAccout.Columns[0].HeaderText = "Id No.";
                this.dGridAccout.Columns[1].HeaderText = "Supplier Name";
                this.dGridAccout.Columns[2].HeaderText = "Supplier Address";
                this.dGridAccout.Columns[3].HeaderText = "Supplier Product";


                this.dGridAccout.Columns[0].Width = 50;
                this.dGridAccout.Columns[1].Width = 65;
                this.dGridAccout.Columns[3].Width = 50;

                dGridAccout.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reset();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmAddSupplier frm = new frmAddSupplier();
            account.IsSupplierEdit = true;
            timer1.Start();
            frm.ShowDialog();
        }

        private void dGridAccout_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dGridAccout.Rows[e.RowIndex];
                string temp = row.Cells[0].Value.ToString();
                account.SelectSupplierId = int.Parse(temp);
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Delete?", "Delete Supplier Account",
                  MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    String query = "Delete from tblsupplier where supplierId = " + account.SelectSupplierId;
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
    }
    

}
