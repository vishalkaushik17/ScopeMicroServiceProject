using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.CustomValidations
{
    /// <summary>
    /// Validate date format
    /// </summary>
    public class ValidteDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateTime dt)
                return new ValidationResult(ErrorMessage ?? "Invalid date!");

            return ValidationResult.Success;

        }

    }
    //public class Min18Years : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var student = (Student)validationContext.ObjectInstance;

    //        if (student.DateofBirth == null)
    //            return new ValidationResult("Date of Birth is required.");

    //        var age = DateTime.Today.Year - student.DateofBirth.Year;

    //        return (age >= 18)
    //            ? ValidationResult.Success
    //            : new ValidationResult("Student should be at least 18 years old.");
    //    }
    //}
}
