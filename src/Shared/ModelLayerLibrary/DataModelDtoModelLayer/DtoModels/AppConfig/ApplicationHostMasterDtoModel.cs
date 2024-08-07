using GenericFunction.Enums;
using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.AppConfig;



public sealed class ApplicationHostMasterDtoModel : BaseDtoTemplate
{

    [Required(ErrorMessage = "Domain name is required!")]
    [StringLength(1000, ErrorMessage = "Provided value exceed the required limit (1000 Characters).")]
    public string Domain { get; set; } = string.Empty;

    [Required(ErrorMessage = "Connection string is required!")]
    [StringLength(2000, ErrorMessage = "Provided value exceed the required limit (2000 Characters).")]
    public string ConnectionString { get; set; } = string.Empty;

    [Required(ErrorMessage = "User name for Domain is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type of database is required!")]
    public EnumDBType DatabaseType { get; set; }

    [StringLength(450, ErrorMessage = "Provided value exceed the required limit (450 Characters).")]
    public string HashString { get; set; } = string.Empty;

}
