using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    public partial class StockEntry : Form
    {
        HomePage home;
        private string pcode, pdesc;
        private int qty,i=0;
        public StockEntry(HomePage home)
        {
            InitializeComponent();
            LoadSupplierData();
            this.home = home;
        }

        private void LoadSupplierData()
        {
            string query = "SELECT supplier FROM dtSupplier";

            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                supCombo.DataSource = dataTable;
                supCombo.DisplayMember = "supplier";
                supCombo.ValueMember = "supplier";
                
            }
        }

        private void refgen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string stime = DateTime.Now.ToString("hhmmss");
            string refNo = stime + 10001;
            refTxt.Text = refNo;
        }

        private void productLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (refTxt.Text == string.Empty || stockbyTxt.Text == string.Empty)
            {
                home.HomeMessageDialogue.Show("Please generate a ref no and enter all information before browsing products.");
            }
            else
            {
                StockInModule stockInModule = new StockInModule(this);
                stockInModule.Show();
            }
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {
                DialogResult result = home.HomeYesNoDialogue.Show("Are you sure you want to delete this product?");
                if (result == DialogResult.Yes)
                {
                    dgvProduct.Rows.RemoveAt(e.RowIndex);
                }
            }
               
        }

        public void LoadData(string pcode,string pdesc,int qty)
        {
            i++;
           this.pcode = pcode;
           this.pdesc = pdesc;
           this.qty = qty;
           string refno = refTxt.Text;
            string stockby = stockbyTxt.Text;
            string sup = supCombo.SelectedValue.ToString();
            DateTime date = datetime.Value;
            dgvProduct.Rows.Add(i,refno, pcode, pdesc, qty, date, stockby, sup);
            dgvProduct.Refresh();
        }

        private void entrybtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                foreach(DataGridViewRow row in dgvProduct.Rows)
                {
                    string refnum = row.Cells[1].Value.ToString();
                    string Pcode = row.Cells[2].Value.ToString();
                    string Pdesc = row.Cells[3].Value.ToString();
                    int Qty = Convert.ToInt32(row.Cells[4].Value);
                    DateTime Date = Convert.ToDateTime(row.Cells[5].Value);
                    string By = row.Cells[6].Value.ToString();
                    string Sup = row.Cells[7].Value.ToString();

                    SqlCommand cmd = new SqlCommand("Insert into dtStockIn(refno,pcode,productdesc,quantity,stockdate,stockinby,supplier) values(@refno,@pcode,@pdesc,@qty,@sdate,@sby,@sup)", con);
                    cmd.Parameters.AddWithValue("@refno", refnum);
                    cmd.Parameters.AddWithValue("@pcode", Pcode);
                    cmd.Parameters.AddWithValue("@pdesc", Pdesc);
                    cmd.Parameters.AddWithValue("@qty", Qty);
                    cmd.Parameters.AddWithValue("@sdate", Date);
                    cmd.Parameters.AddWithValue("@sby", By);
                    cmd.Parameters.AddWithValue("@sup", Sup);
                    cmd.ExecuteNonQuery();

                    int iniqty=0;
                    SqlCommand cmdS = new SqlCommand("Select quantity from dtProduct where pcode = @pcode", con);
                    cmdS.Parameters.AddWithValue("@pcode", Pcode);
                    SqlDataReader reader = cmdS.ExecuteReader();
                    if (reader.Read())
                    {
                        iniqty = Convert.ToInt32(reader["quantity"].ToString());
                    }
                    reader.Close();

                    int sum = Qty + iniqty;
                    SqlCommand cmdQ = new SqlCommand("update dtProduct set quantity = @qty where pcode = @pcode", con);
                    cmdQ.Parameters.AddWithValue("@qty", sum);
                    cmdQ.Parameters.AddWithValue("@pcode", Pcode);
                    cmdQ.ExecuteNonQuery();

                }
                dgvProduct.Rows.Clear();               

            }
        }
    }
}
