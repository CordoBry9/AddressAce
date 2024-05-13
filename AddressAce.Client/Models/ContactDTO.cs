using AddressAce.Client.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AddressAce.Client.Helpers;

namespace AddressAce.Client.Models
{
    public class ContactDTO
    {
        private DateTimeOffset? _birthDate;
        private DateTimeOffset _created;

        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]

        public string? LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]

        public DateTimeOffset? BirthDate
        {
            get { return _birthDate?.ToLocalTime(); }
            set { _birthDate = value?.ToUniversalTime(); }
        }

        [Required]
        [Display(Name = "Address")]
        public string? Address1 { get; set; }


        [Required]
        [Display(Name = "Address2")]
        public string? Address2 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }

        [Required]
        [EmailAddress]

        public string? Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]

        public string? PhoneNumber { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }

        public virtual ICollection<CategoryDTO> Categories { get; set; } = new HashSet<CategoryDTO>();

        public string ImageUrl { get; set; } = ImageHelper.DefaultContactImage;

        //TODO: what about DTOS?
    }
}
