using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// this dto model is qualification master table which consist of employee qualification list
/// in which year employee has taken degree
/// </summary>
public class EmployeeQualificationDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Degree information is required!")]
    public string DegreeId { get; set; }
    public DegreeDtoModel DegreeMaster { get; set; }

    [Required(ErrorMessage = "Completion month information is required!")]
    public DateTime MonthOfCompletion { get; set; }

    [Required(ErrorMessage = "Completion year information is required!")]
    public DateTime YearOfCompletion { get; set; }

    [Required(ErrorMessage = "Obtain grade information is required!")]
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]
    public string Grade { get; set; }

    [Required(ErrorMessage = "Employee information is required!")]
    public string EmployeeParentId { get; set; }
    public EmployeeStudentParentDtoModel EmployeeParentMaster { get; set; }
}
