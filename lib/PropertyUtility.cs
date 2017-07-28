using System.Data.Linq.Mapping;

namespace System.Linq.Dynamic
{
    public static class PropertyUtility
    {
        public static object ConvertToType(string typeName, object value)
        {
            switch (typeName)
            {
                case "Int32":
                    return Convert.ToInt32(value);
                case "Int64":
                    return Convert.ToInt64(value);
                case "Boolean":
                    return Convert.ToBoolean(value);
                case "Decimal":
                    return Convert.ToDecimal(value);
                case "Double":
                    return Convert.ToDouble(value);
                case "String":
                case "string":
                    return value == null ? "" : value.ToString();
                default:
                    return value;
            }
        }

        public static object ConvertToType(MetaDataMember metaData, object value)
        {
            return ConvertToType(metaData.Type.Name, value);
        }
    }
}