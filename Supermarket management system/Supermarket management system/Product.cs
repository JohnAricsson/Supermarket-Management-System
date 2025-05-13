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
    
    public partial class Product : Form
    {
        HomePage home;
        ProductModule product;
        public Product(HomePage page)
        {
            InitializeComponent();
            LoadData();
            this.home = page;
        }
        public void LoadData()
        {

            dgvProduct.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                int i = 0;
                con.Open();
                SqlCommand cmd = new SqlCommand("select p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.quantity from dtProduct as p Inner join dtBrand as b on b.id=p.bid inner join dtCategory as c on c.id=p.cid where concat(p.pdesc,b.brand,c.category) LIKE '%"+searchbox.Text+"%'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    dgvProduct.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());
                }
                reader.Close();
            }
        }
        private void Addbtn_Click(object sender, EventArgs e)
        {
            product = new ProductModule(this);
            product.Show();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {

                DialogResult result = home.HomeYesNoDialogue.Show("Are you sure you want to delete this product?");
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(Users.connection))
                    {
                        int id = Convert.ToInt32(dgvProduct[1, e.RowIndex].Value);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from dtProduct where id=@id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        LoadData();
                    }
                }
            }
            else if (colname == "Edit")
            {
                product = new ProductModule(this);
                product.unlockUpdate(dgvProduct[1, e.RowIndex].Value.ToString(), dgvProduct[2, e.RowIndex].Value.ToString(), dgvProduct[3, e.RowIndex].Value.ToString(), dgvProduct[4, e.RowIndex].Value.ToString(), dgvProduct[5, e.RowIndex].Value.ToString(), dgvProduct[6, e.RowIndex].Value.ToString(), dgvProduct[7, e.RowIndex].Value.ToString());
                product.Show();
            }
        }

        private void searchbox_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
