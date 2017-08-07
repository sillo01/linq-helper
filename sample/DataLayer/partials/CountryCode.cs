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

        public List<CountryCode> Cached
        {
            get
            {
                return LinqHelper.Instance.GetAllCached<CountryCode>();
            }
        }
    }
}
