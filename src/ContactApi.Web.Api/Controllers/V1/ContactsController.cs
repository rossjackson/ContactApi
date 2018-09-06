using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ContactApi.Data.Entities;
using ContactApi.Data.Services;
using ContactApi.Web.Api.Models.V1;
using ContactApi.Web.Common.Routing;
using Newtonsoft.Json;

namespace ContactApi.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("contacts")]
    public class ContactsController : ApiController
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [Route("list")]
        [HttpGet]
        public IHttpActionResult List()
        {
            var contacts = _contactService.GetAll();
            return Ok(contacts);
        }

        [Route("add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddContact([FromBody]ContactModel contactModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var contact = MapContactModelToContactEntity(contactModel);

            await _contactService.AddOrUpdateContactAsync(contact);
            return Ok();
        }

        private Contact MapContactModelToContactEntity(ContactModel contactModel)
        {
            return new Contact
            {
                ContactId = Guid.NewGuid(),
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
                EmailAddress = contactModel.EmailAddress,
                PhoneNumber = contactModel.PhoneNumber,
                Status = "Inactive"
            };
        }
    }
}
