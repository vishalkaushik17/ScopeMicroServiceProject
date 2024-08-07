using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// this model represent name of bank, in which employee/student/parents has its account
/// In UI it will shows as list 
/// </summary>
public class BankDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Bank name is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Name { get; set; }
}
