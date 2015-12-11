using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionCe.PocoMachenProvider
{
    public class ExtendedColumnInfo
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public bool IsNullable { get; set; }
    }
}
