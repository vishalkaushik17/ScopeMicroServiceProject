using System.ComponentModel.DataAnnotations;

namespace VModelLayer.GeneralModule;

public class LanguageVm : BaseVMTemplate
{
    /// <summary>
    /// Language View Model
    /// </summary>

    [Required(ErrorMessage = "Language name is required!")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Native name is required!")]
    public string NativeName { get; set; } = string.Empty;

}
