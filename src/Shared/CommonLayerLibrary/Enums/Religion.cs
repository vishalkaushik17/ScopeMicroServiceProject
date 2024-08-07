using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

public enum Religion : byte
{
    [Display(Name = "Hindu")]
    Hindu = 1,
    [Display(Name = "Muslim")]
    Muslim = 2,
    [Display(Name = "Christian")]
    Christian = 3,
    [Display(Name = "Sikh")]
    Sikh = 4,
    [Display(Name = "Buddhist")]
    Buddhist = 5,
    [Display(Name = "Sarnaism")]
    Sarnaism = 6,
    [Display(Name = "Jain")]
    Jain = 7,
    [Display(Name = "Koyapunem")]
    Koyapunem = 8,
    [Display(Name = "Other")]
    Other = 9,

}
