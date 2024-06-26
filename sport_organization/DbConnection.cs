using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sport_organization
{
    class DbConnection
    {
        private string connectionString;
        public DbConnection()
        {
            string host = "172.20.7.8";
            string port = "5432";
            string username = "st1991";
            string password = "pwd1991";
            string database = "sport_organization";

            connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";
        }
        public bool CheckConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
                return false;
            }
        }
        public string autorization(string login, string password)
        {
            string roleName = null;

            string query = "SELECT r.name FROM roles r JOIN users u ON r.id = u.roles_id WHERE u.login = @login AND u.password = md5(@password)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("login", login);
                    command.Parameters.AddWithValue("password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            roleName = reader.GetString(0);
                        }
                    }
                }
            }

            return roleName;
        }
        public void registration(string login, string password)
        {
            string query = "INSERT INTO users (login, password, roles_id) VALUES(@login, md5(@password), 3);  ";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("login", login);
                    command.Parameters.AddWithValue("password", password);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно зарегистрированы");
                }
            }
        }
        public DataTable GetInfo(string login, string option)
        {
            DataTable table = new DataTable();

            string query = $"SELECT * FROM {option} (@login)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("login", login);

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }

            return table;
        }
        public void SetInfo(string login, string option)
        {
            string query = "INSERT INTO " + option + @" (users_id)
                     VALUES ((SELECT id FROM users WHERE login = @login LIMIT 1));";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("login", login);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateData(string query, object parameterValue)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("value", parameterValue);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void deleteTeam(string query, int id, string login)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("login", login);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateInfo(string query, object parameterValue, string login)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("value", parameterValue);
                    command.Parameters.AddWithValue("login", login);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void InsertTeam(string query, int id, string login)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("login", login);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteData(string query, int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertValue(string tableName, string columnName)
        {
            string query = $"INSERT INTO {tableName} ({columnName}) VALUES (NULL);";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public DataTable GetAllTables()
        {
            DataTable dt = new DataTable();
            string query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'  AND table_type = 'BASE TABLE';";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
        public DataTable GetTableData(string tableName)
        {
            DataTable dataTable = new DataTable();
            string query = $"SELECT * FROM {tableName}";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        public object getNewValue(string typeColumn, string value)
        {
            object newValue = null;

            switch (typeColumn)
            {
                case "DateTime":
                    newValue = Convert.ToDateTime(value);
                    break;
                case "Decimal":
                    newValue = Convert.ToDecimal(value);
                    break;
                case "String":
                    newValue = value;
                    break;
                case "Int32":
                    newValue = Convert.ToInt32(value);
                    break;
                case "Boolean":
                    newValue = Convert.ToBoolean(value);
                    break;
            }

            return newValue;
        }

    }
}
