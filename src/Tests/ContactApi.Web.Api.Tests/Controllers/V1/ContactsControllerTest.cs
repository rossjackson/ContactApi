using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web.ModelBinding;
using ContactApi.Data.Entities;
using ContactApi.Data.Services;
using ContactApi.Test.Utility;
using ContactApi.Web.Api.Controllers.V1;
using ContactApi.Web.Api.Models.V1;
using Moq;
using NUnit.Framework;

namespace ContactApi.Web.Api.Tests.Controllers.V1
{
    [TestFixture]
    public class ContactsControllerTest
    {
        private readonly Mock<IContactService> _mockContactService;
        private readonly ContactsController _contactsController;

        public ContactsControllerTest()
        {
            _mockContactService = new Mock<IContactService>();
            _contactsController = new ContactsController(_mockContactService.Object);
        }

        [Test]
        public void List()
        {
            var actionResult = _contactsController.List();

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<Contact>>>(actionResult);
        }

        [Test]
        public async Task AddContactAsync()
        {
            var actionResult = await _contactsController.AddContact(new ContactModel());
            Assert.IsInstanceOf<CreatedNegotiatedContentResult<Contact>>(actionResult);
        }
    }
}
