using System;
using System.Web.Http;

namespace WebService.Controllers
{
    public class ComputeController : ApiController
    {
        public IHttpActionResult Get(string x)
        {
            try
            {
                var result = ComputeService.Calculate(x);
                return this.Json(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
