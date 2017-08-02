using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqHelper.Context
{
    public class TableDefinition
    {
        private readonly Type _type;
        public string TableName;
        public string CommandText;

        public TableDefinition(System.Type type, DataContext context)
        {
            _type = type;
            var table = context.Mapping.GetTable(type);
            this.TableName = table.TableName;
            CommandText = context.GetCommand(context.GetTable(type)).CommandText;
        }
    }

}
