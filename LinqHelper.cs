using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Runtime.Caching;
using System.Data.SqlClient;
using Guayaba.LinqHelper.Context;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Linq.Expressions;

namespace Guayaba.LinqHelper

{
    public sealed class LinqHelper
    {
        //private static string ConnectionString;
        internal static DataContext DataContext;
        private static LinqHelper _Instance;
        private static string _ConnectionString;

        public static LinqHelper Instance
        {
            get
            {
                return _Instance;
            }
        }
        private string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        // Initialization
        public static void GenerateInstance(DataContext Context)
        {
            _Instance = new LinqHelper(Context);
            _ConnectionString = Context.Connection.ConnectionString;
        }
        private LinqHelper(DataContext Context)
        {
            DataContext = Context;
        }

        // SQL Dependencies
        public void RegisterDependency()
        {
            SqlDependency.Start(ConnectionString);
        }
        public void DisableCacheNotifications()
        {
            SqlDependency.Stop(ConnectionString);
        }

        // Synchronous methods
        public List<T> GetAllCached<T>()
            where T : class, IDataEntity
        {
            var tableName = TableDefinitionCollection.GetTableName(typeof(T), DataContext);

            List<T> result = null;
            result = (List<T>)MemoryCache.Default.Get(tableName);

            if (result == null)
            {
                result = RegisterDependency<T>(tableName);
            }

            return result;
        }

        public T SelectByPK<T>(string id) where T : class
        {
            var table = DataContext.GetTable<T>();
            MetaModel modelMap = DataContext.Mapping;

            ReadOnlyCollection<MetaDataMember> dataMembers = modelMap.GetMetaType(typeof(T)).DataMembers;
            string PrimaryKeyName = (dataMembers.FirstOrDefault<MetaDataMember>(m => m.IsPrimaryKey)).Name;

            //return table.FirstOrDefault<T>(delegate (T t)
            //{
            //    String memberId = t.GetType().GetProperty(PrimaryKeyName).GetValue(t, null).ToString();
            //    return memberId == id.ToString();
            //});

            string tableCmd = TableDefinitionCollection.GetTableCommand(typeof(T), DataContext);
            string strCmd = tableCmd + " WHERE " + PrimaryKeyName + " = @id;";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(strCmd, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters["@id"].Value = int.Parse(id);

                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    return DataContext.Translate<T>(reader).FirstOrDefault();
                }
            }
        }
        public T InserOrUpdate<T>(T item, bool submit = false)
            where T : class, IDataEntity
        {
            bool isNew = (item.Id == "0" || string.IsNullOrEmpty(item.Id));

            if (isNew)
            {
                Type type = typeof(T);
                var table = DataContext.GetTable(type);
                table.InsertOnSubmit(item);
            }
            else
            {
                Type type = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                var table = DataContext.Mapping.GetTable(type);
                var members = table.RowType.DataMembers;
                var key = members.Where(x => x.IsPrimaryKey == true).FirstOrDefault();

                string predicate = "";
                if (key != null)
                {
                    predicate = (key.Name != null ? string.Format("{0}=@0", key.Name) : "");
                }

                var updated = SelectByPK<T>(item.Id);

                foreach (PropertyDescriptor currentProp in properties)
                {
                    if (currentProp.Attributes[typeof(System.Data.Linq.Mapping.ColumnAttribute)] != null)
                    {
                        object val = currentProp.GetValue(item);
                        currentProp.SetValue(updated, val);
                    }
                }
            }

            if (submit) DataContext.SubmitChanges();
            return item;
        }

        // Async methods
        public async Task<T> SelectByPKAsync<T>(string id, CancellationToken token = default(CancellationToken)) where T : class
        {
            var table = DataContext.GetTable<T>();
            MetaModel modelMap = DataContext.Mapping;

            ReadOnlyCollection<MetaDataMember> dataMembers = modelMap.GetMetaType(typeof(T)).DataMembers;
            string PrimaryKeyName = (dataMembers.FirstOrDefault<MetaDataMember>(m => m.IsPrimaryKey)).Name;

            string tableCmd = TableDefinitionCollection.GetTableCommand(typeof(T), DataContext);
            string strCmd = tableCmd + " WHERE " + PrimaryKeyName + " = @id;";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(strCmd, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters["@id"].Value = int.Parse(id);

                    await conn.OpenAsync(token);
                    var reader = await cmd.ExecuteReaderAsync(token);
                    return DataContext.Translate<T>(reader).FirstOrDefault();
                }
            }
        }

        // private methods
        private List<T> RegisterDependency<T>(string tableName)
        {
            List<T> result = null;
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = TableDefinitionCollection.GetTableCommand(typeof(T), DataContext);
                    cmd.Notification = null;

                    SqlDependency dep = new SqlDependency();
                    dep.AddCommandDependency(cmd);

                    var policy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                        Priority = System.Runtime.Caching.CacheItemPriority.Default,
                        SlidingExpiration = new TimeSpan(0, 20, 0),
                        RemovedCallback = (CacheEntryRemovedArguments args) =>
                        {
                            MemoryCache.Default.Remove(tableName);
                        }
                    };
                    SqlChangeMonitor mon = new SqlChangeMonitor(dep);
                    policy.ChangeMonitors.Add(mon);

                    result = DataContext.Translate<T>(cmd.ExecuteReader()).ToList();
                    MemoryCache.Default.Set(tableName, result, policy);
                }
            }
            return result;
        }
    }
}