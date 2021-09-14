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
    public partial class frmAdminDashBoard : Form
    {
        public frmAdminDashBoard()
        {
            InitializeComponent();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers frm = new frmUsers();
            frm.ShowDialog();
        }

        private void frmAdminDashBoard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategories f = new frmCategories();
            f.ShowDialog();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProducts frm = new frmProducts();
            frm.ShowDialog();
        }

        private void dealerCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust frm = new frmDeaCust();
            frm.ShowDialog();
        }
    }
}
