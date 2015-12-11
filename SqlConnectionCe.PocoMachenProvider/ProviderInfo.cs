using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PocoMachen.Integration.Models;

namespace SqlConnectionCe.PocoMachenProvider
{
    public class ProviderInfo : PocoMachen.Integration.IProviderBinder
    {
        private List<ExtendedColumnInfo> ExtendedInfo { get; set; }

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

            PopulateExtendedInfo(commandLineArgs["connectionstring"]);

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
                    //SELECT TABLE_NAME, COLUMN_NAME, iS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS 
                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        table.Columns.Add(new Column()
                        {
                            Name = reader.GetName(col),
                            DotNetType = AddNullableToDotNetType(table.Name, reader.GetName(col), reader.GetFieldType(col).ToString())
                        });
                    }
                }
            }
        }

        private void PopulateExtendedInfo(string connectionString)
        {
            ExtendedInfo = new List<ExtendedColumnInfo>();

            using (var conn = new System.Data.SqlServerCe.SqlCeConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TABLE_NAME, COLUMN_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS";

                using (var cmd = new System.Data.SqlServerCe.SqlCeCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ExtendedInfo.Add(new ExtendedColumnInfo()
                        {
                            ColumnName = reader["COLUMN_NAME"].ToString(),
                            TableName = reader["TABLE_NAME"].ToString(),
                            IsNullable = reader["IS_NULLABLE"].ToString() == "YES"
                        });
                    }
                }
            }
        }

        private string AddNullableToDotNetType(string tableName, string columnName, string baseType)
        {
            if (baseType.Equals("System.String", StringComparison.InvariantCultureIgnoreCase)) return baseType;

            var item =
                ExtendedInfo.Where(x => x.TableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase) &&
                                        x.ColumnName.Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

            if (item != null && item.IsNullable) return baseType + "?";

            return baseType;
        }
    }
}
