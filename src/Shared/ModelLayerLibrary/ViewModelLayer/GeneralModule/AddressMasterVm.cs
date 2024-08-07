using System.ComponentModel.DataAnnotations;

namespace VModelLayer.GeneralModule;

public class AddressMasterVm : BaseVMTemplate
{
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Address1 { get; set; } = DefaultStringValue;

    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Address2 { get; set; } = DefaultStringValue;

    [StringLength(60, ErrorMessage = "Provided value exceed the required limit (60 Characters).")]
    public string Milestone { get; set; } = DefaultStringValue;

    [StringLength(60, ErrorMessage = "Provided value exceed the required limit (60 Characters).")]
    public string Area { get; set; } = DefaultStringValue;

    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string City { get; set; } = DefaultStringValue;

    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string State { get; set; } = DefaultStringValue;

    [StringLength(10, ErrorMessage = "Provided value exceed the required limit (10 Characters).")]
    public string Pincode { get; set; } = DefaultStringValue;

    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string Country { get; set; } = DefaultStringValue;

}