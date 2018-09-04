using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data.Entities
{
    public class Contact : IVersionedEntity
    {
        public virtual Guid ContactId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Status { get; set; }

        public virtual byte[] Version { get; set; }

        public static Contact GetById(Guid contactId)
        {
            using (var context = new ContactApiDb())
            {
                return context.Contacts.FirstOrDefault(c => c.ContactId == contactId);
            }
        }
    }
}
