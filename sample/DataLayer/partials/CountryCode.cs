using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guayaba.LinqHelper;

namespace DataLayer
{
    public partial class CountryCode : IDataEntity
    {
        public string Id { get { return IdCountryCode.ToString(); } }

        public static List<CountryCode> Cached
        {
            get
            {
                return LinqHelper.Instance.GetAllCached<CountryCode>();
            }
        }

        public CountryCode Get()
        {
            return LinqHelper.Instance.SelectByPK<CountryCode>(Id);
        }
        public CountryCode Save(bool submit = false)
        {
            return LinqHelper.Instance.InserOrUpdate(this, submit);
        }

        public async Task<CountryCode> GetAsync()
        {
            return await LinqHelper.Instance.SelectByPKAsync<CountryCode>(this.Id);
        }
    }
}
