using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Linq;
using System.Runtime.Caching;
using System.Data.SqlClient;
using LinqHelper.Extension;
using LinqHelper.Context;

namespace LinqHelper

{
    public sealed class SqlRuntimeCacheManager
    {
        //private static string ConnectionString;
        private static DataContext DataContext;
        private static SqlRuntimeCacheManager _Instance;

        public static SqlRuntimeCacheManager Instance
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
                return DataContext.Connection.ConnectionString;
            }
        }

        // Initialization
        public static void GenerateInstance(DataContext Context)
        {
            _Instance = new SqlRuntimeCacheManager(Context);
        }
        private SqlRuntimeCacheManager(DataContext Context)
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

        public List<T> GetAllCached<T>()
            where T : class, IDataEntity
        {
            var tableName = TableDefinitionCollection.GetTableName(typeof(T), DataContext);

            List<T> result = null;
            result = (List<T>)MemoryCache.Default.Get(tableName);

            if (result == null)
            {
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
            }

            return result;
        }
    }
}