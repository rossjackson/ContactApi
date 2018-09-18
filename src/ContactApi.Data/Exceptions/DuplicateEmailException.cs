using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.Data.Exceptions
{
    public class DuplicateEmailException : ContactDataUpdateException
    {
        public DuplicateEmailException() : base("Duplicate email addresss.")
        {
        }

        public DuplicateEmailException(string message) : base(message)
        {
            
        }
    }
}
