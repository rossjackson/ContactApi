using System.ComponentModel.DataAnnotations;

namespace ContactApi.Web.Api.Models.V1
{
    public class ContactModel
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(255)]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}