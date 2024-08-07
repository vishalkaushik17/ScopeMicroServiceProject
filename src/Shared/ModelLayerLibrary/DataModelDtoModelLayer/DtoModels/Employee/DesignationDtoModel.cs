using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// this dto class represents designations which is used to represents employee designation
/// </summary>
public class DesignationDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Designation name is required!")]
    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Department for designation is required!")]
    public string DepartmentId { get; set; }
    public DepartmentDtoModel? Department { get; set; }

    [Required(ErrorMessage = "Allowed designation seats are required!")]
    [Range(0, 999, ErrorMessage = "Value for designation seats cant exceed 999 nos.")]
    public byte AllowedSeats { get; set; } = 0; //here Zero 0 - represents n nos of seats. and fixed number represents designation can't exceeds for given allowedSeats.

    /// <summary>
    /// this property reflect that Designation which is alloted to user is Head of the department if value is true, 
    /// only one true value is allowed for once department.
    /// </summary>
    public bool IsHeadPosition { get; set; } = false;
}
