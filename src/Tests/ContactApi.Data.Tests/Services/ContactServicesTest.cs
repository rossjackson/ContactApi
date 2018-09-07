using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;
using ContactApi.Data.Services;
using ContactApi.Test.Utility;
using Moq;
using NUnit.Framework;

namespace ContactApi.Data.Tests.Services
{
    [TestFixture]
    public class ContactServicesTest
    {
        [Test]
        public void GetAll_via_context()
        {
            var expectedContactsQueryable = MockHelper.ContactTestCollection;
            var mockContactTestCollection = MockHelper.CreateMockDbSet(expectedContactsQueryable);

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.GetAll()).Returns(mockContactTestCollection.Object);

            var actualContacts = contactServiceMock.Object.GetAll().ToList();
            var expectedContacts = expectedContactsQueryable.ToList();

            Assert.IsTrue(actualContacts.Count == expectedContacts.Count);
            CollectionAssert.AreEqual(actualContacts, expectedContacts);
        }

        [Test]
        public void AddContactAsync()
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var originalContactsCount = contacts.Count;

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.AddContactAsync(It.IsAny<Contact>()))
                .Callback<Contact>(c => contacts.Add(c));

            contactServiceMock.Object.AddContactAsync(MockHelper.NewContact);

            var updatedContactsCount = contacts.Count;

            Assert.AreNotEqual(originalContactsCount, updatedContactsCount);
            Assert.That(originalContactsCount + 1 == updatedContactsCount);
        }

        [Test]
        public void EditContactAsync()
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var contactDataForUpdate = new Contact
            {
                ContactId = MockHelper.ContactIdToUpdate,
                FirstName = "The",
                LastName = "Drake",
                EmailAddress = "tim.drake@gmail.com",
                PhoneNumber = "713-989-8542",
                Status = "Active"
            };

            var originalContactsCount = contacts.Count;
            var originalContact = contacts.FirstOrDefault(c => c.ContactId == contactDataForUpdate.ContactId);
            
            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.EditContactAsync(It.IsAny<Contact>())).Callback<Contact>(c =>
            {
                var contactToUpdateIndex = contacts.FindIndex(thisContact => thisContact.ContactId == c.ContactId);
                contacts[contactToUpdateIndex] = c;
            });

            contactServiceMock.Object.EditContactAsync(contactDataForUpdate);

            var updatedContact = contacts.FirstOrDefault(c => c.ContactId == contactDataForUpdate.ContactId);

            Assert.IsTrue(contacts.Count == originalContactsCount);
            Assert.AreNotEqual(originalContact, updatedContact);
        }

        [Test]
        public void DeleteContactAsync()
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var contactIdToDelete = MockHelper.ContactIdToUpdate;

            var originalContactsCount = contacts.Count;

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.DeleteContactAsync(It.IsAny<Guid>())).Callback<Guid>(c =>
            {
                var contactToRemove = contacts.FirstOrDefault(thisContact => thisContact.ContactId == c);
                contacts.Remove(contactToRemove);
            });

            contactServiceMock.Object.DeleteContactAsync(contactIdToDelete);

            var actualContactsCount = contacts.Count;

            var deletedContact = contacts.FirstOrDefault(c => c.ContactId == contactIdToDelete);

            Assert.IsTrue(originalContactsCount - 1 == actualContactsCount);
            Assert.Null(deletedContact);
        }

        [TestCase("Active", "{D7F34E7A-9AEC-474D-906B-626357AAA9A0}", ExpectedResult = "Active", TestName = "UpdateStatus_Active")]
        [TestCase("Inactive", "{D7F34E7A-9AEC-474D-906B-626357AAA9A0}", ExpectedResult = "Inactive", TestName = "UpdateStatus_Inactive")]
        [TestCase("OtherWords", "{D7F34E7A-9AEC-474D-906B-626357AAA9A0}", ExpectedResult = "Inactive", TestName = "UpdateStatus_OtherWords")]
        [TestCase(null, "{D7F34E7A-9AEC-474D-906B-626357AAA9A0}", ExpectedResult = "Inactive", TestName = "UpdateStatus_Null")]
        public string UpdateStatusAsync(string toUpdateStatus, string contactIdToUpdateString)
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var contactIdToUpdate = new Guid(contactIdToUpdateString);

            var originalContactsCount = contacts.Count;

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<string>())).Callback<Guid, string>((contactId, status) =>
            {
                var contactToUpdateStatus = contacts.FirstOrDefault(c => c.ContactId == contactId);
                if (contactToUpdateStatus == null) return;
                contactToUpdateStatus.Status = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase) ? "Active" : "Inactive";
            });

            contactServiceMock.Object.UpdateStatusAsync(contactIdToUpdate, toUpdateStatus);
        
            Assert.IsTrue(contacts.Count == originalContactsCount);

            return contacts.FirstOrDefault(c => c.ContactId == contactIdToUpdate)?.Status;
        }

        [TestCase("{6D3741D2-9E21-42AC-AC7F-B471C77FDB1E}", ExpectedResult = null, TestName = "GetContact_unknown_contact")]
        [TestCase("{78B15191-98D8-4250-9B5A-5DD52BCE71F2}", ExpectedResult = "batman@gmail.com", TestName = "GetContact_known_contact")]
        public string GetContact(string strContactId)
        {
            var contacts = MockHelper.ContactTestCollection.ToList();

            var contactServiceMock = new Mock<ContactService>();

            var actualContact = contactServiceMock.Object.GetContact(contacts, new Guid(strContactId));
            return actualContact?.EmailAddress;
        }
        
        [Test]
        public void DuplicateEmail()
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var newContactWithDuplicateEmail = new Contact
            {
                ContactId = Guid.NewGuid(),
                EmailAddress = MockHelper.DuplicateEmail
            };

            var newContactWithUniqueEmail = new Contact
            {
                ContactId = Guid.NewGuid(),
                EmailAddress = "random@email.com"
            };

            var contactServiceMock = new Mock<ContactService>();

            Assert.True(contactServiceMock.Object.DuplicateEmail(newContactWithDuplicateEmail.ContactId,
                newContactWithDuplicateEmail.EmailAddress, contacts));
            Assert.False(contactServiceMock.Object.DuplicateEmail(newContactWithUniqueEmail.ContactId,
                newContactWithUniqueEmail.EmailAddress, contacts));
        }

        [Test]
        public void MaintainContactStatusOnEdit()
        {
            var originalContact = new Contact { Status = "Active" };
            var updatedContact = new Contact();

            Assert.True(updatedContact.Status == null);
            
            var contactServiceMock = new Mock<ContactService>();
            contactServiceMock.Object.MaintainContactStatusOnEdit(originalContact, updatedContact);

            Assert.AreEqual(originalContact.Status, updatedContact.Status);
        }
    }
}
