using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContactApi.Test.Utility;
using ContactApi.Web.Api.Models.V1;
using NUnit.Framework;

namespace ContactApi.Web.Api.Tests.Models.V1
{
    [TestFixture]
    public class ContactsModelsTest
    {
        private readonly ContactModel correctContactModel = new ContactModel
        {
            FirstName = "Clark",
            LastName = "Kent",
            EmailAddress = "clark@kent.com",
            PhoneNumber = "202-123-4567"
        };

        [Test]
        public void ContactModel_empty()
        {
            var contact1 = MockHelper.NewContact;
            contact1.FirstName = "samson";
            var contact2 = MockHelper.NewContact;
            var contact = contact2;
            var emptyContactModel = new ContactModel();
            Assert.IsFalse(ValidateContactModel(emptyContactModel));
        }

        [Test]
        public void ContactModel_correct_validation()
        {
            Assert.IsTrue(ValidateContactModel(correctContactModel));
        }

        [Test]
        public void ContactModel_required_lastname()
        {
            correctContactModel.LastName = string.Empty;
            Assert.IsFalse(ValidateContactModel(correctContactModel));
            correctContactModel.LastName = "Kent";
        }

        [Test]
        public void ContactModel_required_emailaddress()
        {
            correctContactModel.EmailAddress = string.Empty;
            Assert.IsFalse(ValidateContactModel(correctContactModel));
            correctContactModel.EmailAddress = "clark@kent.com";
        }

        [Test]
        public void ContactModel_email_validity()
        {
            correctContactModel.EmailAddress = "test";
            Assert.IsFalse(ValidateContactModel(correctContactModel));
            
            correctContactModel.EmailAddress = "hello@@";
            Assert.IsFalse(ValidateContactModel(correctContactModel));

            correctContactModel.EmailAddress = "www.at.com";
            Assert.IsFalse(ValidateContactModel(correctContactModel));

            correctContactModel.EmailAddress = "clark@kent.com";
            Assert.IsTrue(ValidateContactModel(correctContactModel));
        }

        [Test]
        public void ContactModel_max_length_test()
        {
            var stringOfLength100 =
                "fJ30NMFnnRWw5Rf3KiQIxb54wU9Fo1OnQCtD77O7i8Vla3IfnIq0C2l6CG5DebZb1oSiO1YYbUp1ahZflkrjIGySOlyKke3hQ73t";
            correctContactModel.FirstName = stringOfLength100;
            Assert.IsTrue(ValidateContactModel(correctContactModel));

            correctContactModel.FirstName += "a";
            Assert.IsFalse(ValidateContactModel(correctContactModel));
        }

        private bool ValidateContactModel(ContactModel contactModel)
        {
            var context = new ValidationContext(contactModel);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(contactModel, context, results, true);
        }
    }
}
