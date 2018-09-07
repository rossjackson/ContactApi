using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        private readonly ContactsController _contactsController;

        public ContactsControllerTest()
        {
            var mockContactService = new Mock<IContactService>();
            _contactsController = new ContactsController(mockContactService.Object);
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
            var actionResult = await _contactsController.AddContactAsync(new ContactModel());
            Assert.IsInstanceOf<CreatedNegotiatedContentResult<Contact>>(actionResult);
        }

        [Test]
        public async Task EditContactAsync_missing_contactid()
        {
            var actionResult = await _contactsController.EditContactAsync(Guid.Empty, new ContactModel());
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }
        
        [Test]
        public async Task EditContactAsync_null_contact()
        {
            var actionResult = await _contactsController.EditContactAsync(Guid.NewGuid(), null);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }

        [Test]
        public async Task EditContactAsync_correct_contactid()
        {
            var actionResult = await _contactsController.EditContactAsync(Guid.NewGuid(), new ContactModel());
            Assert.IsInstanceOf<ResponseMessageResult>(actionResult);
        }
    }
}
