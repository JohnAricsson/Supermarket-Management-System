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
    public partial class Category : Form
    {
        HomePage home;
        CategoryModule cat;
        public Category(HomePage home)
        {
            InitializeComponent();
            LoadData();
            this.home = home;
        }
        public void LoadData()
        {

            dgvCategory.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from dtCategory", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvCategory.Rows.Add(reader["id"].ToString(), reader["category"].ToString());
                }
                reader.Close();
            }
        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            cat = new CategoryModule(this);
            cat.Show();
        }

        private void dgvBrand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {

                DialogResult result = home.HomeYesNoDialogue.Show("Are you sure you want to delete this category?");
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(Users.connection))
                    {
                        int id = Convert.ToInt32(dgvCategory[0, e.RowIndex].Value);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from dtCategory where id=@id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        LoadData();
                    }
                }
            }
            else if (colname == "Edit")
            {
                cat = new CategoryModule(this);
                cat.unlockUpdate(dgvCategory[1, e.RowIndex].Value.ToString(), Convert.ToInt32(dgvCategory[0, e.RowIndex].Value));
                cat.Show();
            }
        }
    }
}
