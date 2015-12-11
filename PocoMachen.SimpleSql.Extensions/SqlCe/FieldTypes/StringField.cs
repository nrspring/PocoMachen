

namespace PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes
{

    public class StringField :BaseField
    {

        public StringField Equals(string value)
        {
            Parent.AddWhere(string.Format("{0} = '{1}'", FieldName, value));
            return this;
        }

        public StringField Contains(string value)
        {
            Parent.AddWhere(string.Format("{0} like '%{1}%'", FieldName, value));
            return this;
        }

        public StringField(BaseTable parent, string fieldName) : base(parent, fieldName)
        {
        }
    }
}
