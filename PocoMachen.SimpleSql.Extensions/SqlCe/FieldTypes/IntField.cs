namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    public class IntField : BaseField
    {
        public IntField Equals(int value)
        {
            Parent.AddWhere(string.Format("{0} = {1}", FieldName, value));
            return this;
        }

        public IntField LessThan(int value)
        {
            Parent.AddWhere(string.Format("{0} < {1}", FieldName, value));
            return this;
        }

        public IntField GreaterThan(int value)
        {
            Parent.AddWhere(string.Format("{0} > {1}", FieldName, value));
            return this;
        }

        public IntField LessThanOrEqualTo(int value)
        {
            Parent.AddWhere(string.Format("{0} <= {1}", FieldName, value));
            return this;
        }

        public IntField GreaterThanOrEqualTo(int value)
        {
            Parent.AddWhere(string.Format("{0} >= {1}", FieldName, value));
            return this;
        }

        public IntField NotEqualTo(int value)
        {
            Parent.AddWhere(string.Format("{0} <> {1}", FieldName, value));
            return this;
        }

        public IntField(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }
    }
}
