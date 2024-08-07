using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

public enum EmailNotificationType : byte
{
    [Display(Name = "Login")]
    LOGINALERT = 1,

    [Display(Name = "OTP")]
    OTP = 2,
    [Display(Name = "Message")]
    MESSAGE = 3,

    [Display(Name = "Exception")]
    EXCEPTION = 4,

    [Display(Name = "Alert")]
    ALERT = 5,

    [Display(Name = "Test")]
    TEST = 6,

}