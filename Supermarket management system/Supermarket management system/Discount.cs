using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    public partial class Discount : Form
    {
        
        private CashierHome cashier;        
        public static string Pcode;

        public Discount(CashierHome cashier)
        {
            InitializeComponent();           
            this.cashier = cashier;
        }

        private void discountTxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double disc = Convert.ToDouble(totalTxt.Text) * (Convert.ToDouble(discountTxt.Text)/100.0f);
                DisamountTxt.Text= disc.ToString();
            }
            catch (Exception)
            {
                DisamountTxt.Text = "0.00";
            }
        }

        private void confirmbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(discountTxt.Text) ) 
                    
                {
                    guna2MessageDialog2.Show("Please enter the discount percentage!");
                }
                else if(totalTxt.Text == "0.00")
                {
                    guna2MessageDialog2.Show("Please add some items in the cart!");
                }
                else
                {
                    DialogResult result = guna2MessageDialog1.Show("Are you sure you want to add this discount amount?");
                    if (result == DialogResult.Yes)
                    {
                        cashier.discount_update(DisamountTxt.Text,discountTxt.Text);
                        this.Dispose();
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
