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
    public partial class StockAdj : Form
    {
        HomePage home;
        private int quantity;
        public StockAdj(HomePage page)
        {
            InitializeComponent();
            this.home = page;
            quantity = 0;
            LoadData();
        }
        public void LoadData()
        {

            dgvProduct.Rows.Clear();
            using (SqlConnection con = new SqlConnection(Users.connection))
            {
                int i = 0;
                con.Open();
                SqlCommand cmd = new SqlCommand("select p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.quantity from dtProduct as p Inner join dtBrand as b on b.id=p.bid inner join dtCategory as c on c.id=p.cid where concat(p.pdesc,b.brand,c.category) LIKE '%" + searchbox.Text + "%'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    dgvProduct.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());
                }
                reader.Close();
            }
        }
        public void getRefNo()
        {            
            string stime = DateTime.Now.ToString("hhmmss");
            int refno = Convert.ToInt32(stime) + 10001;
            refnoTxt.Text = refno.ToString();
        }
        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvProduct.Columns[e.ColumnIndex].Name;
            if(colname == "Edit")
            {
                refnoTxt.Clear();
                getRefNo();
                pcodeTxt.Clear();
                pcodeTxt.Text = dgvProduct[1, e.RowIndex].Value.ToString();
                quantity = Convert.ToInt32(dgvProduct[7, e.RowIndex].Value);
            }
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(actionCombo.Text) || String.IsNullOrEmpty(refnoTxt.Text) || String.IsNullOrEmpty(pcodeTxt.Text) || String.IsNullOrEmpty(quantityTxt.Text))
            {
                home.HomeMessageDialogue.Show("Please fill all information");
            }
            else if (quantityTxt.Text=="0")
            {
                home.HomeMessageDialogue.Show("Quantity must be a proper value!");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Users.connection))
                {
                    int qty = 0;
                    con.Open();                    
                    if (actionCombo.Text == "Remove it from Inventory")
                    {
                        qty = quantity - Convert.ToInt32(quantityTxt.Text);
                        if(qty > 0)
                        {
                            string query = "update dtProduct set quantity=@quantity where pcode = @pcode ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@quantity", qty);
                            cmd.Parameters.AddWithValue("@pcode", pcodeTxt.Text);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            home.HomeMessageDialogue.Show("Quantity exceeds the available stocks!");
                        }                        
                    }
                    if(actionCombo.Text == "Add to Inventory")
                    {
                            qty = quantity + Convert.ToInt32(quantityTxt.Text);                        
                            string query = "update dtProduct set quantity=@quantity where pcode = @pcode ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@quantity", qty);
                            cmd.Parameters.AddWithValue("@pcode", pcodeTxt.Text);
                            cmd.ExecuteNonQuery();                        
                    }
                    
                    string queryS = "insert into dtStockAdj(refno,pcode,action,quantity,remark) values(@refno,@pcode,@action,@quantity,@remark)";
                    SqlCommand cmdS = new SqlCommand(queryS, con);
                    cmdS.Parameters.AddWithValue("@refno", refnoTxt.Text);
                    cmdS.Parameters.AddWithValue("@pcode", pcodeTxt.Text);
                    cmdS.Parameters.AddWithValue("@action", actionCombo.Text);
                    cmdS.Parameters.AddWithValue("@quantity", Convert.ToInt32(quantityTxt.Text));
                    cmdS.Parameters.AddWithValue("@remark", remarkTxt.Text);
                    cmdS.ExecuteNonQuery();
                    refnoTxt.Text = "";
                    pcodeTxt.Text = "";
                    remarkTxt.Clear();
                    quantityTxt.Clear();
                    actionCombo.Text = "";
                    LoadData();
                    
                }
            }
        }

        private void searchbox_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
