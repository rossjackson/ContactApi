using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data.Services
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Task<bool> AddContactAsync(Contact newContact);
        Task<bool> EditContactAsync(Contact updatedContact);
        Task<bool> DeleteContactAsync(Guid contactId);
        Task<Contact> UpdateStatusAsync(Guid contactId, string status);
    }
}
