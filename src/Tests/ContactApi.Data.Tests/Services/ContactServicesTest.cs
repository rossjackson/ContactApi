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
        public void AddContact_insert_new_contact()
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
    }
}
