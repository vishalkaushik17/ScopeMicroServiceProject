using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Company;

public sealed class CompanyTypeDtoModel : BaseDtoTemplate //<CompanyTypeEntityModel>
{
    [Required(ErrorMessage = "Type is required!")]
    [StringLength(15, ErrorMessage = "Provided value exceed the required limit (15 Characters).")]
    public string TypeName { get; set; }

}