using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using ContactApi.Data.Entities;
using ContactApi.Data.Services;
using ContactApi.Web.Api.Models.V1;
using ContactApi.Web.Common.Routing;
using Newtonsoft.Json;

namespace ContactApi.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("contacts")]
    [Authorize]
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
        public async Task<IHttpActionResult> AddContactAsync([FromBody]ContactModel contactModel)
        {
            if (!ModelStateValid(ModelState, out var badRequest)) return badRequest;
            
            var contact = MapContactModelToContactEntity(contactModel, status: "Inactive");
            await _contactService.AddContactAsync(contact);

            return Created(Request?.RequestUri.ToString() ?? "/", contact);
        }

        [Route("edit")]
        [HttpPut]
        public async Task<IHttpActionResult> EditContactAsync([FromUri]Guid contactId, [FromBody]ContactModel contactModel)
        {
            if (!EditContactValid(contactId, contactModel, ModelState, out var badRequest)) return badRequest;

            var contact = MapContactModelToContactEntity(contactModel, contactId);
            await _contactService.EditContactAsync(contact);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteContactAsync([FromUri] Guid contactId)
        {
            if (!ContactIdValid(contactId, out var badRequest)) return badRequest;

            await _contactService.DeleteContactAsync(contactId);
            return Ok();
        }

        [Route("updatestatus")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateStatusAsync([FromUri] Guid contactId, [FromUri] string status)
        {
            if (!ContactIdValid(contactId, out var badRequest)) return badRequest;

            var contact = await _contactService.UpdateStatusAsync(contactId, status);
            return Ok(contact);
        }

        private Contact MapContactModelToContactEntity(ContactModel contactModel, Guid contactId = default(Guid),
            string status = default(string))
        {
            return new Contact
            {
                ContactId = contactId == Guid.Empty ? Guid.NewGuid() : contactId,
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
                EmailAddress = contactModel.EmailAddress,
                PhoneNumber = contactModel.PhoneNumber,
                Status = status
            };
        }

        private bool EditContactValid(Guid contactId, ContactModel contactModel, ModelStateDictionary modelState, out IHttpActionResult badRequest)
        {
            if (!ContactIdValid(contactId, out badRequest)) return false;
            if (!ContactModelValid(contactModel, out badRequest)) return false;
            if (!ModelStateValid(modelState, out badRequest)) return false;

            badRequest = null;
            return true;
        }

        private bool ModelStateValid(ModelStateDictionary modelState, out IHttpActionResult badRequest)
        {
            badRequest = modelState.IsValid ? null : BadRequest(modelState);
            return badRequest == null;
        }

        private bool ContactModelValid(ContactModel contactModel, out IHttpActionResult badRequest)
        {
            badRequest = contactModel == null ? BadRequest("Please use delete to remove the contact.") : null;
            return badRequest == null;
        }

        private bool ContactIdValid(Guid contactId, out IHttpActionResult badRequest)
        {
            badRequest = contactId == Guid.Empty ? BadRequest("ContactId is required.") : null;
            return badRequest == null;
        }
    }
}
