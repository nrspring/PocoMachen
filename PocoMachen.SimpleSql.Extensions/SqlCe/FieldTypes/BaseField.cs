using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{
    public class BaseField
    {
        protected BaseTable Parent { get; set; }
        protected string FieldName { get; set; }

        public BaseField(BaseTable parent, string fieldName)
        {
            Parent = parent;
            FieldName = fieldName;
        }

        public OrderBy OrderBy()
        {
            if (!String.IsNullOrEmpty(Parent.OrderBy))
            {
                Parent.OrderBy = Parent.OrderBy + ",";
            }

            Parent.OrderBy = string.Format("{0} {1}", Parent.OrderBy,FieldName);

            return new OrderBy(Parent, FieldName);
        }

        public OrderBy OrderByDescending()
        {
            if (!String.IsNullOrEmpty(Parent.OrderBy))
            {
                Parent.OrderBy = Parent.OrderBy + ",";
            }

            Parent.OrderBy = string.Format("{0} {1} Desc", Parent.OrderBy, FieldName);

            return new OrderBy(Parent, FieldName);
        }
    }
}
