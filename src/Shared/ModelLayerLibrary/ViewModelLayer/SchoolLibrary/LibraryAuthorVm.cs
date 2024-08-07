using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class LibraryAuthorVm : BaseVMTemplate
{
    /// <summary>
    /// Author View Model model
    /// </summary>

    [Required(ErrorMessage = "Author name is required!")]
    public string Name { get; set; } = string.Empty;

}