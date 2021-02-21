using System;
using System.Data;

using MySql.Data.MySqlClient;
using NPoco;

namespace ChaosLib.Map.Classes
{
    public class CMySQL
    {
        public static MySqlConnection mysqlConnection;

        public class CDatabaseConfig
        {
            public string Name,
                Description,
                Host,
                Database,
                Username,
                Password;

            public int Port;
        }

        public static (bool success, string msg) SetConnection(string host, int port, string database, string username, string password)
        {
            var test = TestConnection(host, port, database, username, password);
            if (!test.success) return (false, test.msg);

            string connectionString =
                $"Data Source={host};" +
                $"Port={port};" +
                $"Database={database};" +
                $"User ID={username};" +
                $"Password={password};" +
                $"Character Set=utf8";

            mysqlConnection = new MySqlConnection(connectionString);

            return (true, "ok");
        }

        public static dynamic QueryToClass<T>(dynamic c, string sql)
        {
            var test = TestConnection(mysqlConnection);
            if (!test.success) return false;

            using (var db = new Database(mysqlConnection, DatabaseType.MySQL))
            {
                mysqlConnection.Open();
                c.Data = db.Fetch<T>(sql).ToArray();
                mysqlConnection.Close();
            }

            return c;
        }

        public static dynamic ExecuteQuery(string sql)
        {
            var test = TestConnection(mysqlConnection);
            if (!test.success) return false;

            dynamic result = null;

            using (var db = new Database(mysqlConnection, DatabaseType.MySQL))
            {
                mysqlConnection.Open();
                result = db.Fetch<dynamic>(sql);
                mysqlConnection.Close();
            }

            return result;
        }

        public static (bool success, string msg) Action<T>(string tableName, dynamic dataObject, Command c)
        {
            dynamic dbt = null;
            string primaryKey = "a_index";

            var test = TestConnection(mysqlConnection);
            if (!test.success) return (false, test.msg);

            try
            {
                using (var db = new Database(mysqlConnection, DatabaseType.MySQL))
                {
                    mysqlConnection.Open();

                    _ = c switch
                    {
                        Command.Insert => db.Insert<T>(dataObject),
                        Command.Update => db.Update(tableName, primaryKey, dataObject),
                        Command.Delete => db.Delete(tableName, primaryKey, dataObject)
                    };

                    mysqlConnection.Close();

                    dbt = (true, "ok");
                }
            }
            catch (Exception ex) {
                dbt = (false, ex.Message);
            }

            return dbt;
        }

        public static (bool success, string msg) TestConnection(string host, int port, string database, string username, string password)
        {
            dynamic state = null;
            MySqlConnection testConnection = null;

            try
            {
                string connectionString = 
                    $"Data Source={host};" +
                    $"Port={port};" +
                    $"Database={database};" +
                    $"User ID={username};" +
                    $"Password={password}";

                testConnection = new MySqlConnection(connectionString);
                testConnection.Open();

                state = (true, "ok");
            }
            catch (MySqlException ex) {
                state = (false, ex.Message);
            }
            finally
            {
                if (testConnection.State == ConnectionState.Open)
                    testConnection.Close();
            }

            return state;
        }

        public static (bool success, string msg) TestConnection(MySqlConnection testConnection)
        {
            dynamic state = null;

            // if testconnection is not null

            try
            {
                testConnection.Open();
                state = (true, "ok");
            }
            catch (MySqlException ex)
            {
                state = (false, ex.Message);
            }
            finally
            {
                if (testConnection.State == ConnectionState.Open)
                    testConnection.Close();
            }

            return state;
        }
    }
}
