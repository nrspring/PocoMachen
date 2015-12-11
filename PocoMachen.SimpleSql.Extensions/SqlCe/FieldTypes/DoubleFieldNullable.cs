namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    public class DoubleFieldNullable : BaseField
    {
        public DoubleFieldNullable IsNotNull()
        {
            Parent.AddWhere(string.Format("{0} is not NULL", FieldName));
            return this;
        }

        public DoubleFieldNullable IsNull()
        {
            Parent.AddWhere(string.Format("{0} is NULL", FieldName));
            return this;
        }

        public DoubleFieldNullable Equals(double value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} = {1})", FieldName, value));
            return this;
        }

        public DoubleFieldNullable LessThan(double value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} < {1})", FieldName, value));
            return this;
        }

        public DoubleFieldNullable GreaterThan(double value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} > {1})", FieldName, value));
            return this;
        }

        public DoubleFieldNullable LessThanOrEqualTo(double value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} <= {1})", FieldName, value));
            return this;
        }

        public DoubleFieldNullable GreaterThanOrEqualTo(double value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} >= {1})", FieldName, value));
            return this;
        }

        public DoubleFieldNullable NotEqualTo(double value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} <> {1})", FieldName, value));
            return this;
        }

        public DoubleFieldNullable(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }
    }
}
