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
    public partial class StockInModule : Form
    {
        StockEntry stock;
        public StockInModule(StockEntry stock)
        {
            InitializeComponent();
            LoadData();
            this.stock = stock;
        }

        public void LoadData()
        {

            dgvProduct.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                int i = 0;
                con.Open();
                SqlCommand cmd = new SqlCommand("select pcode, pdesc from dtProduct where pdesc LIKE '%" + searchbox.Text + "%'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    dgvProduct.Rows.Add(i, reader[0].ToString(), reader[1].ToString());
                }
                reader.Close();
            }
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colname == "Add")
            {
                Stockqty stockqty = new Stockqty(stock, dgvProduct[1, e.RowIndex].Value.ToString(), dgvProduct[2, e.RowIndex].Value.ToString());
                stockqty.Show();
            }
            
        }

        private void searchbox_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
