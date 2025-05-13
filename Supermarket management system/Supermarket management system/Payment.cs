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
    public partial class Payment : Form
    {
        CashierHome cashier;
        public Payment(CashierHome cashier)
        {
            InitializeComponent();
            this.cashier = cashier;
            totalTxt.Text = cashier.paylbl.Text;
            remTxt.Text = "0.00";
            CashierHome.Paid = false;
        }

        private void onebtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "1";
        }

        private void twobtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "2";
        }

        private void threebtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "3";
        }

        private void fourbtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "4";
        }

        private void fivebtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "5";
        }

        private void sixbtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "6";
        }

        private void sevenbtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "7";
        }

        private void eigthbtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "8";
        }

        private void ninebtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "9";
        }

        private void zerobtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += "0";
        }

        private void dotbtn_Click(object sender, EventArgs e)
        {
            paidTxt.Text += ".";
        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            paidTxt.Clear();
            remTxt.Text = totalTxt.Text;
        }

        private void enterbtn_Click(object sender, EventArgs e)
        {
            try
            {
                double paid = Convert.ToDouble(paidTxt.Text);
                double total = Convert.ToDouble(totalTxt.Text);
                if (totalTxt.Text == "0.00")
                {
                    guna2MessageDialog1.Show("Please add some items in the cart!");
                }
                
                else if (paid >= total)
                {
                    CashierHome.Paid = true;
                    string transno = cashier.transNolbl.Text;
                    string stats = "Paid";
                    using (SqlConnection con = new SqlConnection(Users.connection))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Update dtCart set status = @stats where transno=@transno", con);
                        cmd.Parameters.AddWithValue("@stats", stats);
                        cmd.Parameters.AddWithValue("@transno", transno);
                        cmd.ExecuteNonQuery();                            
                                                    
                        cashier.I = 0;
                        cashier.clrcart();
                                                
                    }

                    this.Dispose();
                }
                else
                {
                    guna2MessageDialog1.Show("Must pay the due total!");
                }
            }
            catch (Exception)
            {
                guna2MessageDialog1.Show("An error occured please try again!");
                paidTxt.Clear();
                remTxt.Text = "0.00";
            }
        }

        private void paidTxt_TextChanged(object sender, EventArgs e)
        {
            double paidAmount = 0;
            double totalAmount = 0;
            
            bool isPaidValid = double.TryParse(paidTxt.Text, out paidAmount);
            bool isTotalValid = double.TryParse(totalTxt.Text, out totalAmount);

            
            if (isPaidValid && isTotalValid)
            {
                double rem = paidAmount - totalAmount;
                if (rem > 0)
                {
                    remTxt.Text = rem.ToString();
                }
                else
                {
                    remTxt.Text = "0"; 
                }
            }
        }
    }
}
