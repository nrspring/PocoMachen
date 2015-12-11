using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{
    public class OrderBy
    {
        protected BaseTable Parent { get; set; }
        protected string FieldName { get; set; }

        public OrderBy(BaseTable parent, string fieldName)
        {
            Parent = parent;
            FieldName = fieldName;
        }

        public void SkipTake(int skip, int take)
        {
            Parent.SkipTake = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", skip, take);
        }
    }
}
