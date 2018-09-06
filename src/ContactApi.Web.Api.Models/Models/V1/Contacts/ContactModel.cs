using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.Web.Api.Models.Models.V1.Contacts
{
    public class ContactModel
    {
        public Guid ContactId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
