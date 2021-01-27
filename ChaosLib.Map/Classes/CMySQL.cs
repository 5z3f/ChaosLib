using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ChaosLib.Map.Classes
{
    class CMySQL
    {
        // full rewrite needed

        public static List<cDatabaseConfig> configStructure;
        public static MySqlConnection mysqlConnection;

        public static string strProvider;

        internal class cSettings
        {
            public List<cDatabaseConfig> CONNECTION;
        }

        internal class cDatabaseConfig
        {
            public string Name,
                Note,
                Host,
                Database,
                Username,
                Password;

            public int Port;
        }

        public static bool SetConnection(string strName)
        {
            cDatabaseConfig cConfig = configStructure.Find(p => p.Name.Equals(strName));

            if (cConfig == null)
                return false;

            strProvider = string.Format("Data Source={0}; Port={1}; Database={2}; User ID={3}; Password={4}; SslMode=none;", cConfig.Host, cConfig.Port, cConfig.Database, cConfig.Username, cConfig.Password);
            return true;
        }


        public static async Task<DataTable> QueryToDataTable(string sql)
        {
            var dt = new DataTable();
            using (mysqlConnection = new MySqlConnection(strProvider))
            {
                mysqlConnection.Open();
                var reader = await MySqlHelper.ExecuteReaderAsync(mysqlConnection, sql);
                dt.Load(reader);
                mysqlConnection.Close();
            }

            return dt;
        }

        public static async void ExecuteQuery(string query)
        {
            using (mysqlConnection = new MySqlConnection(strProvider))
            {
                mysqlConnection.Open();
                await MySqlHelper.ExecuteNonQueryAsync(mysqlConnection, query);
            }
        }

        public static async Task<object> QueryToObject(string query)
        {
            object result = null;

            try
            {
                using (mysqlConnection = new MySqlConnection(strProvider))
                {
                    mysqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(query, mysqlConnection);
                    result = await mySqlCommand.ExecuteScalarAsync();
                    mysqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                //
            }

            return result;
        }

        public static bool TestConnection(cDatabaseConfig settings)
        {
            try
            {
                object[] objArray = new object[5]
                {
                    settings.Host,
                    settings.Port,
                    settings.Database,
                    settings.Username,
                    settings.Password
                };

                using (mysqlConnection = new MySqlConnection(string.Format("Data Source={0}; Port={1}; Database={2}; User ID={3}; Password={4}; SslMode=none;", objArray)))
                {
                    mysqlConnection.Open();
                    mysqlConnection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                //
                return false;
            }
        }

        public static bool TestConnection(string sDescription)
        {
            cDatabaseConfig data = configStructure.Find(p => p.Note.Equals(sDescription));

            if (data != null)
                return TestConnection(data);

            return false;
        }

    }
}
