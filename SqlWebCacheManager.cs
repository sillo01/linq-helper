using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Linq.Dynamic;
using System.Data.Linq;
using LinqHelper.Old;


namespace LinqHelper

{
    public class SqlWebCacheManager
    {
        private static string dbName = "CachedDatabase";
        private static string ConnectionString;
        private static DataContext _DataContext;
        private static SqlWebCacheManager _Instance;

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

        public static SqlWebCacheManager Instance
        {
            get
            {
                return _Instance;
            }
        }
        public static void GenerateInstance(DataContext Context)
        {
            _Instance = new SqlWebCacheManager(Context);
        }
        private SqlWebCacheManager(DataContext Context)
        {
            SqlWebCacheManager.ConnectionString = Context.Connection.ConnectionString;
            _DataContext = Context;
        }

        public void RegisterDependency()
        {
            SqlCacheDependencyAdmin.EnableNotifications(ConnectionString);
        }
        public void DisableCacheNotifications()
        {
            SqlCacheDependencyAdmin.DisableNotifications(ConnectionString);
        }
        public void RegisterTableDependency(string table)
        {
            var currentTables = SqlCacheDependencyAdmin
                .GetTablesEnabledForNotifications(ConnectionString);
            if (!currentTables.Contains(table))
            {
                SqlCacheDependencyAdmin
                    .EnableTableForNotifications(ConnectionString, table);
            }
        }
        public void SaveCache(string name, object value, CacheItemRemovedCallback callback)
        {
            RegisterTableDependency(name);
            var dependency = new SqlCacheDependency(dbName, name);
            HttpRuntime.Cache.Add(
                name, value, dependency, Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0), CacheItemPriority.Normal, callback
                );
        }
        void OnCacheRemoved(string name, object cachedObject, CacheItemRemovedReason reason)
        {
            HttpRuntime.Cache.Remove(name);
        }

        public List<T> GetAllCached<T>()
            where T : class, IDataEntity
        {
            var tableName = dbEntity.GetTableName<T>();
            List<T> result = null;
            result = (List<T>)HttpRuntime.Cache[tableName];

            if (result == null)
            {
                var elements = dbEntity.GetAll<T>().ToList().OrderBy(e => e.Id);
                result = (List<T>)Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(T)));
                result.AddRange(elements);
                SaveCache(tableName, result, OnCacheRemoved);
            }

            return result;
        }
    }
}