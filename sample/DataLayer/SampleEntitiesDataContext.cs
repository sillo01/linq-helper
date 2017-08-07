using Guayaba.LinqHelper;
using System.Runtime.Caching;

namespace DataLayer
{
    public partial class SampleEntitiesDataContext
    {
        public static SampleEntitiesDataContext Current
        {
            get
            {
                SampleEntitiesDataContext ctx = (SampleEntitiesDataContext)MemoryCache.Default["DataContextDataContext"];
                if (ctx == null)
                {
                    ctx = new SampleEntitiesDataContext();
                    CacheItemPolicy policy = new CacheItemPolicy();
                    MemoryCache.Default.Set("DataContextDataContext", ctx, policy);
                }
                return ctx;
            }
        }
        public static void RegisterDependency()
        {
            LinqHelper.GenerateInstance(Current);
            LinqHelper.Instance.RegisterDependency();
        }
    }
}
