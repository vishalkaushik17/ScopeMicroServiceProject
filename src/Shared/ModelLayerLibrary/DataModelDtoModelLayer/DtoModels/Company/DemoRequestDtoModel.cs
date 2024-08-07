using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ModelTemplates.DtoModels.Company
{
    public sealed class DemoRequestDtoModel : BaseDtoTemplate
    {
        [Required(ErrorMessage = "Institute name is required!")]
        [StringLength(100, ErrorMessage = "Provided value exceed the required limit.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit.")]
        public string LastName { get; set; } = string.Empty;





        [Url]
        [Required(ErrorMessage = "Website address is required!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit.")]
        public string Website { get; set; } = string.Empty;


        [EmailAddress]
        [Required(ErrorMessage = "Email address is required!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit.")]
        public string Email { get; set; } = string.Empty;



        [Required(ErrorMessage = "Contact number is required!")]
        [StringLength(20, ErrorMessage = "Provided value exceed the required limit.")]
        //[RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$.",ErrorMessage ="Not a valid format for contact no.")]
        public string ContactNo { get; set; } = string.Empty;


        [Required(ErrorMessage = "Location City is required!")]
        [StringLength(20, ErrorMessage = "Provided value exceed the required limit.")]
        public string City { get; set; } = string.Empty;


        [AllowNull]
        [StringLength(7, ErrorMessage = "Provided value exceed the required limit.")]
        public string ReferenceCode { get; set; } = "Default";

        [AllowNull]
        public bool IsDemoActivated { get; set; } = false;
        [AllowNull]
        public bool IsRestrictedForDemo { get; set; } = false;

        public DateTime? DemoActivatedOn { get; set; }

    }
}
