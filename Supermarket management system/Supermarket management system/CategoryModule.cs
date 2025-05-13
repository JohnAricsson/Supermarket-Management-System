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
    public partial class CategoryModule : Form
    {
        Category cat;
        private int id;
        public CategoryModule(Category cat)
        {
            InitializeComponent();
            this.cat = cat;
            Updatebtn.Enabled = false;
        }
        public void unlockUpdate(string str, int id)
        {
            Savebtn.Enabled = false;
            Updatebtn.Enabled = true;
            Cattxt.Text = str;
            this.id = id;
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Cattxt.Text))
                {
                    guna2MessageDialog2.Show("Please enter category name!");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to add this category?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "Insert into dtCategory(category) values(@cat)";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@cat", Cattxt.Text);
                            cmd.ExecuteNonQuery();
                            Cattxt.Text = "";
                            cat.LoadData();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                guna2MessageDialog2.Show(ex.Message);
            }
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Cattxt.Text))
                {
                    guna2MessageDialog2.Show("Please enter brand name!");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to update this brand?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "update dtCategory set category=@cat where id = @id ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@cat", Cattxt.Text);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();

                            cat.LoadData();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                guna2MessageDialog2.Show(ex.Message);
            }
        }

        private void Clearbtn_Click(object sender, EventArgs e)
        {
            Cattxt.Text = "";
        }
    }
}
