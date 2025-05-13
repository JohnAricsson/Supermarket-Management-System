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
    public partial class Qty : Form
    {
        private string pcode;
        private double price;
        private string transno;
        private int qty;        
        CashierHome cashier;
        LookProduct lookProduct;
        public Qty(CashierHome cashier, LookProduct lookprodcut)
        {
            InitializeComponent();
            this.cashier = cashier;
            this.lookProduct = lookprodcut;
        }
        
        public void ProductDetails(string pcode,double price,string transno, int qty)
        {   
            this.pcode = pcode;
            this.price = price;
            this.transno = transno;
            this.qty = qty;//quantity in product table
        }
        
        private void qtyTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13 && qtyTxt.Text!= string.Empty)
            {

                using (SqlConnection con = new SqlConnection(Users.connection))
                {
                    if (Convert.ToInt32(qtyTxt.Text) <= qty)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into dtCart(transno,pcode,price,quantity,sdate,cashier) values(@transno,@pcode,@price,@qty,@sdate,@cashier)", con);
                        cmd.Parameters.AddWithValue("@transno", transno);
                        cmd.Parameters.AddWithValue("@pcode", pcode);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@qty", qtyTxt.Text);
                        cmd.Parameters.AddWithValue("@sdate", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@cashier", Users.Uname);
                        cmd.ExecuteNonQuery();
                        cashier.LoadCart();
                        
                        using (SqlCommand cmdQ = new SqlCommand("UPDATE dtProduct SET quantity = quantity-@qty WHERE pcode = @pcode", con))
                        {
                            cmdQ.Parameters.AddWithValue("@qty", qtyTxt.Text);
                            cmdQ.Parameters.AddWithValue("@pcode", pcode);
                            cmdQ.ExecuteNonQuery();
                            if (CashierHome.CartItems.ContainsKey(pcode))
                            {
                                CashierHome.CartItems[pcode] += Convert.ToInt32(qtyTxt.Text);
                            }
                            else
                            {
                                CashierHome.CartItems[pcode] = Convert.ToInt32(qtyTxt.Text);
                            }
                        }
                        lookProduct.LoadData();
                        this.Dispose();
                    }
                    else
                    {
                        guna2MessageDialog1.Show("Not enough available stocks, Please choose a smaller quantity!");
                    }
                    
                }
            }
        }
    }
}
