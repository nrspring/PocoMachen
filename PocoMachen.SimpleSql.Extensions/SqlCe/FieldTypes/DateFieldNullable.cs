namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    using System;

    public class DateFieldNullable : BaseField
    {
        public DateFieldNullable IsNotNull()
        {
            Parent.AddWhere(string.Format("{0} is not NULL", FieldName));
            return this;
        }

        public DateFieldNullable IsNull()
        {
            Parent.AddWhere(string.Format("{0} is NULL", FieldName));
            return this;
        }

        public DateFieldNullable MinDate(DateTime value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} >= '{1}')", FieldName, value.ToString("yyyy-MM-dd hh:mm:ss")));
            return this;
        }

        public DateFieldNullable MaxDate(DateTime value)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} <= '{1}')", FieldName, value.ToString("yyyy-MM-dd hh:mm:ss")));
            return this;
        }

        public DateFieldNullable DateRange(DateTime startDate, DateTime endDate)
        {
            Parent.AddWhere(string.Format("({0} is null or {0} >= '{1}')", FieldName, startDate.ToString("yyyy-MM-dd hh:mm:ss")));
            Parent.AddWhere(string.Format("({0} is null or {0} <= '{1}')", FieldName, endDate.ToString("yyyy-MM-dd hh:mm:ss")));

            return this;
        }

        public DateFieldNullable(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }
    }

}
