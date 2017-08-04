using System;
using System.Data.Linq;

namespace Guayaba.LinqHelper.Context
{
    public class TableDefinition
    {
        private readonly Type _type;
        public string TableName { get; set; }
        public string CommandText { get; set; }

        public TableDefinition(System.Type type, DataContext context)
        {
            _type = type;
            var table = context.Mapping.GetTable(type);
            this.TableName = table.TableName;
            CommandText = context.GetCommand(context.GetTable(type)).CommandText;
        }
    }

}
