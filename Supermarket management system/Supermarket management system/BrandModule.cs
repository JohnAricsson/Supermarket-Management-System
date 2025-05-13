using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    public partial class BrandModule : Form
    {
        private Brand brand;
        private int id;
        public BrandModule(Brand br)
        {
            InitializeComponent();
            brand = br;
            Updatebtn.Enabled = false;
        }
        

        private void Clearbtn_Click(object sender, EventArgs e)
        {
            Brandtxt.Text = "";
        }
        public void unlockUpdate(string str,int id)
        {
            Savebtn.Enabled = false;
            Updatebtn.Enabled = true;
            Brandtxt.Text = str;
            this.id=id;
        }
        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Brandtxt.Text))
                {
                    guna2MessageDialog2.Show("Please enter brand name!");
                }                
                else {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to add this brand?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "Insert into dtBrand(brand) values(@brand)";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@brand", Brandtxt.Text);
                            cmd.ExecuteNonQuery();
                            Brandtxt.Text = "";
                            brand.LoadData();
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                guna2MessageDialog2.Show(ex.Message);
            }
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Brandtxt.Text))
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
                            string query = "update dtBrand set brand=@brand where id = @id ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@brand", Brandtxt.Text);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();

                            brand.LoadData();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                guna2MessageDialog2.Show(ex.Message);
            }

        }
    }
}
