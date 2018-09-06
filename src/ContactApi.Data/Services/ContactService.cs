using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;
using ContactApi.Data.Exceptions;

namespace ContactApi.Data.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactApiDb _context = new ContactApiDb();
        
        public virtual IEnumerable<Contact> GetAll()
        {
            return _context.Contacts;
        }

        public async Task<bool> AddOrUpdateContactAsync(Contact newContact)
        {
            if (DuplicateEmail(newContact.EmailAddress, _context.Contacts.ToList()))
                throw new ContactDataUpdateException("Duplicate email address.");

            _context.Contacts.AddOrUpdate(newContact);
            return await _context.SaveChangesAsync() != 0;
        }

        public bool DuplicateEmail(string emailAddress, List<Contact> contacts)
        {
            return contacts.Any(c => string.Equals(c.EmailAddress.Trim(), emailAddress.Trim(),
                StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
