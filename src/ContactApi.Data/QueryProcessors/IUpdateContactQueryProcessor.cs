using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data.QueryProcessors
{
    public interface IUpdateContactQueryProcessor
    {
        //void DeleteContact(Guid contactId);
        //void ChangeStatus(Guid contactId, string status);
        //void UpdateContact(Contact contact);
        void InsertContact(Contact contact);
    }
}
