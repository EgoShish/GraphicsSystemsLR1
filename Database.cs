using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsPanelStanki
{
    public class Database
    {
        private MySqlDataAdapter adapter;
        private MySqlConnection connection;

        public Database()
        {
            connection = new MySqlConnection("Server=localhost; Database=stankidb; User ID=root; Password=root");
            adapter = new MySqlDataAdapter();
        }
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed) { connection.Open(); }
        }
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open) { connection.Close(); }
        }
        public DataTable GetTypesList()
        {
            DataTable table = new DataTable();
            this.OpenConnection();

            MySqlCommand command = new MySqlCommand("GetTypesList", connection);
            command.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = command;
            adapter.Fill(table);
          
            this.CloseConnection();
            return table;
        }
        public DataTable GetModelsByTypeName(string name)
        {
            DataTable table = new DataTable();
            this.OpenConnection();

            MySqlCommand command = new MySqlCommand("GetModelsByTypeName", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@target_typename", name);
            adapter.SelectCommand = command;
            adapter.Fill(table);

            this.CloseConnection();
            return table;
        }
        public DataTable GetMachineStatusLogs(string name)
        {
            DataTable table = new DataTable();
            this.OpenConnection();

            MySqlCommand command = new MySqlCommand("GetMachineStatusLogs", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@target_name", name);
            adapter.SelectCommand = command;
            adapter.Fill(table);

            this.CloseConnection();
            return table;
        }
    }
}
