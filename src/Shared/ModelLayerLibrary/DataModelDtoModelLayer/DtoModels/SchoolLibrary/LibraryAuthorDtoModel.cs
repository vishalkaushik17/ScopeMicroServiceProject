using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary;

/// <summary>
/// Author Dto model
/// </summary>
public class LibraryAuthorDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Author name is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Name { get; set; } = string.Empty;

}
