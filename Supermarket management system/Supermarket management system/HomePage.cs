using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supermarket_management_system
{
    
    public partial class HomePage : Form
    {
        private bool paid = false;
        private static HomePage _instance;
        public static HomePage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HomePage();
                }
                return _instance;
            }
        }
        public HomePage()
        {
            InitializeComponent();
            _instance = this;
            USERNAME.Text = Users.Uname.ToUpper();
            AddControl(new Dashboard());
        }
        internal void AddControl(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            ProductSubPanel.Visible = false;
            StockSubPanel.Visible = false;            
            SettingsSubPanel.Visible = false;
        }

        private void HideSubMenu()
        {
            if (SettingsSubPanel.Visible == true) SettingsSubPanel.Visible = false;             
            if(ProductSubPanel.Visible == true) ProductSubPanel.Visible = false;
            if(StockSubPanel.Visible == true ) StockSubPanel.Visible = false;
        }

        private void Product_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ProductSubPanel.Visible = true;
        }

        private void Dashboard_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            AddControl(new Dashboard());
        }

        private void ProductList_Click(object sender, EventArgs e)
        {
            AddControl(new Product(this));
        }

        private void Category_Click(object sender, EventArgs e)
        {
            AddControl(new Category(this));
        }

        private void Brand_Click(object sender, EventArgs e)
        {
            AddControl(new Brand(this));
        }

        private void Instock_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            StockSubPanel.Visible=true;
        }

        private void StockEntry_Click(object sender, EventArgs e)
        {
            AddControl(new StockEntry(this));
        }

        private void StockAdj_Click(object sender, EventArgs e)
        {
            AddControl(new StockAdj(this));
        }       
                
        private void User_Click(object sender, EventArgs e)
        {
            AddControl(new UserAccount(this));
        }

        private void Store_Click(object sender, EventArgs e)
        {
            Store store = new Store();
            store.Show();
        }

        private void Record_Click(object sender, EventArgs e)
        {
            HideSubMenu();            
            AddControl(new Salehistory());
        }

       

        private void Logout_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            Login login = new Login();
            login.Show();
            this.Dispose();
        }

        private void SETTINGS_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            SettingsSubPanel.Visible = true;
        }

        private void Supplier_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            AddControl(new Supplier());
        }
    }
}
