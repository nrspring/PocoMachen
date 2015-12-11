namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    public class DoubleField : BaseField
    {
        public DoubleField Equals(double value)
        {
            Parent.AddWhere(string.Format("{0} = {1}", FieldName, value));
            return this;
        }

        public DoubleField LessThan(double value)
        {
            Parent.AddWhere(string.Format("{0} < {1}", FieldName, value));
            return this;
        }

        public DoubleField GreaterThan(double value)
        {
            Parent.AddWhere(string.Format("{0} > {1}", FieldName, value));
            return this;
        }

        public DoubleField LessThanOrEqualTo(double value)
        {
            Parent.AddWhere(string.Format("{0} <= {1}", FieldName, value));
            return this;
        }

        public DoubleField GreaterThanOrEqualTo(double value)
        {
            Parent.AddWhere(string.Format("{0} >= {1}", FieldName, value));
            return this;
        }

        public DoubleField NotEqualTo(double value)
        {
            Parent.AddWhere(string.Format("{0} <> {1}", FieldName, value));
            return this;
        }

        public DoubleField(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }
    }

}
