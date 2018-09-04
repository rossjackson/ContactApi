using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data
{
    interface IContactDataSource
    {
        IQueryable<Contact> Contacts { get; }
    }
}
