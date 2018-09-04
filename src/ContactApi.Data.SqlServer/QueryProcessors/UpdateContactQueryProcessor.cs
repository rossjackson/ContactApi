using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;
using ContactApi.Data.Exceptions;
using ContactApi.Data.QueryProcessors;
using NHibernate;

namespace ContactApi.Data.SqlServer.QueryProcessors
{
    public class UpdateContactQueryProcessor : IUpdateContactQueryProcessor
    {
        private readonly ISession _session;

        public UpdateContactQueryProcessor(ISession session)
        {
            _session = session;
        }

        public void DeleteContact(Guid contactId)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(Guid contactId, string status)
        {
            throw new NotImplementedException();
        }

        public void UpdateContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public void InsertContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public virtual Contact GetContact(Guid contactId)
        {
            var contact = _session.Get<Contact>(contactId);
            if (contact == null)
            {
                throw new RootObjectNotFoundException("Contact not found.");
            }

            return contact;
        }
    }
}
