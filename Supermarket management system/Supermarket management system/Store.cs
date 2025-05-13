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
    public partial class Store : Form
    {
        public Store()
        {
            InitializeComponent();
        }

        

        

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            storeTxt.Clear();
            addressTxt.Clear();
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(storeTxt.Text)||
                        String.IsNullOrEmpty(addressTxt.Text))
                {
                    guna2MessageDialog2.Show("Please enter all information!");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to add this store?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "Insert into dtStore(store,address) values(@store,@address)";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@store", storeTxt.Text);
                            cmd.Parameters.AddWithValue("@address", addressTxt.Text);
                            cmd.ExecuteNonQuery();
                            storeTxt.Clear();
                            addressTxt.Clear();
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
