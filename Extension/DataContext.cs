using System.Data.Linq;

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
    }
}
