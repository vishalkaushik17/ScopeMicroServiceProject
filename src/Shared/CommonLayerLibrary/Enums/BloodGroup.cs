using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

public enum BloodGroup : byte
{
    [Display(Name = "A RhD positive (A+)")]
    Apositive = 101,
    [Display(Name = "A RhD negative (A-)")]
    Anegative,
    [Display(Name = "B RhD positive (B+)")]
    Bpositive,
    [Display(Name = "B RhD negative (B-)")]
    Bnegative,
    [Display(Name = "O RhD positive (O+)")]
    Opositive,
    [Display(Name = "O RhD negative (O-)")]
    Onegative,
    [Display(Name = "AB RhD positive (AB+)")]
    ABpositive,
    [Display(Name = "AB RhD negative (AB-)")]
    ABnegative,
    //[Display(Name = "B+")]
    //BPve = 1,
    //[Display(Name = "B-")]
    //BNeg = 2,
    //[Display(Name = "O+")]
    //OPve = 3,
    //[Display(Name = "O-")]
    //ONeg = 4,
    //[Display(Name = "A+")]
    //APve = 5,
    //[Display(Name = "A-")]
    //ANeg = 5,
    //[Display(Name = "AB+")]
    //ABPve = 5,
    //[Display(Name = "AB-")]
    //ABNeg = 5,
}
