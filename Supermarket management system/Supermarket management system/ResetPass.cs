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
    public partial class ResetPass : Form
    {
        UserAccount user;
        public ResetPass(UserAccount user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void confirmbtn_Click(object sender, EventArgs e)
        {
            if(newpassTxt.Text != renewpassTxt.Text)
            {
                guna2MessageDialog2.Show("Passwords do no match, please try again!");
            }
            DialogResult result = guna2MessageDialog1.Show("Are you sure you want to update user pass?");
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(Users.connection))
                {
                    con.Open();
                    string query = "update dtUsers set password = @pass where username =@user ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@pass", newpassTxt.Text);
                    cmd.Parameters.AddWithValue("@user", user.User);                    
                    cmd.ExecuteNonQuery();
                    guna2MessageDialog2.Show("User password has been updated!");
                    this.Dispose();
                }
            }
        }
    }
}
