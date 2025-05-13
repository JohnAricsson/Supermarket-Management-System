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
    public partial class Supplier : Form
    {
        SupplierModule supplier;
        public Supplier()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {

            dgvSupplier.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from dtSupplier where supplier LIKE '%"+searchbox.Text+"%'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {                    
                    dgvSupplier.Rows.Add(reader["id"].ToString(), reader["supplier"].ToString(), reader["address"].ToString(), reader["productdetails"].ToString(), reader["contact"].ToString(), reader["email"].ToString());
                }
                reader.Close();
            }
        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            supplier = new SupplierModule(this);
            supplier.Show();
        }

        private void dgvSupplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvSupplier.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {

                DialogResult result = MessageBox.Show("Are you sure?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(Users.connection))
                    {
                        int id = Convert.ToInt32(dgvSupplier[0, e.RowIndex].Value);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from dtSupplier where id=@id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        LoadData();
                    }
                }
            }
            else if (colname == "Edit")
            {
                supplier = new SupplierModule(this);
                supplier.unlockUpdate(Convert.ToInt32(dgvSupplier[0, e.RowIndex].Value),dgvSupplier[1, e.RowIndex].Value.ToString(), dgvSupplier[2, e.RowIndex].Value.ToString(), dgvSupplier[3, e.RowIndex].Value.ToString(), dgvSupplier[4, e.RowIndex].Value.ToString(), dgvSupplier[5, e.RowIndex].Value.ToString());
                supplier.Show();
            }
        }

        private void searchbox_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
