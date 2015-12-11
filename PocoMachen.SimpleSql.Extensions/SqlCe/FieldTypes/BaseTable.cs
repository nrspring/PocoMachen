namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    using System.Text;

    public class BaseTable
    {
        public string Select { get; set; }
        public string From { get; set; }
        public string Where { get; set; }
        public string OrderBy { get; set; }
        public string SkipTake { get; set; }

        public string Sql()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Select {0} From {1} ", Select, From);

            if (!string.IsNullOrEmpty(Where))
            {
                sb.AppendFormat("Where {0} ", Where);
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                sb.AppendFormat("Order By {0} ", OrderBy);
            }

            if (!string.IsNullOrEmpty(SkipTake))
            {
                sb.Append(SkipTake +  " ");
            }

            return sb.ToString();
        }

        public void AddWhere(string value)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = Where + " and ";
            }

            Where = Where + value;
        }

        public void Top(int value)
        {
            Select = string.Format("Top {0} {1}", value, Select);
        }
    }
}
