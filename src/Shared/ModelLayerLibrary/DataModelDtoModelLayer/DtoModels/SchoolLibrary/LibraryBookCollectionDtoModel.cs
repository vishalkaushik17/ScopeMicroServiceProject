using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary;

/// <summary>
/// Book collection Dto model
/// </summary>
public class LibraryBookCollectionDtoModel : BaseDtoTemplate
{
    /// <summary>
    /// Name of the book collection
    /// </summary>
    [Required(ErrorMessage = "Book collection name is required!")]
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Abbreviation of book. eg. BK
    /// </summary>

    [Required(ErrorMessage = "Book Abbreviation is required!")]
    [StringLength(4, ErrorMessage = "Provided value exceed the required limit (4 Characters).")]
    public string Abbreviation { get; set; } = string.Empty;

}