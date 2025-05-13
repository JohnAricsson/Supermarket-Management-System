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
    public partial class LookProduct : Form
    {
        CashierHome cashier;
        
        public LookProduct(CashierHome cashier)
        {
            InitializeComponent();
            LoadData();
            this.cashier = cashier;
            
        }

        public void LoadData()
        {

            dgvProduct.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                int i = 0;
                con.Open();
                SqlCommand cmd = new SqlCommand("select p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.quantity from dtProduct as p Inner join dtBrand as b on b.id=p.bid inner join dtCategory as c on c.id=p.cid where concat(p.barcode,p.pdesc,b.brand,c.category) LIKE '%" + searchbox.Text + "%'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    dgvProduct.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());
                }

                reader.Close();
            }
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            
            if(colName == "Select")
            {                
                    Qty qty = new Qty(cashier,this);
                    qty.ProductDetails(dgvProduct[1, e.RowIndex].Value.ToString(), Convert.ToDouble(dgvProduct[6, e.RowIndex].Value), cashier.transNolbl.Text, Convert.ToInt32(dgvProduct[7, e.RowIndex].Value));
                    qty.Show();
                
            }
        }

        private void searchbox_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
