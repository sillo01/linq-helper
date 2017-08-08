using DataLayer;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LinqHelper_Sample.Controllers
{
    public class CountryController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string ISOshort)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                CountryCode.Cached
                    .Where(c => c.ISOshort.Equals(ISOshort, StringComparison.CurrentCultureIgnoreCase))
            );
        }
        [HttpGet]
        public HttpResponseMessage PK(int IdCountryCode)
        {
            var entity = new CountryCode() { IdCountryCode = IdCountryCode };
            return Request.CreateResponse(
                HttpStatusCode.OK,
                entity.Get()
            );
        }
        public HttpResponseMessage Post(CountryCode entity)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                entity.Save(true)
            );
        }
    }
}
