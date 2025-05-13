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
    public partial class ChangePass : Form
    {
        public ChangePass()
        {
            InitializeComponent();
            unameTxt.Text = Users.Uname;
        }

        private void savebtn2_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(unameTxt.Text) ||
                    String.IsNullOrEmpty(oldpassTxt.Text) ||
                    String.IsNullOrEmpty(newpassTxt.Text) ||
                    String.IsNullOrEmpty(renewpassTxt.Text))
                {
                    guna2MessageDialog1.Show("Please fill all the required information to add this user!");
                }
                else if (newpassTxt.Text != renewpassTxt.Text)
                {
                    guna2MessageDialog1.Show("Passwords do not match! Re-enter the passwords");

                }
                else
                {
                    DialogResult result = guna2MessageDialog2.Show("Are you sure you want to update this User??");

                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "select * from dtUsers where username = @uname";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@uname", unameTxt.Text);
                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                if (oldpassTxt.Text == reader["password"].ToString())
                                {
                                    reader.Close();
                                    string queryS = "update dtUsers set password =@pass where username =@user";
                                    SqlCommand cmdS = new SqlCommand(queryS, con);
                                    cmdS.Parameters.AddWithValue("@pass", newpassTxt.Text);
                                    cmdS.Parameters.AddWithValue("@user", unameTxt.Text);
                                    cmdS.ExecuteNonQuery();
                                    guna2MessageDialog1.Show(".......User has been updated.......");
                                    oldpassTxt.Clear();
                                    newpassTxt.Clear();
                                    renewpassTxt.Clear();
                                }
                                else
                                {
                                    guna2MessageDialog1.Show("Incorrect password, please try again!");

                                }
                            }
                            else
                            {
                                guna2MessageDialog1.Show("Invalid username and password, Please try again!");

                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Show(ex.Message);
            }
        }

        private void cancelbtn2_Click(object sender, EventArgs e)
        {
            oldpassTxt.Clear();
            newpassTxt.Clear();
            renewpassTxt.Clear();
        }
    }
}
