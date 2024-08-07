using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class MediaTypeVm : BaseVMTemplate
{
    /// <summary>
    /// Library Hall View Model model
    /// </summary>

    [Required(ErrorMessage = "Media type is required!")]
    public string Name { get; set; } = string.Empty;


}