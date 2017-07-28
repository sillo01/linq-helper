using System.Collections.Generic;
using System.Data.Linq;
using System.Transactions;
using LinqHelper;

namespace System.Linq.Dynamic
{
    public class DataBaseEntity
    {
        private DataContext _context;
        public DataContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public IQueryable<T> Get<T>(Dictionary<string, object> filter) where T : class, IDataEntity
        {
            var type = typeof(T);

            var table = Context.GetTable<T>();
            if (filter.Count == 0)
            {
                return table;
            }

            var filterParams = filter.Select(e => e.Value).ToArray();
            var filterString = "";
            var i = 0;

            var definition = TableDefinitionCollection.GetDefinition(type, this.Context);

            foreach (var item in filter.Where(e => definition.ColumnMembers.Any(p => p.Name == e.Key)))
            {
                filterString += CreateSingleFilter(item.Key, item.Value, i++);
                if (i < filter.Count)
                {
                    filterString += " AND ";
                }
            }

            if (filterString.Length == 0)
            {
                return table;
            }

            return table.Where<T>(filterString, filterParams);
        }
        public List<T> GetAll<T>() where T : class, IDataEntity
        {
            var type = typeof(T);
            var definition = TableDefinitionCollection.GetDefinition(type, this.Context);
            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                return Context.GetTable<T>().ToList();
            }
        }
        public string GetTableName<T>() where T : class, IDataEntity
        {
            var type = typeof(T);
            var definition = TableDefinitionCollection.GetDefinition(type, this.Context);

            var tableParts = definition.TableName.Split('.');

            return tableParts[tableParts.Length - 1];
        }
        public string CreateSingleFilter(string column, object value, int index)
        {
            switch (value.GetType().Name)
            {
                case "String":
                case "string":
                    return string.Format("{0}.Contains(@{1})", column, index);
                default:
                    return string.Format("{0}=@{1}", column, index);
            }
        }
    }
}