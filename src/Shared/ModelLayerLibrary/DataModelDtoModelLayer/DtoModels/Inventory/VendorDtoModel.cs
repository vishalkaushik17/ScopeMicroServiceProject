using GenericFunction.Enums;
using ModelTemplates.CustomValidations;
using ModelTemplates.DtoModels.BaseDtoContract;
using ModelTemplates.DtoModels.Employee;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Inventory
{
    /// <summary>
    /// Vendor Dto model
    /// </summary>
    public class VendorDtoModel : BaseDtoTemplate
    {
        [Required(ErrorMessage = "Company name is required!")]
        [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
        public string CompanyName { get; set; } = DefaultStringValue;

        //[Required(ErrorMessage = "GST no is required!")]
        [ValidateGSTCSTNo(ErrorMessage = "Invalid GST No!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
        public string GSTNo { get; set; } = DefaultStringValue;

        //[Required(ErrorMessage = "CST no is required!")]
        [ValidateGSTCSTNo(ErrorMessage = "Invalid CST No!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
        public string CSTNo { get; set; } = DefaultStringValue;


        [Required(ErrorMessage = "Contact person is required!")]
        [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
        public string ContactPerson { get; set; } = DefaultStringValue;

        [Required(ErrorMessage = "Contact no is required!")]
        [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
        public string ContactNo { get; set; } = DefaultStringValue;

        [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
        public string EmailId { get; set; } = DefaultStringValue;

        [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
        public string Website { get; set; } = DefaultStringValue;

        public EnumVendorTypes Type { get; set; } = EnumVendorTypes.Vendor;

        public string AddressId { get; set; } = string.Empty;
        public AddressDtoModel Address { get; set; }// = new AddressMasterDtoModel();
    }
}
