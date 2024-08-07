using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ModelTemplates.CustomValidations
{
    public class ValidateGSTCSTNo : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not string str)
                return false;

            if (string.IsNullOrEmpty(str))
                str = "n/a";

            string strRegex = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(str) || str.ToUpper() == ("n/a").ToUpper() || string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            else
                return false;
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
