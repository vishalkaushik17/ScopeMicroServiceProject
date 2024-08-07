using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// This dto class represents in which category Designation belongs to.
/// </summary>
public class DepartmentDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Department name is required!")]
    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string Name { get; set; }
    public ICollection<DesignationDtoModel> Designations { get; set; } = new List<DesignationDtoModel>();

}
