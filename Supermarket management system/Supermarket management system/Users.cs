using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace Supermarket_management_system
{
    internal static class Users
    {
        public static readonly string connection = "Data Source=DESKTOP-MK8RRL3\\SQLEXPRESS03;Initial Catalog=SupermarketManagement;Integrated Security=True;Encrypt=False";
        private static SqlConnection con = new SqlConnection(connection);

        private static string role;
        private static string uname;
        private static string status;
        public static string Role
        {
            get { return role; }
            set { role = value; }
        }
        public static string Uname
        {
            get { return uname; }
            set { uname = value; }
        }
        public static string Status
        {
            get { return status; }
            set { status = value; }
        }
        public static bool IsValid(string username, string password)
        {
            string query = "SELECT username, role, active FROM dtUsers WHERE username = @user AND password = @pass";

            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password); 
                    
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Uname = reader["username"].ToString();
                            Role = reader["role"].ToString();
                            Status = reader["active"].ToString();
                            return true;
                        }
                    }
                }
            }

            return false; 
        }




    }
}
