using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLUtils
{
    public class SQLManager : IDisposable
    {
        private readonly SqlConnection connection;

        public SQLManager(string ip, string serverName,string username, string password)
        {
            string connetionString = $"Data Source = {ip}; Initial Catalog = {serverName}; User ID = {username}; Password = {password}";

            connection = new SqlConnection(connetionString);

            connection.Open();
        }

        public int DoCommand(string command)
        {
            SqlCommand cmd = new SqlCommand(command, connection);

            return cmd.ExecuteNonQuery();
        }

        public DataTable DoQuery(string query)
        {
            SqlCommand cmd = new SqlCommand(query, connection);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            sda.Fill(dt);

            return dt;
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
