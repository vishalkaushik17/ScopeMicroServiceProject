using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.CommonDtoModels;

/// <summary>
/// Currency Dto model
/// </summary>
public class CurrencyDtoModel : BaseDtoTemplate
{

    /// <summary>
    /// Currency name.
    /// </summary>
    [Required(ErrorMessage = "Currency name is required!")]
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Currency symbol
    /// </summary>
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]
    public string? Symbol { get; set; } = string.Empty;



}