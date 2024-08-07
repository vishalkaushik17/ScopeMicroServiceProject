using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class LibraryHallVm : BaseVMTemplate
{
    /// <summary>
    /// Library Hall View Model model
    /// </summary>

    [Required(ErrorMessage = "Library name is required!")]
    public virtual string Name { get; set; } = string.Empty;

    public virtual string Location { get; set; } = "N/A";

    [DataType(DataType.MultilineText)]

    public virtual string Description { get; set; } = "N/A";

}

public class BlankVM : BaseVMTemplate
{
    /// <summary>
    /// Blank model for dashboard page
    /// </summary>
}