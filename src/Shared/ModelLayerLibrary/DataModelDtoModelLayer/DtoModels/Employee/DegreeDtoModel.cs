using ModelTemplates.Core.GenericModel;
using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// this model represents qualification of employee/parents.  
/// In UI it will shows as list 
/// </summary>
public class DegreeDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Name of degree information is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Name { get; set; }

}


