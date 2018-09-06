using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;
using Moq;

namespace ContactApi.Test.Utility
{
    public static class MockHelper
    {
        public static Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(IQueryable<TEntity> models) where TEntity : class
        {
            Mock<DbSet<TEntity>> dbSet = new Mock<DbSet<TEntity>>();

            dbSet.As<IQueryable<TEntity>>().Setup(e => e.ElementType).Returns(models.ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(e => e.Expression).Returns(models.Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(e => e.GetEnumerator()).Returns(models.GetEnumerator());
            dbSet.As<IQueryable<TEntity>>().Setup(e => e.Provider).Returns(models.Provider);

            return dbSet;
        }

        public static IQueryable<Contact> ContactTestCollection => new List<Contact>
        {
            new Contact
            {
                ContactId = new Guid("{78B15191-98D8-4250-9B5A-5DD52BCE71F2}"),
                FirstName = "Bat",
                LastName = "Man",
                EmailAddress = "batman@gmail.com",
                PhoneNumber = "201-456-8952",
                Status = "Active"
            },
            new Contact
            {
                ContactId = new Guid("{D7F34E7A-9AEC-474D-906B-626357AAA9A0}"),
                FirstName = "Bat",
                LastName = "Luther",
                EmailAddress = "lex.luther@gmail.com",
                PhoneNumber = "321-457-8542",
                Status = "Inactive"
            },
            new Contact
            {
                ContactId = new Guid("{58117087-6E6F-4BA6-B115-A04DE77C4E7F}"),
                FirstName = "Tim",
                LastName = "Drake",
                EmailAddress = "tim.drake@gmail.com",
                PhoneNumber = "713-989-8542",
                Status = "Active"
            }
        }.AsQueryable();

        public static Contact NewContact => new Contact
        {
            ContactId = Guid.NewGuid(),
            FirstName = "Clark",
            LastName = "Kent",
            EmailAddress = "clark.kent@gmail.com",
            Status = "Inactive",
            PhoneNumber = "202-460-0101"
        };
    }
}
