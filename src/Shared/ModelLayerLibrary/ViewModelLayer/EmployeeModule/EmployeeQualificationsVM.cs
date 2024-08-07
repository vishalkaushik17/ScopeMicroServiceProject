using System.ComponentModel.DataAnnotations;

namespace VModelLayer.EmployeeModule;

/// <summary>
/// this dto model is qualification master table which consist of employee qualification list
/// in which year employee has taken degree
/// </summary>
public class EmployeeQualificationsVM : BaseVMTemplate
{
    [Required(ErrorMessage = "Degree information is required!")]
    public string DegreeId { get; set; }
    public DegreeVM DegreeMaster { get; set; }

    [Required(ErrorMessage = "Completion month information is required!")]
    public DateTime MonthOfCompletion { get; set; }

    [Required(ErrorMessage = "Completion year information is required!")]
    public DateTime YearOfCompletion { get; set; }

    [Required(ErrorMessage = "Obtain grade information is required!")]
    public string Grade { get; set; }

    [Required(ErrorMessage = "Employee information is required!")]
    public string EmployeeParentId { get; set; }
    public EmployeeStudentParentVM EmployeeParentMaster { get; set; }
}
