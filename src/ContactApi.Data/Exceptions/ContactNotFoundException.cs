using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.Data.Exceptions
{
    public class ContactNotFoundException : ContactDataUpdateException
    {
        public ContactNotFoundException() : base("Contact not found.")
        {
        }

        public ContactNotFoundException(string message) : base(message)
        {
            
        }
    }
}
