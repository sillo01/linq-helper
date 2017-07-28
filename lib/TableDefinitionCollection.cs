using System.Collections.Generic;
using System.Data.Linq;

namespace System.Linq.Dynamic
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
    }
}