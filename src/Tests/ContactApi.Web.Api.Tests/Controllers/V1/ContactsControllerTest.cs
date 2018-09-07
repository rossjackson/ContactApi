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
            var contactIdToEdit = Guid.Empty;
            var actionResult = await _contactsController.EditContactAsync(contactIdToEdit, new ContactModel());
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }
        
        [Test]
        public async Task EditContactAsync_null_contact()
        {
            var contactIdToEdit = Guid.NewGuid();
            var actionResult = await _contactsController.EditContactAsync(contactIdToEdit, null);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }

        [Test]
        public async Task EditContactAsync_correct_contactid()
        {
            var contactIdToEdit = Guid.NewGuid();
            var actionResult = await _contactsController.EditContactAsync(contactIdToEdit, new ContactModel());
            Assert.IsInstanceOf<ResponseMessageResult>(actionResult);
        }

        [Test]
        public async Task DeleteContactAsync_missing_contactid()
        {
            var contactIdToDelete = Guid.Empty;
            var actionResult = await _contactsController.DeleteContactAsync(contactIdToDelete);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }

        [Test]
        public async Task DeleteContactAsync_correct_contactid()
        {
            var contactIdToDelete = Guid.NewGuid();
            var actionResult = await _contactsController.DeleteContactAsync(contactIdToDelete);
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public async Task UpdateStatusAsync_missing_contactid()
        {
            var contactIdToUpdate = Guid.Empty;
            var status = "Active";
            var actionResult = await _contactsController.UpdateStatusAsync(contactIdToUpdate, status);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }

        [Test]
        public async Task UpdateStatusAsync_correct_contactid()
        {
            var contactIdToUpdate = Guid.NewGuid();
            var status = "Active";
            var actionResult = await _contactsController.UpdateStatusAsync(contactIdToUpdate, status);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Contact>>(actionResult);
        }
    }
}
