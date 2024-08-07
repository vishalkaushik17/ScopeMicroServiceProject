using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

public enum MaritalStatus : byte
{
    [Display(Name = "Married")]
    Male = 1,
    [Display(Name = "Unmarried")]
    Female = 2,
    [Display(Name = "Single/Divorce")]
    Single = 3,

}
