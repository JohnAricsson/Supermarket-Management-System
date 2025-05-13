using System;
using System.Collections;
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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadSales();
            LoadProduct();
            LoadStock();
            LoadCashierSales();
        }

        public void LoadSales()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Users.connection))
            {
                DateTime date = DateTime.Now.Date;
                string query = "SELECT * FROM dtCart WHERE CONVERT(date, sdate) = @sdate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sdate", date);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            saleslbl.Text = dt.Rows.Count.ToString(); 
        }
        public void LoadProduct()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Users.connection))
            {                
                string query = "SELECT * FROM dtProduct";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {                    
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            productLbl.Text = dt.Rows.Count.ToString();
        }
        public void LoadStock()
        {
            int stock = 0;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Users.connection))
            {   conn.Open();
                string query = "SELECT quantity FROM dtProduct";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stock += Convert.ToInt32(reader["quantity"]);
                }
                reader.Close();
                
            }
            stocklbl.Text = stock.ToString();
        }

        public void LoadCashierSales()
        {
            using (SqlConnection conn = new SqlConnection(Users.connection))
            {
                string query = "SELECT cashier, COUNT(*) AS total_sales FROM dtCart where status = 'Paid' GROUP BY cashier";
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cashier = reader["cashier"].ToString();
                        int salesCount = Convert.ToInt32(reader["total_sales"]);
                        chart.Series["cashier"].Points.AddXY(cashier,salesCount);
                    }
                }
            }
        }


    }
}
