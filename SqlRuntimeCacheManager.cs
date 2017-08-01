using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Runtime.Caching;
using System.Data.SqlClient;
using LinqHelper.Extension;

namespace LinqHelper

{
    public sealed class SqlRuntimeCacheManager
    {
        private static string dbName = "CachedDatabase";
        private static string ConnectionString;
        private static DataContext _DataContext;
        private static SqlRuntimeCacheManager _Instance;
        private static HashSet<string> _RegisteredTables;

        public static DataBaseEntity dbEntity
        {
            get
            {
                var ctx = DataContextFactory.GetScopedDataContext<DataBaseEntity>(dbName);
                if (ctx.Context == null)
                {
                    ctx.Context = _DataContext;
                }

                return ctx;
            }
        }
        public static SqlRuntimeCacheManager Instance
        {
            get
            {
                return _Instance;
            }
        }
        private HashSet<string> RegisteredTables { get; }

        // Initialization
        public static void GenerateInstance(DataContext Context)
        {
            _Instance = new SqlRuntimeCacheManager(Context);
            _RegisteredTables = new HashSet<string>();
        }
        private SqlRuntimeCacheManager(DataContext Context)
        {
            SqlRuntimeCacheManager.ConnectionString = Context.Connection.ConnectionString;
            _DataContext = Context;
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
            //var tableName = dbEntity.GetTableName<T>();
            var tableName = _DataContext.GetTableName<T>();

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
                        cmd.CommandText = dbEntity.Context.GetCommand(dbEntity.Context.GetTable<T>()).CommandText;
                        cmd.Notification = null;

                        SqlDependency dep = new SqlDependency();
                        dep.AddCommandDependency(cmd);

                        //MemoryCache.Default.Set(table, value, policy);
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

                        result = dbEntity.Context.Translate<T>(cmd.ExecuteReader()).ToList();
                        MemoryCache.Default.Set(tableName, result, policy);
                    }
                }
            }

            return result;
        }
    }
}