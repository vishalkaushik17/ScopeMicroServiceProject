using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Company;

public sealed class CompanyMasterProfileDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Name of the company is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Database name is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string DatabaseName { get; set; }


    [Required(ErrorMessage = "Host name is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string HostName { get; set; }

    public int NoOfStudents { get; set; } = 0;

    public int NoOfEmployees { get; set; } = 0;

    public int NoOfUsers { get; set; } = 1; //default is 1 for newly created school db configuration
    public CompanyMasterProfileDtoModel() : base()
    {
        NoOfStudents = 0;
        NoOfEmployees = 0;
        NoOfUsers = 1;
    }

}