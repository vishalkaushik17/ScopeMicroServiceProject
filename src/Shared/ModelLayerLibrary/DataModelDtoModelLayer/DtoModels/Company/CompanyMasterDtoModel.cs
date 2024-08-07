using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ModelTemplates.DtoModels.Company;

public sealed class CompanyMasterDtoModel : BaseDtoTemplate
{

    [Required(ErrorMessage = "User identity is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Name { get; set; }

    [Required(ErrorMessage = "User identity is required!")]
    [EmailAddress]
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string Email { get; set; }

    [Required(ErrorMessage = "User identity is required!")]
    public DateTime EnrollmentDate { get; set; }
    [Required(ErrorMessage = "User identity is required!")]
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string SuffixDomain { get; set; }

    [AllowNull]
    public DateTime DemoExpireDate { get; set; }
    public DateTime? AccountExpire { get; set; }

    public CompanyTypeDtoModel CompanyTypeEntityModel { get; set; }


    [Required(ErrorMessage = "User identity is required!")]
    public string CompanyTypeId { get; set; }

    [AllowNull]
    [StringLength(7, ErrorMessage = "Provided value exceed the required limit (7 Characters).")]
    public string ReferenceCode { get; set; } = "Default";

    [AllowNull]
    [StringLength(7, ErrorMessage = "Provided value exceed the required limit (7 Characters).")]
    public string ParentReferenceCode { get; set; } = "Default";

    [AllowNull]
    public bool IsDemoExpired { get; set; } = false;
    [AllowNull]
    public bool IsDemoMode { get; set; } = true;

    public CompanyMasterProfileDtoModel CompanyMasterProfile { get; set; }

}