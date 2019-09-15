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
using System.Collections;

namespace POSFM
{
    public partial class frmProduction : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlDataAdapter adpt;
        MySqlCommand cmd;
        public frmProduction()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
            timerDate.Start();
        }

        ArrayList List_transId = new ArrayList();
        ArrayList List_tableNo = new ArrayList();
        ArrayList List_time = new ArrayList();
        ArrayList List_Item = new ArrayList();
        ArrayList List_remarks = new ArrayList();
        ArrayList List_ordType = new ArrayList();
        ArrayList ordQty = new ArrayList();
        List<string> singleOrd = new List<string>();
        ArrayList oneOrder = new ArrayList();

        string strTest = "";
        string strTest2 = "";
        string chk = "";
        int num = 0;
        private void selectData()
        {
            try
            {
                string @query = @"SELECT tblproduction.`Order_TransNo`, tblproduction .`tableNo`,
                tblorder.ProdDesc, tblproduction.`remarks`, tblproduction.orderType, tblorder.Order_Qty, TIME_FORMAT(STR_TO_DATE(tblorder.Order_TransDate, '%c/%e/%Y %H:%i'), '%h:%i %p') AS 'Order_TransDate'
                FROM `tblproduction` 
                left join tblorder 
                on tblproduction.`Order_TransNo` = tblorder.`Order_TransNo` 
                WHERE tblproduction.Status = 'Pending'
                ORDER BY tblproduction.`Order_TransNo` ASC;";
                adpt = new MySqlDataAdapter(query, con);
                con.Open();
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (List_transId.Count == 0)
                    {
                        List_transId.Add(dt.Rows[i]["Order_TransNo"].ToString());
                        List_tableNo.Add(dt.Rows[i]["tableNo"].ToString());
                        List_remarks.Add(dt.Rows[i]["remarks"].ToString());
                        List_ordType.Add(dt.Rows[i]["orderType"].ToString());
                        List_time.Add(dt.Rows[i]["Order_TransDate"].ToString());
                    }
                    else
                    {
                        if (!List_transId.Contains(dt.Rows[i]["Order_TransNo"].ToString()))
                        {
                            List_transId.Add(dt.Rows[i]["Order_TransNo"].ToString());
                            List_tableNo.Add(dt.Rows[i]["tableNo"].ToString());
                            List_remarks.Add(dt.Rows[i]["remarks"].ToString());
                            List_ordType.Add(dt.Rows[i]["orderType"].ToString());
                            List_time.Add(dt.Rows[i]["Order_TransDate"].ToString());
                        }
                    }

                    string strItems = dt.Rows[i]["Order_TransNo"].ToString() + "-" + dt.Rows[i]["ProdDesc"].ToString() + " - " + dt.Rows[i]["Order_Qty"].ToString();
                    List_Item.Add(strItems);

                    singleOrd.Add(dt.Rows[i]["Order_TransNo"].ToString());
                }

                string temp11 = "";
                var q = from x in singleOrd
                        group x by x into g
                        let count = g.Count()
                        orderby count descending
                        select new { Value = g.Key, Count = count };
                foreach (var x in q)
                {
                    if(x.Count == 1)
                    {
                        temp11 += (x.Value) + "\n";
                        oneOrder.Add(temp11);
                    }
                }

                for (int ctr = 0; ctr < List_transId.Count; ctr++)
                {
                    Label lblTrans = (Label)this.Controls.Find("lblOrderNo" + (ctr + 0), true).FirstOrDefault();
                    Label lblTbl = (Label)this.Controls.Find("lbltblNo" + (ctr + 0), true).FirstOrDefault();
                    Label lblType = (Label)this.Controls.Find("lblOrdType" + (ctr + 0), true).FirstOrDefault();
                    Label lblProdTime = (Label)this.Controls.Find("lblTransTime" + (ctr + 0), true).FirstOrDefault(); 
                    Panel paneOrd = (Panel)this.Controls.Find("pane_" + (ctr + 0), true).FirstOrDefault();
                    TextBox txtbx = (TextBox)this.Controls.Find("textBox" + (ctr + 0), true).FirstOrDefault();

                    lblTrans.Text = List_transId[ctr].ToString();
                    lblTbl.Text = List_tableNo[ctr].ToString();
                    lblProdTime.Text = List_time[ctr].ToString();
                    txtbx.Text = List_remarks[ctr].ToString();
                    lblType.Text = List_ordType[ctr].ToString();
                    paneOrd.Visible = true;
                }

                //listview Items
                if (oneOrder.Count > 0)
                {
                    withSingleOrder();
                }
                else
                {
                    withoutSingleOder();
                }


            }
            catch (Exception)
            {
                return;
                //MessageBox.Show(ex.Message + "ASDFAS", "Error");
            }
        }

        private void withSingleOrder()
        {          
            for (int s = 0; s < List_Item.Count; s++)
            { 
                strTest = List_Item[s].ToString().Substring(0, 6);
                strTest2 = List_Item[s].ToString().Substring(7);
                //MessageBox.Show(strTest2);
                if (chk.ToString() == "")
                {
                    chk = strTest;
                }

                if (chk.Equals(strTest))
                {
                    ListView lview = (ListView)this.Controls.Find("listView" + (num + 0), true).FirstOrDefault();
                    lview.Items.Add(strTest2);
                }
                else
                {
                    foreach (string strChk in oneOrder)
                    {
                        if (strChk.Contains(strTest))
                        {
                            num += 1;
                            ListView lview = (ListView)this.Controls.Find("listView" + (num + 0), true).FirstOrDefault();
                            lview.Items.Add(strTest2);
                        }
                        else
                        {
                            chk = "";
                            num += 1;
                            ListView lview = (ListView)this.Controls.Find("listView" + (num + 0), true).FirstOrDefault();
                            lview.Items.Add(strTest2);
                        }
                    }
                }
            }
        }

        private void withoutSingleOder()
        {
            for (int s = 0; s < List_Item.Count; s++)
            {
                strTest = List_Item[s].ToString().Substring(0, 6);
                strTest2 = List_Item[s].ToString().Substring(7);
                if (chk.ToString() == "")
                {
                    chk = strTest;
                }

                if (chk.Equals(strTest))
                {
                    ListView lview = (ListView)this.Controls.Find("listView" + (num + 0), true).FirstOrDefault();
                    lview.Items.Add(strTest2);
                }
                else
                {
                    chk = "";
                    num += 1;
                    ListView lview = (ListView)this.Controls.Find("listView" + (num + 0), true).FirstOrDefault();
                    lview.Items.Add(strTest2);
                }
            }
        }

        private void frmProduction_Load(object sender, EventArgs e)
        {
            selectData();

            listView0.Columns[0].Width = button1.Width;
            listView1.Columns[0].Width = button1.Width;
            listView2.Columns[0].Width = button1.Width;
            listView3.Columns[0].Width = button1.Width;
            listView5.Columns[0].Width = button1.Width;
            listView6.Columns[0].Width = button1.Width;
            listView7.Columns[0].Width = button1.Width;
            listView8.Columns[0].Width = button1.Width;
            listView9.Columns[0].Width = button1.Width;
        }

        string strTransNo = "";
        private void updData()
        {
            string query = "update tblproduction set Status = @d1 where Order_TransNo = '" + strTransNo + "'";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@d1", "Served");
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            con.Close();
        }

        private void reset()
        {
            for (int ctr = 0; ctr < 10; ctr++)
            {
                Panel paneOrd = (Panel)this.Controls.Find("pane_" + (ctr + 0), true).FirstOrDefault();
                paneOrd.Visible = false;
            }            

            strTest = "";
            strTest2 = "";
            chk = "";
            num = 0;

            listView0.Items.Clear();
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            listView6.Items.Clear();
            listView7.Items.Clear();
            listView8.Items.Clear();
            listView9.Items.Clear();

            updData();
            List_transId.Clear();
            List_tableNo.Clear();
            List_Item.Clear();
            List_remarks.Clear();
            List_ordType.Clear();
            List_time.Clear();
            ordQty.Clear();
            ordQty.Clear();
            singleOrd.Clear();
            oneOrder.Clear();
            selectData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo0.Text;
            reset();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo1.Text;
            reset();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo2.Text;
            reset();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo3.Text;
            reset();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo4.Text;
            reset();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo5.Text;
            reset();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo6.Text;
            reset();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo7.Text;
            reset();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo8.Text;
            reset();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            strTransNo = lblOrderNo9.Text;
            reset();
        }

        private void timerDate_Tick(object sender, EventArgs e)
        {
            DateTime dtime = DateTime.Now;
            lblTime.Text = dtime.ToString("h:mm:ss tt");
            lblDate.Text = DateTime.Today.ToShortDateString();
        }

        private void listView0_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblOrderNo0_Click(object sender, EventArgs e)
        {

        }
    }
}
