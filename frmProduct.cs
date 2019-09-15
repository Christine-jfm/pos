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
    public partial class frmProduct : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmProduct()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void selectData()
        {
            try
            {
                string query = "Select Concat(LEFT(ProdDesc, 1), ProdId), ProdCat, ProdDesc, ProdPrice, ProdDescInfo, Status from tblProduct";
                //string query = "Select Concat(LEFT(ProdDesc, 1), ProdId), ProdCat, ProdDesc, ProdPrice, ProdDescInfo from tblProduct WHERE Status = 'Active'";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dGridAccout.DataSource = dt;
                con.Close();

                this.dGridAccout.Columns[0].HeaderText = "Id No.";
                this.dGridAccout.Columns[1].HeaderText = "Category";
                this.dGridAccout.Columns[2].HeaderText = "Name";
                this.dGridAccout.Columns[3].HeaderText = "Price";
                this.dGridAccout.Columns[4].HeaderText = "Description";

                this.dGridAccout.Columns[0].Width = 50;
                this.dGridAccout.Columns[1].Width = 80;
                this.dGridAccout.Columns[3].Width = 65;

                dGridAccout.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void cboxSelect()
        {
            try
            {
                String query = "Select * from tblcategory";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int row = 0;
                while (row < dt.Rows.Count)
                {
                    cboxCat.Items.Add(dt.Rows[row]["categoryItem"].ToString());
                    ++row;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            //selectData();
            cboxSelect();
            if (account.Usertype == "Cashier")
            {
                paneBtn.Visible = false;
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchName != null && !string.IsNullOrWhiteSpace(txtSearchName.Text))
            {
                try
                {
                    string query = "Select ProdId, ProdCat, ProdDesc, ProdPrice, ProdDescInfo from tblProduct where ProdDesc like '" + txtSearchName.Text + "%' AND Status = 'Active'";
                    cmd = new MySqlCommand(query, con);
                    con.Open();
                    adpt = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    dGridAccout.DataSource = dt;
                    con.Close();

                    this.dGridAccout.Columns[0].HeaderText = "Id No.";
                    this.dGridAccout.Columns[1].HeaderText = "Category";
                    this.dGridAccout.Columns[2].HeaderText = "Name";
                    this.dGridAccout.Columns[3].HeaderText = "Price";
                    this.dGridAccout.Columns[4].HeaderText = "Description";

                    this.dGridAccout.Columns[0].Width = 50;
                    this.dGridAccout.Columns[1].Width = 80;
                    this.dGridAccout.Columns[3].Width = 65;
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

        private void reset()
        {
            selectData();
            txtSearchName.Clear();
            cboxCat.SelectedIndex = -1;
            btnNew.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmAddProduct frm = new frmAddProduct();
            account.isprodAdd = true;
            timer1.Start();
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (account.isprodAdd == false && account.IsProdEdit == false)
            {
                timer1.Stop();
                reset();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void dGridAccout_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dGridAccout.Rows[e.RowIndex];
                string temp = row.Cells[0].Value.ToString().Substring(1);
                account.SelectProdId = int.Parse(temp);
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmAddProduct frm = new frmAddProduct();
            account.IsProdEdit = true;
            timer1.Start();
            frm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete?", "Delete Product?",
             MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    string query = "Delete from tblproduct where ProdId = " + account.SelectProdId;
                    cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@d1", "Inactive");
                    con.Open();
                    adpt = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adpt.Fill(ds);
                    con.Close();
                    MessageBox.Show("Delete Successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
        }

        private void cboxCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchName.Clear();
            try
            {
                string query = "Select Concat(LEFT(ProdDesc, 1), ProdId), ProdCat, ProdDesc, ProdPrice, ProdDescInfo from tblProduct where ProdCat = '" + cboxCat.Text + "' AND Status = 'Active'";

                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dGridAccout.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            this.dGridAccout.Columns[0].HeaderText = "Id No.";
            this.dGridAccout.Columns[1].HeaderText = "Category";
            this.dGridAccout.Columns[2].HeaderText = "Name";
            this.dGridAccout.Columns[3].HeaderText = "Price";
            this.dGridAccout.Columns[4].HeaderText = "Description";

            this.dGridAccout.Columns[0].Width = 50;
            this.dGridAccout.Columns[1].Width = 80;
            this.dGridAccout.Columns[3].Width = 65;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmCategory frm = new frmCategory();
            cboxCat.SelectedIndex = -1;
            timer_category.Start();
            frm.ShowDialog();
        }

        private void timer_category_Tick(object sender, EventArgs e)
        {
            if(account.IsCategory)
            {
                timer_category.Stop();
                account.IsCategory = false;
                cboxCat.Items.Clear();
                cboxSelect();
            }
        }

        private void dGridAccout_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
