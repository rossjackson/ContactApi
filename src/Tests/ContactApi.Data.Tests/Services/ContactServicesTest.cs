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
        public void AddOrUpdateContactAsync_insert_contact()
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var originalContactsCount = contacts.Count;

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.AddOrUpdateContactAsync(It.IsAny<Contact>()))
                .Callback<Contact>(c => contacts.Add(c));

            contactServiceMock.Object.AddOrUpdateContactAsync(MockHelper.NewContact);

            var updatedContactsCount = contacts.Count;

            Assert.AreNotEqual(originalContactsCount, updatedContactsCount);
            Assert.That(originalContactsCount + 1 == updatedContactsCount);
        }

        [Test]
        public void AddOrUpdateContactAsync_update_contact()
        {
            var contacts = MockHelper.ContactTestCollection.ToList();
            var originalContactsCount = contacts.Count;
            var originalContactToUpdate = contacts.FirstOrDefault(c => c.ContactId == MockHelper.ContactIdToUpdate);

            var contactToUpdateData = new Contact
            {
                ContactId = MockHelper.ContactIdToUpdate,
                FirstName = "Peter",
                LastName = "Parker",
                EmailAddress = "peter.parker@gmail.com"
            };

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.AddOrUpdateContactAsync(It.IsAny<Contact>()))
                .Callback<Contact>(c =>
                {
                    var contactToAddOrUpdate = contacts.FirstOrDefault(contact => contact.ContactId == c.ContactId);
                    var contactToUpdateIndex = contacts.IndexOf(contactToAddOrUpdate);
                    contacts[contactToUpdateIndex] = c;
                });

            contactServiceMock.Object.AddOrUpdateContactAsync(contactToUpdateData);
            var updatedContactsCount = contacts.Count;
            var updatedContact = contacts.FirstOrDefault(c => c.ContactId == contactToUpdateData.ContactId);

            Assert.That(originalContactsCount == updatedContactsCount);
            Assert.AreNotEqual(originalContactToUpdate, updatedContact);
            Assert.AreEqual(contactToUpdateData, updatedContact);
        }

        [Test]
        public void AddOrUpdateContactAsync_duplicate_email()
        {
            var contactServiceMock = new Mock<ContactService>();
            var contacts = MockHelper.ContactTestCollection.ToList();

            Assert.True(contactServiceMock.Object.DuplicateEmail(MockHelper.DuplicateEmail, contacts));
            Assert.False(contactServiceMock.Object.DuplicateEmail("random@email.com", contacts));
        }
    }
}
