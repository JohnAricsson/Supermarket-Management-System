using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supermarket_management_system
{
    public partial class Login : Form
    {
        private int retry = 4;
        public Login()
        {
            InitializeComponent();
            this.AcceptButton = loginbtn; 
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            if (usernameTxt.Text == "" || passwordTxt.Text == "")
            {
                guna2MessageDialog1.Show("Please enter username and password");
            }
            else if (Users.IsValid(usernameTxt.Text, passwordTxt.Text))
            {
                if(Users.Status == "Active")
                {
                    if (Users.Role == "Admin")
                    {
                        HomePage home = new HomePage();
                        home.Show();
                        this.Hide();
                    }
                    if (Users.Role == "Cashier")
                    {
                        CashierHome home = new CashierHome();
                        home.Show();
                        this.Hide();
                    }
                }
                else
                {
                    guna2MessageDialog1.Show("User Id is inactive!");
                }
               
            }
            else
            {
                retry -= 1;
                guna2MessageDialog1.Show("Invalid username or password, please try again! \nAttemps left : " + retry);
                if (retry == 0)
                {
                    guna2MessageDialog1.Show("Too many failed login attempts. Please try again later.");
                    usernameTxt.Enabled = false;
                    passwordTxt.Enabled = false;
                    guna2ImageButton1.Visible = false;
                    guna2ImageButton2.Visible = false;

                }
            }
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            passwordTxt.UseSystemPasswordChar = true;
            guna2ImageButton2.Visible = false;
            guna2ImageButton1.BringToFront();
            guna2ImageButton1.Visible = true;
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            passwordTxt.UseSystemPasswordChar = false;
            passwordTxt.PasswordChar = '\0';
            guna2ImageButton1.Visible = false;
            guna2ImageButton2.BringToFront();
            guna2ImageButton2.Visible = true;
        }
    }
}
