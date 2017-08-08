using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer;
using System.Threading.Tasks;

namespace LinqHelper_Sample.Controllers
{
    public class CountryAsyncController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> PK(int IdCountryCode)
        {
            var entity = new CountryCode() { IdCountryCode = IdCountryCode };
            return Request.CreateResponse(
                HttpStatusCode.OK,
                await entity.GetAsync()
            );
        }
    }
}
