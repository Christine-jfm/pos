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
    public partial class frmAddEmployee : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmAddEmployee()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
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
            mtxtpicImage.Image = Properties.Resources.photo;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reset()
        {
            txtFname.Clear();
            txtMi.Clear();
            txtLname.Clear();
            txtAdd.Clear();
            dtBday.ResetText();
            txtAge.Clear();
            txtNickname.Clear();
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            txtContact.Clear();
            cboxStatus.SelectedIndex = -1;
            cBoxPosition.SelectedIndex = -1;
            txtRate.Clear();
            txtTin.Clear();
            txtSss.Clear();
            dtHired.ResetText();
            mtxtpicImage.Image = Properties.Resources.photo;

            dtBday.Value = dtBday.MaxDate;
        }

        private void addAccount()
        {
            string strGender = "Male";

            if (radioButton1.Checked)
            {
                strGender = "Female";
            }

            try
            {
                string query = @"Insert into tblemployee(EmpFname, EmpMname, EmpLname, EmpAdd, EmpBday, EmpAge, EmpNickname, EmpGender, EmpContact, EmpStatus, EmpPosition, EmpRate, EmpTin, EmpSss, EmpDateHired, EmpImage, EmpDateReg, EmpActive) 
                values (@d1, @d2, @d3, @d4, @d5, @d6, @d7, @d8, @d9, @d10, @d11, @d12, @d13, @d14, @d15, @d16, @d17, @d18)";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtFname.Text);
                cmd.Parameters.AddWithValue("@d2", txtMi.Text);
                cmd.Parameters.AddWithValue("@d3", txtLname.Text);
                cmd.Parameters.AddWithValue("@d4", txtAdd.Text);
                cmd.Parameters.AddWithValue("@d5", dtBday.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@d6", txtAge.Text);
                cmd.Parameters.AddWithValue("@d7", txtNickname.Text);
                cmd.Parameters.AddWithValue("@d8", strGender);
                cmd.Parameters.AddWithValue("@d9", txtContact.Text);
                cmd.Parameters.AddWithValue("@d10", cboxStatus.Text);
                cmd.Parameters.AddWithValue("@d11", cBoxPosition.Text);
                cmd.Parameters.AddWithValue("@d12", txtRate.Text);
                cmd.Parameters.AddWithValue("@d13", txtTin.Text);
                cmd.Parameters.AddWithValue("@d14", txtSss.Text);
                cmd.Parameters.AddWithValue("@d15", dtHired.Value.ToShortDateString());

                MemoryStream ms = new MemoryStream();
                Bitmap bmpImage = new Bitmap(mtxtpicImage.Image);
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] data = ms.GetBuffer();
                MySqlParameter p = new MySqlParameter("@d16", MySqlDbType.Blob);
                p.Value = data;
                cmd.Parameters.Add(p);

                cmd.Parameters.AddWithValue("@d17", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@d18", cboxActive.Text);

                con.Open();
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("Employee information added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string query = "update tblemployee set EmpAdd = @d1, EmpStatus = @d2, EmpContact = @d3, EmpPosition = @d4, EmpActive = @d5 where EmpId = " + account.SelectEmpId;
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtAdd.Text);
                cmd.Parameters.AddWithValue("@d2", cboxStatus.Text);
                cmd.Parameters.AddWithValue("@d3", txtContact.Text);
                cmd.Parameters.AddWithValue("@d4", cBoxPosition.Text);
                cmd.Parameters.AddWithValue("@d5", cboxActive.Text);
                con.Open();
                adpt = new MySqlDataAdapter(cmd); 
                 DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("Employee Information Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtFname.Text == "")
            {
                MessageBox.Show("Please Input First Name!", "Message");
                return;
            }
            if (txtMi.Text == "")
            {
                MessageBox.Show("Please Input Middle Initial!", "Message");
                return;
            }
            if (txtLname.Text == "")
            {
                MessageBox.Show("Please Input Last Name!", "Message");
                return;
            }
            if (txtAdd.Text == "")
            {
                MessageBox.Show("Please Input Address!", "Message");
                return;
            }
            if (txtContact.Text == "")
            {
                MessageBox.Show("Please Input Contact No.!", "Message");
                return;
            }
            if(cboxStatus.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Civil Status", "Message");
                return;
            }

            if (cBoxPosition.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Position", "Message");
                return;
            }

            if (account.IsEmpAdd)
            {
                addAccount();
            }
            else
            {
                updAccount();
            }

            txtFname.Clear();
            txtMi.Clear();
            txtLname.Clear();
            txtAdd.Clear();
            dtBday.ResetText();
            txtAge.Clear();
            txtNickname.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtContact.Clear();
            cboxStatus.SelectedIndex = -1;
            cBoxPosition.SelectedIndex = -1;
            txtRate.Clear();
            txtTin.Clear();;
            txtSss.Clear();
            dtHired.ResetText();
        }

        private void frmAddEmployee_Load(object sender, EventArgs e)
        {
            dtHired.MaxDate = DateTime.Now;
            cboxActive.SelectedIndex = 0;
            cboxActive.Enabled = false;
            radioButton1.Checked = false;
            dtBday.Value = dtBday.MaxDate;

            if (account.IsEmpAdd == false)
            {
                getData();

                cboxActive.Enabled = true;
                button1.Enabled = false;
                txtFname.Enabled = false;
                txtMi.Enabled = false;
                txtLname.Enabled = false;
                dtBday.Enabled = false;
                txtAge.Enabled = false;
                txtNickname.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                //cBoxPosition.Enabled = false;
                //txtRate.Enabled = falsee;
              txtTin.Enabled = false;  
                txtSss.Enabled = false;
                dtHired.Enabled = false;

                if(cboxActive.SelectedIndex == 1)
                {
                    cBoxPosition.Enabled = false;
                    txtRate.Enabled = false;
                }
            }
        }

        private void frmAddEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            account.IsEmpAdd = false;
            account.IsEmpEdit = false;
            this.Hide();
            e.Cancel = true;
        }

        private void dtBday_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void dtBday_ValueChanged(object sender, EventArgs e)
        {
            int years = DateTime.Now.Year - dtBday.Value.Year;

            if (dtBday.Value.AddYears(years) > DateTime.Now) years--;
            txtAge.Text = years.ToString();
        }

        private void getData()
        {
            try
            {
                string query = "Select * from tblemployee where EmpId = " + account.SelectEmpId;
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    txtFname.Text = dt.Rows[0]["EmpFname"].ToString();
                    txtMi.Text = dt.Rows[0]["EmpMname"].ToString();
                    txtLname.Text = dt.Rows[0]["EmpLname"].ToString();
                    txtAdd.Text = dt.Rows[0]["EmpAdd"].ToString();
                    dtBday.Text = dt.Rows[0]["EmpBday"].ToString();
                    txtAge.Text = dt.Rows[0]["EmpAge"].ToString();
                    txtNickname.Text = dt.Rows[0]["EmpNickname"].ToString();
                    cboxActive.Text = dt.Rows[0]["EmpActive"].ToString();

                    string temp = dt.Rows[0]["EmpGender"].ToString();
                    if(temp == "Female")
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                    }

                    txtContact.Text = dt.Rows[0]["EmpContact"].ToString();
                    cboxStatus.Text = dt.Rows[0]["EmpStatus"].ToString();
                    cBoxPosition.Text = dt.Rows[0]["EmpPosition"].ToString();
                    txtRate.Text = dt.Rows[0]["EmpRate"].ToString();
                    txtTin.Text = dt.Rows[0]["EmpTin"].ToString();
                    txtSss.Text = dt.Rows[0]["EmpSss"].ToString();
                    dtHired.Text = dt.Rows[0]["EmpDateHired"].ToString();

                    byte[] img = (byte[])dt.Rows[0]["EmpImage"];
                    MemoryStream ms = new MemoryStream(img);
                    mtxtpicImage.Image = Image.FromStream(ms);
                    adpt.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void txtFname_Leave(object sender, EventArgs e)
        {
            txtFname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFname.Text);
            txtFname.Text = txtFname.Text.TrimEnd();
        }

        private void txtMi_Leave(object sender, EventArgs e)
        {
            txtMi.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtMi.Text);
            txtMi.Text = txtMi.Text.TrimEnd();
        }

        private void txtLname_Leave(object sender, EventArgs e)
        {
            txtLname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLname.Text);
            txtLname.Text = txtLname.Text.TrimEnd();
        }

        private void txtAdd_Leave(object sender, EventArgs e)
        {
            txtAdd.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtAdd.Text);
            txtAdd.Text = txtAdd.Text.TrimEnd();
        }

        private void txtNickname_Leave(object sender, EventArgs e)
        {
            txtNickname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtNickname.Text);
            txtNickname.Text = txtNickname.Text.TrimEnd();
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTin_KeyPress(object sender, KeyPressEventArgs e)
        {
          //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
          //{
          //    e.Handled = true;
          //}
        }

        private void txtSss_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtFname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar);
        }

        private void txtMi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar);
        }

        private void txtLname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar);
        }

        private void txtNickname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar);
        }

        private void cboxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (account.IsEmpAdd == false)
            {
                if(cboxStatus.SelectedIndex == 1)
                {
                    txtMi.Enabled = true;
                    txtLname.Enabled = true;
                }
            }
        }

        private void cboxActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboxActive.SelectedIndex == 0)
            {
                cBoxPosition.Enabled = true;
                txtRate.Enabled = true;
            }
            else
            {
                cBoxPosition.Enabled = false;
                txtRate.Enabled = false;
            }
        }
    }
}
