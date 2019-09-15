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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.IO;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections;

namespace POSFM
{
    public partial class frmTransaction : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;

        string strOper = "";
        public frmTransaction()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
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
                cboxCat.Items.Add("All");
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
        private void selectData()
        {
            try
            {
                string query = "Select ProdId AS 'No.', ProdCat AS 'Category', ProdDesc AS 'Description', ProdPrice AS 'Price' from tblproduct";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dGridAccout.DataSource = dt;
                con.Close();
                this.dGridAccout.Columns[0].Width = 45;
                this.dGridAccout.Columns[1].Width = 75;
                this.dGridAccout.Columns[2].Width = 170;
                dGridAccout.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void frmTransaction_Load(object sender, EventArgs e)
        {
            cboxSelect();
            getProduct();
            selectTransId();
            selectData();
            lViewPOS.Columns[0].Width = 0;
        }

        private void cboxCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            List_prodId.Clear();
            List_cat.Clear();
            List_desc.Clear();
            List_price.Clear();

            txtQty.Clear();
            txtSuppName.Clear();
            txtQty.Enabled = false;
            btnSave.Enabled = false;

            if(cboxCat.SelectedIndex == 0)
            {
                getProduct();
                return;
            }

            try
            {
                string query = "Select * from tblproduct WHERE ProdCat = '"+cboxCat.Text+"'";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Button b = new Button();
                        b.Name = dt.Rows[i]["ProdId"].ToString();
                        b.Size = new Size(100, 100);

                        byte[] img = (byte[])dt.Rows[i]["ProdImage"];
                        MemoryStream ms = new MemoryStream(img);
                        //b.Image = Image.FromStream(ms);
                        b.BackgroundImage = Image.FromStream(ms);
                        b.BackgroundImageLayout = ImageLayout.Stretch;

                        adpt.Dispose();
                        b.Click += new EventHandler(OnButtonClick);
                        flowLayoutPanel1.Controls.Add(b);

                        List_prodId.Add(dt.Rows[i]["ProdId"].ToString());
                        List_cat.Add(dt.Rows[i]["ProdCat"].ToString());
                        List_desc.Add(dt.Rows[i]["ProdDesc"].ToString());
                        List_price.Add(dt.Rows[i]["ProdPrice"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void selectTransId()
        {
            String query = "SELECT MAX(Order_TransNo) FROM tblorder";
            cmd = new MySqlCommand(query, con);
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            con.Close();

            int count = ds.Tables[0].Rows.Count;
            DataRow dr = ds.Tables[0].Rows[0];
            if (!dr[0].ToString().Equals(""))
            {
                int countId = Convert.ToInt32(dr[0].ToString()) + 1;
                txtTransNo.Text = String.Format("{0:D5}", countId);
            }
            else
            {
                txtTransNo.Text = "1" + String.Format("{0:D5}", 1);
            }
        }

        private string strIdNum = "";
        private string strCat = "";
        private string strDesc = "";
        private decimal itemPrice = 0;
        
        private void dGridAccout_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dGridAccout.Rows[e.RowIndex];
                strIdNum = dGridAccout.CurrentCell.RowIndex.ToString();
                itemPrice = decimal.Parse(row.Cells[3].Value.ToString());
                txtSuppName.Text = row.Cells[1].Value.ToString() + "-" + row.Cells[2].Value.ToString();
                txtQty.Text = "1";
                txtQty.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void reset()
        {
            txtQty.Clear();
            txtQty.Enabled = false;
            txtSuppName.Clear();
            selectData();
            btnSave.Enabled = false;            
            checkBox1.Checked = false;
            ItemClick = 0;
            txtDesc.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtQty.Text) < 1)
            {
                MessageBox.Show("Please Input a correct quantity", "Error");
                txtQty.Text = "1";
                return;
            }

            if (isEditQty)
            {
                isEditQty = false;
                try
                {
                    bool isExist = false;
                    foreach (ListViewItem item in lViewPOS.Items)
                    {
                        if (item.SubItems[0].Text == strIdNum)
                        {
                            isExist = true;
                            item.SubItems[3] = new ListViewItem.ListViewSubItem()
                            {
                                Text = (int.Parse(txtQty.Text)).ToString()
                            };
                            item.SubItems[4] = new ListViewItem.ListViewSubItem()
                            {

                                Text = (decimal.Parse(txtQty.Text) * itemPrice).ToString("#,##0.00")
                            };
                            reset();
                            total();
                            break;
                        }
                    }

                    if (isExist == false)
                    {
                        ListViewItem itm = new ListViewItem(strIdNum);
                        itm.SubItems.Add(strCat);
                        itm.SubItems.Add(strDesc);
                        itm.SubItems.Add(txtQty.Text);
                        decimal pricTot = decimal.Parse(txtQty.Text) * itemPrice;

                        itm.SubItems.Add(pricTot.ToString());
                        lViewPOS.Items.Add(itm);
                        txtQty.Clear();
                        txtSuppName.Clear();                        
                        reset();
                        total();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    bool isExist = false;
                    foreach (ListViewItem item in lViewPOS.Items)
                    {
                        if (item.SubItems[0].Text == strIdNum)
                        {
                            isExist = true;
                            item.SubItems[3] = new ListViewItem.ListViewSubItem()
                            {
                                Text = (int.Parse(item.SubItems[3].Text.ToString()) + int.Parse(txtQty.Text)).ToString()
                            };
                            item.SubItems[4] = new ListViewItem.ListViewSubItem()
                            {
                                Text = (decimal.Parse(item.SubItems[4].Text.ToString()) + itemPrice).ToString("#,##0.00")
                            };
                            total();
                            reset();
                            break;
                        }
                    }

                    if (isExist == false)
                    {
                        ListViewItem itm = new ListViewItem(strIdNum);
                        itm.SubItems.Add(strCat);
                        itm.SubItems.Add(strDesc);
                        itm.SubItems.Add(txtQty.Text);
                        decimal pricTot = decimal.Parse(txtQty.Text) * itemPrice;

                        itm.SubItems.Add(pricTot.ToString());
                        lViewPOS.Items.Add(itm);
                        txtQty.Clear();
                        txtSuppName.Clear();
                        reset();
                        total();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cboxCat.SelectedIndex = -1;
            flowLayoutPanel1.Controls.Clear();
            List_prodId.Clear();
            List_cat.Clear();
            List_desc.Clear();
            List_price.Clear();

            selectTransId();
            reset();
            txtTableNo.Clear();
            textBox1.Clear();
            lViewPOS.Items.Clear();
            total();
            lblTotSales.Text = "0.00";
            lblVat.Text = "0.00";
            ItemClick = 0;
            getProduct();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lViewPOS.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select item you want to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ListViewItem eachItem in lViewPOS.SelectedItems)
            {
                lViewPOS.Items.Remove(eachItem);
            }
            total();
        }

        bool isEditQty = false;
        private void button2_Click(object sender, EventArgs e)
        {
            isEditQty = true;

            if (lViewPOS.SelectedItems.Count > 0)
            {
                for (var i = 0; i < List_prodId.Count; i++)
                {
                    if (List_prodId[i].Equals(lViewPOS.SelectedItems[0].Text.ToString()))
                    {
                        itemPrice = decimal.Parse(List_price[i].ToString());
                        txtSuppName.Text = List_desc[i].ToString();
                        strCat = List_cat[i].ToString();
                        strDesc = List_desc[i].ToString();
                        txtDesc.Text = List_descInfo[i].ToString();
                    }
                }
                strIdNum = lViewPOS.SelectedItems[0].Text.ToString();
                foreach (ListViewItem item in lViewPOS.SelectedItems)
                {
                    txtQty.Text = item.SubItems[3].Text.ToString();
                }
                txtQty.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please Select Item in Order List!", "Message");
            }

            //if (lViewPOS.SelectedItems.Count > 0)
            //{
            //    foreach (ListViewItem item in lViewPOS.SelectedItems)
            //    {
            //        item.SubItems[3] = new ListViewItem.ListViewSubItem()
            //        {
            //        Text = (int.Parse(item.SubItems[3].Text.ToString()) + 1).ToString()
            //        };
            //        item.SubItems[4] = new ListViewItem.ListViewSubItem()
            //        {
            //        Text = (decimal.Parse(item.SubItems[4].Text.ToString()) + regPrice).ToString("#,##0.00")
            //        };
            //        total();
            //        break;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please Select Item", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        decimal regPrice = 0;

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dGridAccout.CurrentCell.OwningRow;
            //DataGridViewRow row2 = dtGridService.CurrentCell.OwningRow;

            if (lViewPOS.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lViewPOS.SelectedItems)
                {
                    if(int.Parse(item.SubItems[3].Text.ToString()) == 1)
                    {
                        foreach (ListViewItem eachItem in lViewPOS.SelectedItems)
                        {
                            lViewPOS.Items.Remove(eachItem);
                            lblTotSales.Text = "0.00";
                            lblVat.Text = "0.00";
                        }
                    }
                    else
                    {
                        item.SubItems[3] = new ListViewItem.ListViewSubItem()
                        {
                            Text = (int.Parse(item.SubItems[3].Text.ToString()) - 1).ToString()
                        };
                        item.SubItems[4] = new ListViewItem.ListViewSubItem()
                        {
                            Text = (decimal.Parse(item.SubItems[4].Text.ToString()) - regPrice).ToString("#,##0.00")
                        };
                    }
                    total();
                    break;
                }
            }
            else
            {
                MessageBox.Show("Please Select Item", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
                
        private void total()
        {
            double total = 0;
            if (lViewPOS.Items.Count == 0)
            {
                total = 0;
                lblAmounDue.Text = total.ToString("0.00");
            }
            foreach (ListViewItem l in lViewPOS.Items)
            {
                total += Convert.ToDouble(l.SubItems[4].Text);
                if (checkBox1.Checked == true)
                {
                    double totVat = total * 0.12;
                    double vatLess = total - totVat;

                    double discount = vatLess * 0.20;
                    double totDis = vatLess - discount;
                    lblDiscount.Text = discount.ToString("#,##0.00");
                    label9.Text = "20%";
                    lblAmounDue.Text = totDis.ToString("#,##0.00");
                    lblTotSales.Text = total.ToString("#,##0.00");
                    lblVat.Text = "0.00";
                    label6.Text = "0% VAT ";
                }
                else
                {
                    double tax = total - (total * 0.12);
                    double taxless = total - tax;
                    lblDiscount.Text = "0.00";
                    label9.Text = "0%";
                    lblTotSales.Text = (total - taxless).ToString("#,##0.00");
                    lblVat.Text = taxless.ToString("#,##0.00");
                    lblAmounDue.Text = total.ToString("#,##0.00");
                    label6.Text = "12% VAT";
                }

                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                frmDiscount frm = new frmDiscount();
                frm.ShowDialog();
            }

            total();
        }

        private void addProduction()
        {
            string temp = "";
            if(txtTableNo.Text == "")
            {
                temp = "-";
            }
            else
            {
                temp = txtTableNo.Text;
            }
            try
            {
                string query = "insert into tblproduction (Order_TransNo, tableNo, remarks, Status, orderType) values (@d1, @d2, @d3, @d4, @d5)";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtTransNo.Text);
                cmd.Parameters.AddWithValue("@d2", temp);
                cmd.Parameters.AddWithValue("@d3", textBox1.Text);
                cmd.Parameters.AddWithValue("@d4", "Pending");
                cmd.Parameters.AddWithValue("@d5", account.OrderType);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void saveDiscount()
        {
            try
            {
                con.Open();
                string query = "Insert into tbldiscount (discountCardNo, discountName, dateRegs) values ('"+account.DiscNo+"', '"+account.DiscName+"','"+ DateTime.Now.ToString() + "')";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lViewPOS.Items.Count == 0)
            {
                MessageBox.Show("No items in listview!", "Message");
                return;
            }

            insertData();
            saveDiscount();
            addProduction();
            account.TransNo = int.Parse(txtTransNo.Text);
            frmPayment frm = new frmPayment();
            frm.txtTotal.Text = lblAmounDue.Text;
            account.StrTableNo = txtTableNo.Text;

            strOper = "Create new transaction: " + txtTransNo.Text;
            Logs();

            timer1.Start();
            frm.ShowDialog();
        }
        private void insertData()
        {
            try
            {
                for (int i = 0; i <= lViewPOS.Items.Count - 1; i++)
                {
                    string query = "insert into tblorder (Order_ProdId, Order_Qty, Order_TotalPrice, Order_Discount, Order_TotalSales, Order_Vat, Order_AmountDue, Order_TransNo, Order_TransDate, ProdDesc, fullUserName) values (@d1, @d2, @d3, @d4, @d5, @d6, @d7, @d8, @d9, @d10, @d11)";
                    cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@d1", lViewPOS.Items[i].SubItems[0].Text.ToString());
                    cmd.Parameters.AddWithValue("@d2", lViewPOS.Items[i].SubItems[3].Text.ToString());
                    cmd.Parameters.AddWithValue("@d3", lViewPOS.Items[i].SubItems[4].Text.ToString());
                    cmd.Parameters.AddWithValue("@d4", lblDiscount.Text);
                    cmd.Parameters.AddWithValue("@d5", lblTotSales.Text);
                    cmd.Parameters.AddWithValue("@d6", lblVat.Text);
                    cmd.Parameters.AddWithValue("@d7", lblAmounDue.Text);
                    cmd.Parameters.AddWithValue("@d8", txtTransNo.Text);
                    cmd.Parameters.AddWithValue("@d9", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@d10", lViewPOS.Items[i].SubItems[2].Text.ToString());
                    cmd.Parameters.AddWithValue("@d11", account.Username);
                    con.Open();
                    adpt = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adpt.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(account.IsTrans)
            {
                account.IsTrans = false;
                selectTransId();
                reset();
                lViewPOS.Items.Clear();
                txtTableNo.Clear();
                textBox1.Clear();
                total();
                lblTotSales.Text = "0.00";
                lblVat.Text = "0.00";
            }
        }

        ArrayList List_prodId = new ArrayList();
        ArrayList List_cat = new ArrayList();
        ArrayList List_desc = new ArrayList();
        ArrayList List_price = new ArrayList();
        ArrayList List_descInfo = new ArrayList();

        private void getProduct()
        {
            try
            {
                string query = "Select * from tblproduct";
                cmd = new MySqlCommand(query, con);
                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Button b = new Button();
                        b.Name = dt.Rows[i]["ProdId"].ToString();
                        b.Size = new Size(100, 100);
                        
                        byte[] img = (byte[])dt.Rows[i]["ProdImage"];
                        MemoryStream ms = new MemoryStream(img);
                        //b.Image = Image.FromStream(ms);
                        b.BackgroundImage = Image.FromStream(ms);
                        b.BackgroundImageLayout = ImageLayout.Stretch;

                        adpt.Dispose();
                        b.Click += new EventHandler(OnButtonClick);
                        flowLayoutPanel1.Controls.Add(b);

                        List_prodId.Add(dt.Rows[i]["ProdId"].ToString());
                        List_cat.Add(dt.Rows[i]["ProdCat"].ToString());
                        List_desc.Add(dt.Rows[i]["ProdDesc"].ToString());
                        List_price.Add(dt.Rows[i]["ProdPrice"].ToString());
                        List_descInfo.Add(dt.Rows[i]["ProdDescInfo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        int ItemClick = 0;
        private void OnButtonClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            for (var i = 0; i < List_prodId.Count; i++)
            {
                if(List_prodId[i].Equals(b.Name.ToString()))
                {
                    //MessageBox.Show(List_cat[i].ToString() + "\n" + List_desc[i].ToString() + "\n" + List_price[i].ToString());
                    if(strIdNum.Equals(List_prodId[i]))
                    {
                        ItemClick += 1;
                    }
                    else
                    {
                        ItemClick = 1;
                    }
                    itemPrice = decimal.Parse(List_price[i].ToString());
                    txtSuppName.Text = List_desc[i].ToString();
                    strCat = List_cat[i].ToString();
                    strDesc = List_desc[i].ToString();
                    txtDesc.Text = List_descInfo[i].ToString();
                }
            }
            strIdNum = b.Name.ToString();
            txtQty.Text = ItemClick.ToString();            
            txtQty.Enabled = true;
            btnSave.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            account.OrderType = "Take-Out";
            btnDineIn.Enabled = true;
            button7.Enabled = false;
        }

        private void btnDineIn_Click(object sender, EventArgs e)
        {
            account.OrderType = "Dine-In";
            btnDineIn.Enabled = false;
            button7.Enabled = true;
        }

        private void txtTableNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmProduction frm = new frmProduction();
            frm.Show();
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

        private void button8_Click(object sender, EventArgs e)
        {
            if (lViewPOS.Items.Count == 0)
            {
                MessageBox.Show("No items in listview!", "Message");
                return;
            }

            insertData();
            saveDiscount();
            addProduction();
            account.TransNo = int.Parse(txtTransNo.Text);
            frmPayment frm = new frmPayment();
            frm.txtTotal.Text = lblAmounDue.Text;
            account.StrTableNo = txtTableNo.Text;

            strOper = "Create new transaction: " + txtTransNo.Text;
            Logs();

            timer1.Start();
            frm.ShowDialog();
        
        }

        private void txtTransNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

