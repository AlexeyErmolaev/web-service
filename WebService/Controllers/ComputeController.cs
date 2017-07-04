using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    public class ComputeController : ApiController
    {
        public IHttpActionResult Get(string x)
        {
            var computeService = new ComputeService();
            var result = computeService.Calculate(x);
            return this.Json(result);
        }
    }
}
