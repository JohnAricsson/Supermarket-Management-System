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
    public partial class SupplierModule : Form
    {
        private Supplier supplier;
        private int id;
        public SupplierModule(Supplier supplier)
        {
            InitializeComponent();
            this.supplier = supplier;
            Updatebtn.Enabled = false;
        }

        public void unlockUpdate(int id,string sup, string add, string pro, string con, string email)
        {            
            Savebtn.Enabled = false;
            Updatebtn.Enabled = true;
            this.id = id;
            SupplierTxt.Text = sup;
            AddressTxt.Text = add;
            productTxt.Text = pro;
            ContactTxt.Text = con;
            emailTxt.Text = email;            
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(SupplierTxt.Text) ||
                    String.IsNullOrEmpty(AddressTxt.Text) ||
                    String.IsNullOrEmpty(productTxt.Text) ||
                    String.IsNullOrEmpty(ContactTxt.Text)||
                    String.IsNullOrEmpty(emailTxt.Text))
                {
                    guna2MessageDialog2.Show("Please fill up all information");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to add this brand?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "Insert into dtSupplier(supplier,address,productdetails,contact,email) values(@sup,@add,@pro,@con,@email)";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@sup", SupplierTxt.Text);
                            cmd.Parameters.AddWithValue("@add", AddressTxt.Text);
                            cmd.Parameters.AddWithValue("@pro", productTxt.Text);
                            cmd.Parameters.AddWithValue("@con", ContactTxt.Text);
                            cmd.Parameters.AddWithValue("@email", emailTxt.Text);                            
                            cmd.ExecuteNonQuery();
                            SupplierTxt.Clear();
                            AddressTxt.Clear();
                            productTxt.Clear();
                            ContactTxt.Clear();
                            emailTxt.Clear();
                            supplier.LoadData();
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
                if (String.IsNullOrEmpty(SupplierTxt.Text) ||
                    String.IsNullOrEmpty(AddressTxt.Text) ||
                    String.IsNullOrEmpty(productTxt.Text) ||
                    String.IsNullOrEmpty(ContactTxt.Text) ||
                    String.IsNullOrEmpty(emailTxt.Text))
                {
                    guna2MessageDialog2.Show("Please fill up all information");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to update this brand?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "update dtSupplier set supplier=@sup, address = @add,productdetails = @pro, contact=@con,email=@email where id = @id ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@sup", SupplierTxt.Text);
                            cmd.Parameters.AddWithValue("@add", AddressTxt.Text);
                            cmd.Parameters.AddWithValue("@pro", productTxt.Text);
                            cmd.Parameters.AddWithValue("@con", ContactTxt.Text);
                            cmd.Parameters.AddWithValue("@email", emailTxt.Text);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                            supplier.LoadData();
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
            SupplierTxt.Clear();
            AddressTxt.Clear();
            productTxt.Clear();
            ContactTxt.Clear();
            emailTxt.Clear();
        }
    }
}
