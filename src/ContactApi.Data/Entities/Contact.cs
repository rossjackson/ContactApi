using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApi.Data.Entities;

namespace ContactApi.Data.Entities
{
    public class Contact
    {
        public virtual Guid ContactId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Status { get; set; }
    }
}
