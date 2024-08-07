using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

/// <summary>
/// Type of employment
/// </summary>
public enum EmploymentType : byte
{
    [Display(Name = "Permanent")]
    Permanent = 1,
    [Display(Name = "Contract Based")]
    Contract = 2,
    [Display(Name = "Full-time employment")]
    FullTimeEmployment = 3,
    [Display(Name = "Part-time employment")]
    PartTimeEmployment = 4,
    [Display(Name = "Apprenticeship")]
    Apprenticeship = 5,
    [Display(Name = "Trainee")]
    Traineeship = 6,
    [Display(Name = "Internship")]
    Internship = 7,
    [Display(Name = "Employment on commission")]
    EmploymentOnCommission = 8,
    [Display(Name = "Probation")]
    Probation = 9,
    [Display(Name = "Seasonal employment")]
    SeasonalEmployment = 10,
    [Display(Name = "Leased employment")]
    LeasedEmployment = 11,
    [Display(Name = "Contingent employment")]
    ContingentEmployment = 12,
}
