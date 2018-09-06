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
        List<Contact> GetAll();
        void AddContact(Contact newContact);
    }
}
