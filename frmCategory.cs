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
    public partial class frmCategory : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        int prodId = 0;
        bool isAdd = false;
        public frmCategory()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void selectData()
        {
            try
            {
                string query = "Select * from tblcategory";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dGridAccout.DataSource = dt;
                con.Close();

                this.dGridAccout.Columns[0].HeaderText = "No.";
                this.dGridAccout.Columns[1].HeaderText = "Category";
                this.dGridAccout.Columns[0].Width = 40;


                dGridAccout.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            selectData();
        }

        private void reset()
        {
            selectData();
            txtCategory.Clear();
            txtCategory.Enabled = false;
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            isAdd = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtCategory.Clear();
            txtCategory.Focus();
            txtCategory.Enabled = true;
            btnNew.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            isAdd = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string query = "";
            if(isAdd)
            {
               query = "Insert into tblcategory (categoryItem) values ('" + txtCategory.Text + "') ";
            }
            else
            {
                query = "Update tblcategory set categoryItem = '" + txtCategory.Text + "' where categoryId = " + prodId;
            }
            try
            {
                cmd = new MySqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Successful!", "Message");
                reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete?", "Delete Product?",
             MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    String query = "Delete from tblcategory where categoryId = " + prodId;
                    cmd = new MySqlCommand(query, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
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

        private void dGridAccout_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dGridAccout.Rows[e.RowIndex];
                prodId = int.Parse(row.Cells[0].Value.ToString());
                txtCategory.Text = row.Cells[1].Value.ToString();
                btnNew.Enabled = true;
                btnEdit.Enabled = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtCategory.Enabled = true;
            btnNew.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            isAdd = false;
        }

        private void frmCategory_FormClosing(object sender, FormClosingEventArgs e)
        {
            account.IsCategory = true;
        }
    }
}
