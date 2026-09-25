using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class Databasecs
    {
        public class Database
        {
            private string connectionString =
                @"Data Source=.;Initial Catalog=HotelManagement;Integrated Security=True;TrustServerCertificate=True";

            public SqlConnection GetConnection()
            {
                return new SqlConnection(connectionString);
            }

            public DataTable ExecuteQuery(string query)
            {
                SqlConnection conn = GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            public void ExecuteNonQuery(string query)
            {
                SqlConnection conn = GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
