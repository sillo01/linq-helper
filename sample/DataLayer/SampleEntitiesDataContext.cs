using Guayaba.LinqHelper;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

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
