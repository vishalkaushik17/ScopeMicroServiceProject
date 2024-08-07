using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

/// <summary>
/// Company types enum collection
/// </summary>
public enum EnumCompanyTypes : byte
{
    [Display(Name = "MASTER")]
    MASTER = 1,
    [Display(Name = "AGENCY")]
    AGENCY = 2,
    [Display(Name = "SUB AGENCY")]
    SUBAGENCY = 3,
    [Display(Name = "SCHOOL")]
    SCHOOL = 4
}
