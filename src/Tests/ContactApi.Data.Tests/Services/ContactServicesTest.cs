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
        private IQueryable<Contact> ContactTestQueryableCollection;
        private Contact NewContact;

        [SetUp]
        public void SetUp()
        {
            ContactTestQueryableCollection = GenerateContactTestCollection();
            NewContact = new Contact
            {
                ContactId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Santos",
                EmailAddress = "John.Santos@gmail.com",
                Status = "Active",
                PhoneNumber = "202-460-0101"
            };
        }

        private IQueryable<Contact> GenerateContactTestCollection()
        {
            return new List<Contact>
            {
                new Contact {FirstName = "Test 1", LastName = "Test 2", EmailAddress = "test@test.com"},
                new Contact {FirstName = "second", LastName = "test", EmailAddress = "test2@test.com"}
            }.AsQueryable();
        }
        
        [Test]
        public void GetAll_via_context()
        {
            var mockContactTestCollection = MockHelper.CreateMockDbSet(ContactTestQueryableCollection);

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.GetAll()).Returns(mockContactTestCollection.Object.ToList);

            var contacts = contactServiceMock.Object.GetAll();

            Assert.IsTrue(contacts.Count == 2);
            Assert.That(contacts[0], Is.EqualTo(ContactTestQueryableCollection.ToList()[0]));
            Assert.AreEqual("Test 1", contacts[0].FirstName);
            Assert.AreEqual("test2@test.com", contacts[1].EmailAddress);
        }

        [Test]
        public void AddContact_insert_new_contact()
        {
            var contacts = ContactTestQueryableCollection.ToList();
            var originalContactsCount = contacts.Count;

            var contactServiceMock = new Mock<IContactService>();
            contactServiceMock.Setup(c => c.AddContact(It.IsAny<Contact>()))
                .Callback<Contact>(c => contacts.Add(c));

            contactServiceMock.Object.AddContact(NewContact);

            var updatedContactsCount = contacts.Count;

            Assert.AreNotEqual(originalContactsCount, updatedContactsCount);
            Assert.That(originalContactsCount + 1 == updatedContactsCount);

        }
    }
}
