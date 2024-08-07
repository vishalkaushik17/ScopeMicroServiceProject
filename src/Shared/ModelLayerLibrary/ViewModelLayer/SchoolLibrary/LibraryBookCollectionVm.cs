using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

/// <summary>
/// Book Collection View Model
/// </summary>

public class LibraryBookCollectionVm : BaseVMTemplate
{

    /// <summary>
    /// Name of the book collection
    /// </summary>
    [Required(ErrorMessage = "Book collection name is required!")]

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Abbreviation of book. eg. BK
    /// </summary>

    [Required(ErrorMessage = "Book Abbreviation is required!")]
    public string Abbreviation { get; set; } = string.Empty;


}