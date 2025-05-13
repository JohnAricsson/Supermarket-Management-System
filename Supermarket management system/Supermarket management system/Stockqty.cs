using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    public partial class Stockqty : Form
    {
        private string pcode,pdesc;
        private int qty;
        
        StockEntry stock;
        public Stockqty(StockEntry stock,string pcode,string pdesc )
        {
            InitializeComponent();
            this.stock = stock;
            this.pcode = pcode;
            this.pdesc = pdesc;
            
        }

        private void stockqtyTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && stockqtyTxt.Text != string.Empty)
            {
                qty = Convert.ToInt32(stockqtyTxt.Text);
                stock.LoadData(pcode, pdesc, qty);
                this.Dispose();
            }
        }
    }
}
