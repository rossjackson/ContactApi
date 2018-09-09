using System.Web.Http;
using ContactApi.Web.Common.Security;

namespace ContactApi.Web.Api.Controllers.V2
{
    [RoutePrefix("api/{apiVersion:apiVersionConstraint(v2)}/jwt")]
    [AllowAnonymous]
    public class JwtController : ApiController
    {
        [Route("generatetoken")]
        [HttpGet]
        public IHttpActionResult GenerateToken([FromUri]int expiresInMinutes)
        {
            if (expiresInMinutes == 0) return BadRequest("Expiration must be greater than 0.");

            var domain = Request?.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty) ??
                         "http://localhost:50602";

            var jwt = new JwtService(domain);
            return Ok(jwt.GenerateToken(expiresInMinutes));
        }
    }
}
