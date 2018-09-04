using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ContactApi.Data.Entities;

namespace ContactApi.Data
{
    public class ContactApiDb : DbContext, IContactDataSource
    {
        public DbSet<Contact> Contacts { get; set; }

        IQueryable<Contact> IContactDataSource.Contacts => Contacts;
    }
}
