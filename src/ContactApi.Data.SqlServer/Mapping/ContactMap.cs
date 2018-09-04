using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data.SqlServer.Mapping
{
    public class ContactMap : VersionedClassMap<Contact>
    {
        public ContactMap()
        {
            Id(c => c.ContactId).Not.Nullable();
            Map(c => c.LastName).Not.Nullable();
            Map(c => c.EmailAddress).Not.Nullable().Index("IX_Contacts");
            Map(c => c.Status).Not.Nullable();
        }
    }
}
