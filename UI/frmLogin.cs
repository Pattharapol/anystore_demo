using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmLogin : Form
    {
        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();
        string title = "C# dev by TIK";
        public static string loggedIn;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            l.username = txtUserName.Text.Trim();
            l.password = txtPassword.Text.Trim();
            l.user_type = cmbUserType.Text.Trim();

            bool result = dal.loginCheck(l);
            if(result == true)
            {
                MessageBox.Show("Login Successfully", title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                loggedIn = l.username;
                switch (l.user_type)
                {
                    case "Admin":
                        {
                            frmAdminDashBoard admin = new frmAdminDashBoard();
                            this.Hide();
                            admin.ShowDialog();
                        }
                        break;

                    case "User":
                        {
                            frmUserDashBoard user = new frmUserDashBoard();
                            this.Hide();
                            user.ShowDialog();
                        }
                        break;

                    default:
                        {
                            MessageBox.Show("Invalid User, Please contact Admin...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show("Login Failed", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
