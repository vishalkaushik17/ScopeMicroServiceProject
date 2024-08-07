using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.CommonDtoModels;

/// <summary>
/// Language Dto model
/// </summary>
public class LanguageDtoModel : BaseDtoTemplate
{

    /// <summary>
    /// Language name.
    /// </summary>
    [Required(ErrorMessage = "Language name is required!")]
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Native name for language.
    /// </summary>
    [Required(ErrorMessage = "Language native name is required!")]
    [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
    public string? NativeName { get; set; } = string.Empty;



}