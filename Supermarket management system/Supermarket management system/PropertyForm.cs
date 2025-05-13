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
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    public partial class PropertyForm : Form
    {
        UserAccount user;
        public PropertyForm(UserAccount user)
        {
            InitializeComponent();
            this.user = user;
            nameTxt.Text = user.User;
            roleTxt.Text = user.Role;
        }

        private void confirmbtn_Click(object sender, EventArgs e)
        {
            if (statusCombo.Text == "")
            {
                guna2MessageDialog1.Show("Please select user status!");
            }
            DialogResult result = guna2MessageDialog1.Show("Are you sure you want to update user status?");
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(Users.connection))
                {
                    con.Open();
                    string query = "update dtUsers set active = @active where username =@user ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@active", statusCombo.Text);
                    cmd.Parameters.AddWithValue("@user", nameTxt.Text);
                    cmd.ExecuteNonQuery();
                    guna2MessageDialog2.Show("User status has been updated!");
                    user.LoadUserData();
                    this.Dispose();
                }
            }
        }
    }
}
