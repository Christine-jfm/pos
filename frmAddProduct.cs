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
using System.IO;
using System.Drawing.Imaging;
using System.Globalization;

using System.Text.RegularExpressions;
namespace POSFM
{
    public partial class frmAddProduct : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        bool isExist = false;
        public frmAddProduct()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void Reset()
        {
            cboxCat.SelectedIndex = -1;
            txtDesc.Clear();
            txtPrice.Clear();
            txtDescInfo.Clear();
            mtxtpicImage.Image = Properties.Resources.default_image;
        }

        private void addAccount()
        {
            decimal price = decimal.Parse(txtPrice.Text);
            string strPrice = price.ToString("#,##0.00");

            try
            {
                string query = "Insert into tblproduct(ProdCat, ProdDesc, ProdPrice, ProdDateReg, ProdImage, ProdDescInfo, Status) values (@d1, @d2, @d3, @d4, @d5, @d6, @d7)";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", cboxCat.Text);
                cmd.Parameters.AddWithValue("@d2", txtDesc.Text);
                cmd.Parameters.AddWithValue("@d3", strPrice);
                cmd.Parameters.AddWithValue("@d4", DateTime.Now.ToString());

                MemoryStream ms = new MemoryStream();
                Bitmap bmpImage = new Bitmap(mtxtpicImage.Image);
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] data = ms.GetBuffer();
                MySqlParameter p = new MySqlParameter("@d5", MySqlDbType.Blob);
                p.Value = data;
                cmd.Parameters.Add(p);
                cmd.Parameters.AddWithValue("@d6", txtDescInfo.Text);
                cmd.Parameters.AddWithValue("@d7", "Active");

                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("New Product added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);


                strOper = "Add New Product: " + txtDesc.Text;
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
            decimal price = decimal.Parse(txtPrice.Text);
            string strPrice = price.ToString("#,##0.00");

            try
            {
                string query = "update tblproduct set ProdCat = @d1, ProdDesc = @d2, ProdPrice = @d3, ProdImage = @d4, ProdDescInfo = @d5 where ProdId = " + account.SelectProdId;
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", cboxCat.Text);
                cmd.Parameters.AddWithValue("@d2", txtDesc.Text);
                cmd.Parameters.AddWithValue("@d3", strPrice);

                MemoryStream ms = new MemoryStream();
                Bitmap bmpImage = new Bitmap(mtxtpicImage.Image);
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] data = ms.GetBuffer();
                MySqlParameter p = new MySqlParameter("@d4", MySqlDbType.Blob);
                p.Value = data;
                cmd.Parameters.Add(p);

                cmd.Parameters.AddWithValue("@d5", txtDescInfo.Text);

                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("Product Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                strOper = "Update New Product: " + txtDesc.Text;
                Logs();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        string strOper = "";
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
            if(cboxCat.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Item Category!", "Message");
                return;
            }

            if (txtDesc.Text == "")
            {
                MessageBox.Show("Please Input Item Description!", "Message");
                return;
            }

            if (txtPrice.Text == "")
            {
                MessageBox.Show("Please Input Item Price!", "Message");
                return;
            }

            if (account.isprodAdd)
            {
                chkExist();
                if(isExist)
                {
                    isExist = false;
                    MessageBox.Show("Product item already exist!", "Message");
                    return;
                }

                addAccount();
                Logs();
            }
            else
            {
                updAccount();
            }
        }

        private void chkExist()
        {
            try
            {
                con.Open();
                string query = "select * from tblproduct WHERE ProdCat = '"+cboxCat.Text+ "' AND ProdDesc = '"+ txtDesc.Text + "'";
                adpt = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if(count > 0)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
                con.Close();
            }
        }
        private void frmAddProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            account.isprodAdd = false;
            account.IsProdEdit = false;
            this.Hide();
            e.Cancel = true;
        }

        private void frmAddProduct_Load(object sender, EventArgs e)
        {
            cboxSelect();
            if (account.isprodAdd == false)
            {
                getData();
                button2.Enabled = true;
            }
        }

        private void getData()
        {
            try
            {
                string query = "Select * from tblproduct where ProdId = " + account.SelectProdId;
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    cboxCat.Text = dt.Rows[0]["ProdCat"].ToString();
                    txtDesc.Text = dt.Rows[0]["ProdDesc"].ToString();
                    txtPrice.Text = dt.Rows[0]["ProdPrice"].ToString();

                    byte[] img = (byte[])dt.Rows[0]["ProdImage"];
                    MemoryStream ms = new MemoryStream(img);
                    mtxtpicImage.Image = Image.FromStream(ms);
                    adpt.Dispose();

                    txtDescInfo.Text = dt.Rows[0]["ProdDescInfo"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var _with1 = openFileDialog1;

                _with1.Filter = ("Image Files |*.png; *.bmp; *.jpg;*.jpeg; *.gif;");
                _with1.FilterIndex = 4;
                openFileDialog1.FileName = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    mtxtpicImage.Image = Image.FromFile(openFileDialog1.FileName);
                    button2.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mtxtpicImage.Image = Properties.Resources.default_image;
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 46 || (char.IsControl(e.KeyChar)))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar);
        }

        private void txtDesc_Leave(object sender, EventArgs e)
        {
            txtDesc.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtDesc.Text);
            txtDesc.Text = txtDesc.Text.TrimEnd();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
