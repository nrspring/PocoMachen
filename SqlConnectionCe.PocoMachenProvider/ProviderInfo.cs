using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PocoMachen.Integration.Models;

namespace SqlConnectionCe.PocoMachenProvider
{
    public class ProviderInfo : PocoMachen.Integration.IProviderBinder
    {
        public string GetProviderName()
        {
            return "sqlconnectionce";
        }

        public List<Table> GetTables(Dictionary<string, string> commandLineArgs)
        {
            if (!commandLineArgs.ContainsKey("connectionstring"))
            {
                throw new Exception("This provider requires a connectionstring argument");
            }

            return GetTables(commandLineArgs["connectionstring"]);
        }

        private List<Table> GetTables(string connectionString)
        {
            var tables = GetListOfTables(connectionString);

            foreach (var current in tables)
            {
                SetTableColumns(current, connectionString);
            }

            return tables;
        }

        private List<Table> GetListOfTables(string connectionString)
        {
            var returnList = new List<Table>();

            using (var conn = new System.Data.SqlServerCe.SqlCeConnection(connectionString))
            {
                conn.Open();
                string query = "select table_name from information_schema.tables where TABLE_TYPE <> 'VIEW'";

                using (var cmd = new System.Data.SqlServerCe.SqlCeCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new Table();

                        item.Name = reader["table_name"].ToString();

                        returnList.Add(item);
                    }
                }
            }

            return returnList;
        }

        private void SetTableColumns(Table table, string connectionString)
        {
            using (var conn = new System.Data.SqlServerCe.SqlCeConnection(connectionString))
            {
                conn.Open();
                string query = $"Select * from {table.Name} where 1=0";

                using (var cmd = new System.Data.SqlServerCe.SqlCeCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        table.Columns.Add(new Column()
                        {
                            Name = reader.GetName(col),
                            DotNetType = reader.GetFieldType(col).ToString()
                        });
                    }
                }
            }
        }
    }
}
