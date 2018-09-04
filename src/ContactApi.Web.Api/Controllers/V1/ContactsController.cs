using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ContactApi.Data.Entities;
using ContactApi.Web.Common.Routing;
using Newtonsoft.Json;

namespace ContactApi.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("contacts")]
    public class ContactsController : ApiController
    {
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var contact = Contact.GetById(new Guid("3F976D52-3D65-43F3-B7A0-3CF0AE26C47B"));
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json")
            };
        }
    }
}
