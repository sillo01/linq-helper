using Guayaba.LinqHelper;
using System.Linq;
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
                    string executionPath = System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
                        .Replace("file:\\", "")
                        .Replace("LinqHelper-Sample\\bin", "DataLayer");

                    string connectionString = string.Format(_connectionString, executionPath);

                    ctx = new SampleEntitiesDataContext(connectionString);
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

        private const string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0}\sampleDB.mdf;Integrated Security=True;Connect Timeout=30";
    }
}
