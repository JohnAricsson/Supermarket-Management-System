using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;

namespace Supermarket_management_system
{
    public partial class ProductModule : Form
    {

        private Product product;
        private string str;
        public ProductModule(Product product)
        {
            InitializeComponent();
            LoadBrandData();
            LoadCategoryData();
            this.product = product;
            Updatebtn.Enabled = false;
        }
        
        private void LoadBrandData()
        {
            string query = "SELECT * FROM dtBrand";

            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                brandCombo.DataSource = dataTable;
                brandCombo.DisplayMember = "brand";
                brandCombo.ValueMember = "id";
            }
        }
        private void LoadCategoryData()
        {
            string query = "SELECT * FROM dtCategory";

            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                CatCombo.DataSource = dataTable;
                CatCombo.DisplayMember = "category";
                CatCombo.ValueMember = "id";
            }
        }
        public void unlockUpdate(string Pcode,string Barcode, string Pdesc, string Bid, string Cid,string Price, string Quantity)
        {
            str = Pcode;
            Savebtn.Enabled = false;
            Updatebtn.Enabled = true;
            ProductTxt.Text = Pcode;
            BarcodeTxt.Text = Barcode;
            DescripTxt.Text = Pdesc;
            brandCombo.Text = Bid;
            CatCombo.Text = Cid;
            PriceTxt.Text = Price;
                      
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(ProductTxt.Text) ||
                    String.IsNullOrEmpty(BarcodeTxt.Text) ||
                    String.IsNullOrEmpty(DescripTxt.Text) ||
                    String.IsNullOrEmpty(PriceTxt.Text))
                {
                    guna2MessageDialog2.Show("Please fill up all information");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to add this brand?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();

                            string qry = "SELECT * FROM dtProduct WHERE pcode = @pcode OR barcode = @barcode";
                            using (SqlCommand cmdS = new SqlCommand(qry, con))
                            {
                                cmdS.Parameters.AddWithValue("@barcode", BarcodeTxt.Text);
                                cmdS.Parameters.AddWithValue("@pcode", ProductTxt.Text);

                                using (SqlDataReader reader = cmdS.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        guna2MessageDialog2.Show("Product already exists with the provided product code/barcode!");
                                        return; 
                                    }
                                }
                            }

                            
                            string query = "INSERT INTO dtProduct(pcode, barcode, pdesc, bid, cid, price) VALUES(@Pcode, @Barcode, @Pdesc, @Bid, @Cid, @Price)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@Pcode", ProductTxt.Text);
                                cmd.Parameters.AddWithValue("@Barcode", BarcodeTxt.Text);
                                cmd.Parameters.AddWithValue("@Pdesc", DescripTxt.Text);
                                cmd.Parameters.AddWithValue("@Bid", brandCombo.SelectedValue);
                                cmd.Parameters.AddWithValue("@Cid", CatCombo.SelectedValue);
                                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTxt.Text));

                                cmd.ExecuteNonQuery(); 
                            }
                                                       
                            ProductTxt.Clear();
                            BarcodeTxt.Clear();
                            DescripTxt.Clear();
                            brandCombo.SelectedIndex = 0;
                            CatCombo.SelectedIndex = 0;
                            PriceTxt.Clear();
                            product.LoadData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Show any exceptions that occur
                guna2MessageDialog2.Show(ex.Message);
            }
        }


        private void Clearbtn_Click(object sender, EventArgs e)
        {
            ProductTxt.Clear();
            BarcodeTxt.Clear();
            DescripTxt.Clear();
            PriceTxt.Clear();
            brandCombo.SelectedIndex = 0;
            CatCombo.SelectedIndex = 0;            
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(ProductTxt.Text) ||
                    String.IsNullOrEmpty(BarcodeTxt.Text) ||
                    String.IsNullOrEmpty(DescripTxt.Text) ||
                    String.IsNullOrEmpty(PriceTxt.Text))
                {
                    guna2MessageDialog2.Show("Please fill up all information");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to update this brand?");
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(Users.connection))
                        {
                            con.Open();
                            string query = "update dtProduct set pcode=@Pcode, barcode = @Barcode,pdesc = @Pdesc, bid=@Bid,cid=@Cid, price=@Price where pcode = @id ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@Pcode", ProductTxt.Text);
                            cmd.Parameters.AddWithValue("@Barcode", BarcodeTxt.Text);
                            cmd.Parameters.AddWithValue("@Pdesc", DescripTxt.Text);
                            cmd.Parameters.AddWithValue("@Bid", brandCombo.SelectedValue);
                            cmd.Parameters.AddWithValue("@Cid", CatCombo.SelectedValue);
                            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTxt.Text));                            
                            cmd.Parameters.AddWithValue("@id",str);
                            cmd.ExecuteNonQuery();

                            product.LoadData();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                guna2MessageDialog2.Show(ex.Message);
            }
        }
    }
}
