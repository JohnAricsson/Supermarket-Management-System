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
    public partial class Salehistory : Form
    {
        private int i;
        public Salehistory()
        {
            InitializeComponent();
            LoadUserData();
            i = 0;
        }
        private void LoadUserData()
        {
            string query = "SELECT username FROM dtUsers where role like 'Cashier' ";

            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                userCombo.DataSource = dataTable;
                userCombo.DisplayMember = "username";
                userCombo.SelectedIndex = -1;
            }
        }

        private void LoadSoldData()
        {
            dgvSales.Rows.Clear();
            i = 0;
            string query = "SELECT c.transno, c.pcode, p.pdesc, c.price, c.quantity, c.discperc, c.svat, c.sdate,c.cashier " +
                           "FROM dtCart AS c " +
                           "INNER JOIN dtProduct AS p ON c.pcode = p.pcode " +
                           "WHERE status = 'Paid' " +
                           "AND sdate BETWEEN @fromdate AND @todate " +
                           "AND cashier = @cashier";

            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    
                    cmd.Parameters.AddWithValue("@fromdate", fromdatetime.Value.Date);
                    cmd.Parameters.AddWithValue("@todate", todatetime.Value.Date);
                    cmd.Parameters.AddWithValue("@cashier", userCombo.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i++;
                            dgvSales.Rows.Add(i,
                                reader["transno"].ToString(),
                                reader["pcode"].ToString(),
                                reader["pdesc"].ToString(),
                                reader["price"].ToString(),
                                reader["quantity"].ToString(),
                                reader["discperc"].ToString(),
                                reader["svat"].ToString(),
                                Convert.ToDateTime(reader["sdate"]).ToString("yyyy-MM-dd"),
                                reader["cashier"].ToString());

                        }
                    }
                }
            }
        }

        private void fromdatetime_ValueChanged(object sender, EventArgs e)
        {
            LoadSoldData();
        }

        private void todatetime_ValueChanged(object sender, EventArgs e)
        {
            LoadSoldData();
        }

        private void userCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSoldData();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;  // Ensure the document is assigned
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            using (Bitmap bm = new Bitmap(dgvSales.Width, dgvSales.Height))
            {
                dgvSales.DrawToBitmap(bm, new Rectangle(0, 0, dgvSales.Width, dgvSales.Height));
                e.Graphics.DrawImage(bm, 0, 0);
            }
        }




    }

}
