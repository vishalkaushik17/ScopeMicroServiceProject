using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// this model represents address of the employee/students/parents/vendors etc.
/// </summary>
public class AddressDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Address line 1 information is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Address1 { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address line 2 information is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Address2 { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address milestone line 1 information is required!")]
    [StringLength(60, ErrorMessage = "Provided value exceed the required limit (60 Characters).")]
    public string Milestone { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address area information is required!")]
    [StringLength(60, ErrorMessage = "Provided value exceed the required limit (60 Characters).")]
    public string Area { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address city information is required!")]
    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string City { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address state information is required!")]
    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string State { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address pincode information is required!")]
    [StringLength(10, ErrorMessage = "Provided value exceed the required limit (10 Characters).")]
    public string Pincode { get; set; } = DefaultStringValue;

    [Required(ErrorMessage = "Address country information is required!")]
    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string Country { get; set; } = DefaultStringValue;
}
