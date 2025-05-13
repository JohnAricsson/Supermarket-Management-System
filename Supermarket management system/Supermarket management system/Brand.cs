using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    public partial class Brand : Form
    {
        HomePage home;
        BrandModule brand;
        
        public Brand(HomePage home)
        {
            InitializeComponent();
            LoadData();
            this.home = home;
        }
       public void LoadData()
        {
            
            dgvBrand.Rows.Clear();
            using(SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();                
                SqlCommand cmd = new SqlCommand("select * from dtBrand", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {                   
                  dgvBrand.Rows.Add(reader["id"].ToString(), reader["brand"].ToString());
                }
                reader.Close();
            }
        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            brand = new BrandModule(this);
            brand.Show();
        }

        private void dgvBrand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvBrand.Columns[e.ColumnIndex].Name;
            if(colname == "Delete")
            {

                DialogResult result = home.HomeYesNoDialogue.Show("Are you sure you want to delete this product?");
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(Users.connection))
                    {
                        int id = Convert.ToInt32(dgvBrand[0, e.RowIndex].Value);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from dtBrand where id=@id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        LoadData();
                    }
                }
            }
            else if(colname == "Edit")
            {
                brand = new BrandModule(this);
                brand.unlockUpdate(dgvBrand[1, e.RowIndex].Value.ToString(),Convert.ToInt32(dgvBrand[0, e.RowIndex].Value));
                brand.Show();        
            }

        }
    }
}
