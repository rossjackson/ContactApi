using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ContactApi.Web.Api.Controllers
{
    public abstract class WebApiControllerBase : ApiController
    {
        [Route("{*actionName}")]
        [AcceptVerbs("GET", "POST", "PUT", "DELETE", "PATCH")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual HttpResponseMessage HandleUnknownAction(string actionName)
        {
            var status = HttpStatusCode.NotFound;
            return Request.CreateResponse(status);
        }
    }
}