using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Supermarket_management_system
{
    public partial class CashierHome : Form
    {
        public static bool Paid {  get; set; }
        public static Dictionary<string, int> CartItems = new Dictionary<string, int>();
        private static CashierHome _instance;
        private double total = 0;
        private int i;
        public int I
        {
            set { i = value; }
        }
        public static CashierHome Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CashierHome();
                }
                return _instance;
            }
        }
        public CashierHome()
        {            
            InitializeComponent();
            _instance = this;
            timer1.Start();
            i = 0;
            cashiername.Text = Users.Uname.ToUpper();
            Paid = false;
        }
        
        public void LoadCart()
        {
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                

                dgvProduct.Rows.Clear();
                SqlCommand cmd = new SqlCommand("select c.id,c.pcode,p.pdesc,c.price,c.quantity,c.disc,c.total from dtCart as c inner join dtProduct as p on c.pcode=p.pcode where c.transno like @transno and c.status like 'Pending'", con);
                cmd.Parameters.AddWithValue("@transno", transNolbl.Text);                
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    total += Convert.ToDouble(reader["total"]);
                    
                    dgvProduct.Rows.Add(reader["id"].ToString(), reader["pcode"].ToString(), reader["pdesc"].ToString(), reader["price"].ToString(), reader["quantity"].ToString(), reader["total"].ToString());

                }
                saleslbl.Text = total.ToString();                
                totalLbl.Text = total.ToString();
                GetCartTotal();
            }
        }


        public void GetCartTotal()
        {
            double discount = Convert.ToDouble(discountlbl.Text);
            double sales = Convert.ToDouble(saleslbl.Text)-discount;
            double vat = sales * 0.05;
            double payable = (sales + vat);
            vatlbl.Text = vat.ToString();
            paylbl.Text = payable.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerlbl.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        public void getTransNo()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");
            string stime = DateTime.Now.ToString("hhmmss");
            string transNo = sdate + stime;
            transNolbl.Text = transNo;
        }
        
        private void newTrans_Click(object sender, EventArgs e)
        {
            if (i == 0) { 
                getTransNo();
                i++;
            }
            
            else
            {
                HomeMessageDialogue.Show("You have already generated a transaction number, Please clear the cart");
            }

        }
        private void RestoreStock()
        {
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open(); 

                foreach (KeyValuePair<string, int> item in CartItems)
                {
                    string pcode = item.Key;
                    int qty = item.Value;

                    if (string.IsNullOrWhiteSpace(pcode)) continue;

                    using (SqlCommand cmdQ = new SqlCommand("UPDATE dtProduct SET quantity = quantity + @qty WHERE pcode = @pcode", con))
                    {
                        cmdQ.Parameters.AddWithValue("@qty", qty);
                        cmdQ.Parameters.AddWithValue("@pcode", pcode);
                        cmdQ.ExecuteNonQuery();
                    }
                }
            }
        }
        private void searchpro_Click(object sender, EventArgs e)
        {
            
            if (transNolbl.Text == "000000000")
            {
                HomeMessageDialogue.Show("Please generate a transaction number first!");
            }
            else
            {
                LookProduct lookUp = new LookProduct(this);
                lookUp.LoadData();
                lookUp.Show();
            }
            
        }
        public void discount_update(string Disamount,string Discperc)
        {
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                con.Open();
                string query = "Update dtCart set disc = @disc , discperc=@perc where transno=@transno";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@disc", Convert.ToDecimal(Disamount));
                cmd.Parameters.AddWithValue("@perc",Convert.ToDecimal( Discperc));
                cmd.Parameters.AddWithValue("@transno", transNolbl.Text);
                cmd.ExecuteNonQuery();
                discountlbl.Text = Disamount;
                GetCartTotal();               
                
            }
        }
        
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            if (CartItems.Count > 0 && Paid == false)
            {
                RestoreStock();
                CartItems.Clear();
                Paid = false;
                clearCart();
            }
            Application.Exit();
        }

        private void discount_Click(object sender, EventArgs e)
        {
            Discount discount = new Discount(this);
            discount.totalTxt.Text = totalLbl.Text;
            discount.Show();

        }

        private void payment_Click(object sender, EventArgs e)
        {           
            Payment payment  = new Payment(this);
            payment.Show();
        }
        public void clrcart()
        {
            dgvProduct.Rows.Clear();
            transNolbl.Text = "000000000";
            totalLbl.Text = "0.00";
            saleslbl.Text = "0.00";
            discountlbl.Text = "0.00";
            vatlbl.Text = "0.00";
            paylbl.Text = "0.00";
        }
        public void clearCart()
        {
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                discount.Enabled = true;
                if (transNolbl.Text != "000000000"||
                    saleslbl.Text != "0.00"||discountlbl.Text!="0.00"||vatlbl.Text!="0.00"||paylbl.Text!="0.00")
                {
                    transNolbl.Text = "000000000";
                    saleslbl.Text = "0.00";
                    discountlbl.Text = "0.00";
                    vatlbl.Text = "0.00";
                    paylbl.Text = "0.00";
                    i = 0;

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from dtCart where transno=@transno and status =@status", con);
                    cmd.Parameters.AddWithValue("@transno", transNolbl.Text);
                    cmd.Parameters.AddWithValue("@status", "Pending");
                    cmd.ExecuteNonQuery();
                    dgvProduct.Rows.Clear();     
                    
                }                
                
            }
        }
        private void clearcart_Click(object sender, EventArgs e)
        {
            if (CartItems.Count > 0 && Paid == false)
            {
                RestoreStock();
                CartItems.Clear();
                Paid = false;
                clearCart();
                
            }
            else if (dgvProduct.Rows.Count == 0)
            {
                HomeMessageDialogue.Show("You have not added any items in the cart!");
                i = 0;
            }
        }

        private void dailysales_Click(object sender, EventArgs e)
        {
            DailySales dailySales = new DailySales();
            dailySales.Show();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Dispose();

        }

        private void changepass_Click(object sender, EventArgs e)
        {
            ChangePass changePass = new ChangePass();
            changePass.Show();
        }
    }
}
