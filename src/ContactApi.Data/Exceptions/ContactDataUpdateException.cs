using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.Data.Exceptions
{
    public class ContactDataUpdateException : Exception
    {
        public ContactDataUpdateException(string message) : base(message)
        {
            
        }
    }
}
