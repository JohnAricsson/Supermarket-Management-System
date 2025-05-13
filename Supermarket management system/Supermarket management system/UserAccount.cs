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
    public partial class UserAccount : Form
    {
        HomePage home;
        private string user, name, role, status;
        public string User
        {
            get { return user; }
        }
        public string Role
        {
            get { return role;}
        }
        public UserAccount(HomePage home)
        {
            InitializeComponent();
            unameTxt.Text = Users.Uname;
            this.home = home;
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(usernameTxt.Text) ||
                    string.IsNullOrWhiteSpace(passwordTxt.Text) ||
                    string.IsNullOrWhiteSpace(repasswordTxt.Text) ||
                    string.IsNullOrWhiteSpace(nameTxt.Text))
                {
                    home.HomeMessageDialogue.Show("Please fill all the required information to add this user!");
                   
                }                                
                if (passwordTxt.Text != repasswordTxt.Text)
                {
                    home.HomeMessageDialogue.Show("Passwords do not match! Re-enter the passwords");
               
                }

                
                DialogResult result = home.HomeYesNoDialogue.Show("Are you sure you want to add this user?"); 
                if (result != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection con = new SqlConnection(Users.connection))
                {
                    con.Open();
                   
                    string checkQuery = "SELECT COUNT(*) FROM dtUsers WHERE username = @uname";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@uname", usernameTxt.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            home.HomeMessageDialogue.Show("Username already exists! Choose another username.");
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO dtUsers(username, password, role, fullname) VALUES(@Username, @Password, @Role, @Fullname)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                    {
                        insertCmd.Parameters.AddWithValue("@Username", usernameTxt.Text);
                        insertCmd.Parameters.AddWithValue("@Password", passwordTxt.Text);
                        insertCmd.Parameters.AddWithValue("@Role", roleCombo.Text);
                        insertCmd.Parameters.AddWithValue("@Fullname", nameTxt.Text);
                        insertCmd.ExecuteNonQuery();
                    }

                    usernameTxt.Clear();
                    passwordTxt.Clear();
                    repasswordTxt.Clear();
                    roleCombo.SelectedIndex = -1; 
                    nameTxt.Clear();

                    home.HomeMessageDialogue.Show("User has been added successfully!");
                }
            }
            catch (Exception ex)
            {
                home.HomeMessageDialogue.Show("Error: " + ex.Message);
            }
        }


        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            usernameTxt.Clear();  
            passwordTxt.Clear();
            repasswordTxt.Clear();
            roleCombo.Text = "";
            nameTxt.Clear();
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
                    home.HomeMessageDialogue.Show("Please fill all the required information to add this user!");
                }
                else if (newpassTxt.Text != renewpassTxt.Text)
                {
                    home.HomeMessageDialogue.Show("Passwords do not match! Re-enter the passwords");

                }
                else
                {
                    DialogResult result = home.HomeYesNoDialogue.Show("Are you sure you want to update this user?");

                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "select * from dtUsers where username = @uname";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@uname",unameTxt.Text);
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
                                        home.HomeMessageDialogue.Show(".......User has been updated.......");
                                        oldpassTxt.Clear();
                                        newpassTxt.Clear();
                                        renewpassTxt.Clear();
                                    }
                                    else
                                    {
                                        home.HomeMessageDialogue.Show("Incorrect password, please try again!");

                                    }
                                }
                                else
                                {
                                    home.HomeMessageDialogue.Show("Invalid username and password, Please try again!");

                               }
                            
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                home.HomeMessageDialogue.Show(ex.Message);
            }
        }

        private void cancelbtn2_Click(object sender, EventArgs e)
        {            
            oldpassTxt.Clear();
            newpassTxt.Clear();
            renewpassTxt.Clear();
        }

        public void LoadUserData()
        {   dgvUsers.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {   int i =0;
                con.Open();                
                string query = "SELECT * from dtUsers";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    dgvUsers.Rows.Add(i, reader["username"].ToString(), reader["fullname"].ToString(), reader["active"].ToString(), reader["role"].ToString());
                }
                reader.Close();

            }
        }

        private void resetbtn_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(user))
            {
             home.HomeMessageDialogue.Show("Please select an user first from the datagridview");

            }
            else
            {
                ResetPass reset = new ResetPass(this);
                reset.Show();
            }
           
        }

        private void propertybtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(user))
            {
               home.HomeMessageDialogue.Show("Please select an user first from the datagridview");

            }
            else
            {
               PropertyForm property = new PropertyForm(this);
                property.Show();

            }
        }

        private void dgvUsers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i = dgvUsers.CurrentRow.Index;
            user = dgvUsers[1, i].Value.ToString();
            name = dgvUsers[2, i].Value.ToString();
            status = dgvUsers[3, i].Value.ToString();
            role = dgvUsers[4, i].Value.ToString();

            if (user == Users.Uname)
            {
                removebtn.Enabled = false;
                resetbtn.Enabled = false;
            }
            else
            {
                removebtn.Enabled = true;
                resetbtn.Enabled = true;
            }
            guna2GroupBox1.Text = "Password for " + user;
            textlbl.Text = "To change the password of " + user + ", click  Reset Password";
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2TabControl1.SelectedIndex == 1 || guna2TabControl1.SelectedIndex == 2)
            {
                dgvUsers.Rows.Clear();
            }
            if (guna2TabControl1.SelectedIndex == 2)
            {
                LoadUserData();
            }
        }

        private void removebtn_Click(object sender, EventArgs e)
        {
            if (home.HomeYesNoDialogue.Show("Are you sure you want to delete this user?") == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(Users.connection)) 
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from dtUsers where username = @user", con);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.ExecuteNonQuery();
                    dgvUsers.Rows.Clear();
                    LoadUserData();                    
                }
            }
        }

        


    }
}
