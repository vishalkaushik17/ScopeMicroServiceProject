using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

public enum Gender : byte
{
    [Display(Name = "Male")]
    Male = 1,
    [Display(Name = "Female")]
    Female,
    [Display(Name = "Not Specified")]
    NotSpecified,
}
