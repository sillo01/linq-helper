using System.Collections.Generic;

namespace System.Linq.Dynamic
{
    public static class EntityExtensions
    {

        public static List<string> ValidTypesTocopy =
           new List<string> { "Int32", "Int64", "Decimal", "DateTime", "Float", "Double", "String", "Boolean", "short", "Int16",
                "Nullable`1" };

        public static void CopyTo<T>(this Dictionary<string, object> from, T to)
        {
            var type = typeof(T);
            foreach (var value in from.Where(e => type.GetProperties().Any(p => p.Name == e.Key)))
            {
                var prop = type.GetProperty(value.Key);
                prop.SetValue(to, ConvertToType(prop.PropertyType.Name, value.Value), null);
            }
        }

        private static object ConvertToType(string typeName, object value)
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
                default:
                    return value;
            }
        }
    }
}