using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class frmUserDashBoard : Form
    {

        // Set a public method whether the FORM is purchsae or sales
        public static string transactionType;

        public frmUserDashBoard()
        {
            InitializeComponent();
        }

        private void frmUserDashBoard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
        }


        private void dealerCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust frm = new frmDeaCust();
            frm.ShowDialog();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //set value on transactionType static method
            transactionType = "Purchase";

            frmPurchaseAndSale frm = new frmPurchaseAndSale();
            frm.ShowDialog();

        }

        private void salesFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            transactionType = "Sale";

            frmPurchaseAndSale frm = new frmPurchaseAndSale();
            frm.ShowDialog();
        }
    }
}
