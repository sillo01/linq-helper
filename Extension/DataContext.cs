using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;

namespace Guayaba.LinqHelper.Extension
{
    public static class DataContextEx
    {
        public static string GetTableName<T>(this DataContext context)
        {
            string tableName = string.Empty;

            var model = context.Mapping;
            var tables = model.GetTables();
            
            foreach (var table in tables)
            {
                if (table.GetType() == typeof(T))
                {
                    tableName = table.TableName;
                    return tableName;
                }
            }
            return tableName;
        }
        public static string GetPKValue<T>(this DataContext context, T item)
        {
            Type type = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            var table = context.Mapping.GetTable(type);
            var members = table.RowType.DataMembers;
            var key = members.Where(x => x.IsPrimaryKey == true).FirstOrDefault();
            var pk = properties.Find(key.Name, true);
            return pk.GetValue(item).ToString();
        }
    }
}
