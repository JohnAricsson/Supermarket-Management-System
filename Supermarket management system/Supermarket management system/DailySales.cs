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
    public partial class DailySales : Form
    {
        private int i;
        public DailySales()
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
            dgvSold.Rows.Clear();
            i = 0;
            string query = "SELECT c.transno, c.pcode, p.pdesc, c.price, c.quantity, c.discperc, c.svat " +
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
                    // Add parameters safely
                    cmd.Parameters.AddWithValue("@fromdate", fromdatetime.Value.Date);
                    cmd.Parameters.AddWithValue("@todate", todatetime.Value.Date);
                    cmd.Parameters.AddWithValue("@cashier", userCombo.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i++;
                            dgvSold.Rows.Add(i,
                                reader["transno"].ToString(),
                                reader["pcode"].ToString(),
                                reader["pdesc"].ToString(),
                                reader["price"].ToString(),
                                reader["quantity"].ToString(),
                                reader["discperc"].ToString(),
                                reader["svat"].ToString());
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
    }
}
