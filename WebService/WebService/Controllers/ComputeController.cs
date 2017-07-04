using System.Web.Http;

namespace WebService.Controllers
{
    public class ComputeController : ApiController
    {
        public IHttpActionResult Get(string x)
        {
            int result = ComputeService.Calculate(x);
            return this.Json(result);
        }
    }
}
