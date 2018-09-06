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
        private readonly ContactApiDb _context;
        
        public ContactService(ContactApiDb context)
        {
            _context = context;
        }

        public virtual List<Contact> GetAll()
        {
            return _context.Contacts.ToList();
        }

        public void AddContact(Contact newContact)
        {
            _context.Contacts.Add(newContact);
            _context.Contacts.AddOrUpdate();
        }
    }
}
