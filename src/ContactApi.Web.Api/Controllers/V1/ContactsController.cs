using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ContactApi.Data;
using ContactApi.Data.Entities;
using ContactApi.Data.Services;
using ContactApi.Web.Common.Routing;
using Newtonsoft.Json;

namespace ContactApi.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("contacts")]
    public class ContactsController : ApiController
    {
        [Route("list")]
        [HttpGet]
        public HttpResponseMessage List()
        {
            var contactService = new ContactService(new ContactApiDb());
            var contacts = contactService.GetAll();
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json")
            };
        }
    }
}
