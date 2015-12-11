namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    using System;

    public class DateField : BaseField
    {
        public DateField(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }

        public DateField MinDate(DateTime value)
        {
            Parent.AddWhere(string.Format("{0} >= '{1}'", FieldName, value.ToString("yyyy-MM-dd HH:mm:ss")));
            return this;
        }

        public DateField MaxDate(DateTime value)
        {
            Parent.AddWhere(string.Format("{0} <= '{1}'", FieldName, value.ToString("yyyy-MM-dd HH:mm:ss")));
            return this;
        }

        public DateField DateRange(DateTime startDate, DateTime endDate)
        {
            Parent.AddWhere(string.Format("{0} >= '{1}'", FieldName, startDate.ToString("yyyy-MM-dd HH:mm:ss")));
            Parent.AddWhere(string.Format("{0} <= '{1}'", FieldName, endDate.ToString("yyyy-MM-dd HH:mm:ss")));

            return this;
        }
    }

}
