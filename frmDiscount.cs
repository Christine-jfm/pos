using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSFM
{
    public partial class frmDiscount : Form
    {
        public frmDiscount()
        {
            InitializeComponent();
        }

        private void frmDiscount_FormClosing(object sender, FormClosingEventArgs e)
        {
            //frmTransaction frm = new frmTransaction();
            //frm.checkBox1.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            account.DiscNo = txtId.Text;
            account.DiscName = txtName.Text;
            this.Close();
        }
    }
}
