using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactApiDb _context = new ContactApiDb();
        
        public virtual IEnumerable<Contact> GetAll()
        {
            return _context.Contacts;
        }

        public async Task<int> AddOrUpdateContactAsync(Contact newContact)
        {
            _context.Contacts.AddOrUpdate(newContact);
            return await _context.SaveChangesAsync();
        }

    }
}
