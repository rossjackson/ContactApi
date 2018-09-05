using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ContactApi.Data;
using ContactApi.Data.Entities;
using ContactApi.Data.Services;
using Moq;
using NUnit.Framework;

namespace ContactApi.Web.Api.Tests.Controllers.V1
{
    [TestFixture]
    public class ContactsControllerTest
    {
        [Test]
        public void GetAll_Via_Context()
        {
            var data = new List<Contact>
            {
                new Contact {FirstName = "Test 1", LastName = "Test 2", EmailAddress = "test@test.com"},
                new Contact {FirstName = "second", LastName = "test", EmailAddress = "test2@test.com"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Contact>>();
            mockSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ContactApiDb>();
            mockContext.Setup(c => c.Contacts).Returns(mockSet.Object);

            var service = new ContactService(mockContext.Object);
            var contacts = service.GetAll();

            Assert.AreEqual(2, contacts.Count);
            Assert.AreEqual("Test 1", contacts[0].FirstName);

        }
    }
}
