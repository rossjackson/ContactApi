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

        public async Task<bool> AddContactAsync(Contact newContact)
        {
            if (DuplicateEmail(newContact.ContactId, newContact.EmailAddress, _context.Contacts.ToList()))
                throw new ContactDataUpdateException("Duplicate email address.");

            _context.Contacts.Add(newContact);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> EditContactAsync(Contact updatedContact)
        {
            var contacts = _context.Contacts.ToList();

            var originalContact = GetContact(contacts, updatedContact.ContactId);

            if(originalContact == null)
                throw new ContactDataUpdateException("Contact not found.");

            if (DuplicateEmail(updatedContact.ContactId, updatedContact.EmailAddress, contacts))
                throw new ContactDataUpdateException("Duplicate email address.");

            MaintainContactStatusOnEdit(originalContact, updatedContact);

            _context.Contacts.AddOrUpdate(updatedContact);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> DeleteContactAsync(Guid contactId)
        {
            var contacts = _context.Contacts.ToList();

            var contactToDelete = GetContact(contacts, contactId);

            if (contactToDelete == null)
                throw new ContactDataUpdateException("Contact not found.");

            _context.Contacts.Remove(contactToDelete);
            return await _context.SaveChangesAsync() == 1;
        }

        public Contact GetContact(List<Contact> contacts, Guid contactIdToCheck)
        {
            return contacts.FirstOrDefault(c => c.ContactId == contactIdToCheck);

        }

        public bool DuplicateEmail(Guid contactIdToCheck, string emailAddressToCheck, List<Contact> contacts)
        {
            return contacts.Any(c => c.ContactId != contactIdToCheck && string.Equals(c.EmailAddress.Trim(),
                                         emailAddressToCheck.Trim(), StringComparison.InvariantCultureIgnoreCase));
        }

        public void MaintainContactStatusOnEdit(Contact originalContact, Contact updatedContact)
        {
            updatedContact.Status = originalContact.Status;
        }
    }
}
