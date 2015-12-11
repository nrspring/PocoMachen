namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    public class IntFieldNullable : BaseField
    {

        public IntFieldNullable IsNotNull()
        {
            Parent.AddWhere(string.Format("{0} is not NULL", FieldName));
            return this;
        }

        public IntFieldNullable IsNull()
        {
            Parent.AddWhere(string.Format("{0} is NULL", FieldName));
            return this;
        }

        public IntFieldNullable Equals(int value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} = {1})", FieldName, value));
            return this;
        }

        public IntFieldNullable LessThan(int value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} < {1})", FieldName, value));
            return this;
        }

        public IntFieldNullable GreaterThan(int value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} > {1})", FieldName, value));
            return this;
        }

        public IntFieldNullable LessThanOrEqualTo(int value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} <= {1})", FieldName, value));
            return this;
        }

        public IntFieldNullable GreaterThanOrEqualTo(int value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} >= {1})", FieldName, value));
            return this;
        }

        public IntFieldNullable NotEqualTo(int value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} <> {1})", FieldName, value));
            return this;
        }

        public IntFieldNullable(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }
    }

}
