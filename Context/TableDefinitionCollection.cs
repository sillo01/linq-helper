using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace Guayaba.LinqHelper.Context
{
    public class TableDefinitionCollection : Dictionary<System.Type, TableDefinition>
    {
        private static TableDefinitionCollection _current;

        public static TableDefinitionCollection Current
        {
            get { return _current ?? (_current = new TableDefinitionCollection()); }
        }

        public static TableDefinition GetDefinition(Type type, DataContext context)
        {
            if (!Current.ContainsKey(type))
            {
                Current[type] = new TableDefinition(type, context);
            }
            return Current[type];
        }
        public static string GetTableCommand(Type type, DataContext context)
        {
            if (!Current.ContainsKey(type))
            {
                Current[type] = new TableDefinition(type, context);
            }
            return Current[type].CommandText;
        }
        public static string GetTableName(Type type, DataContext context)
        {
            if (!Current.ContainsKey(type))
            {
                Current[type] = new TableDefinition(type, context);
            }
            return Current[type].TableName;
        }
    }
}