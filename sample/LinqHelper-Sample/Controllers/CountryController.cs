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
        public HttpResponseMessage Get(string ISOshort)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                CountryCode.Cached
                    .Where(c => c.ISOshort.Equals(ISOshort, StringComparison.CurrentCultureIgnoreCase))
            );
        }
    }
}
