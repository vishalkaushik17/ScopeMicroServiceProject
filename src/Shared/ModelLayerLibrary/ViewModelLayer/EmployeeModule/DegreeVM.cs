using System.ComponentModel.DataAnnotations;

namespace VModelLayer.EmployeeModule;

/// <summary>
/// this model represents qualification of employee/parents.  
/// In UI it will shows as list 
/// </summary>
public class DegreeVM : BaseVMTemplate
{
    [Required(ErrorMessage = "Name of degree information is required!")]
    public string Name { get; set; }

}


