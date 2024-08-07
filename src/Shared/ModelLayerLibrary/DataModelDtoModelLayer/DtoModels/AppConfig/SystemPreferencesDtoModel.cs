using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.AppConfig;

public class SystemPreferencesDtoModel : BaseDtoTemplate
{
    /// <summary>
    /// Key belongs to which module
    /// </summary>

    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string ModuleName { get; set; } = string.Empty;
    /// <summary>
    /// Name of the preference key
    /// </summary>
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string PreferenceName { get; set; } = string.Empty;
    /// <summary>
    /// Default Value of the preference key
    /// </summary>
    [StringLength(5000, ErrorMessage = "Provided value exceed the required limit.")]
    public string DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// Custom Value of the preference key
    /// </summary>

    [StringLength(5000, ErrorMessage = "Provided value exceed the required limit.")] 
    public string CustomValue { get; set; } = string.Empty;

    /// <summary>
    /// Data type of the preference key
    /// </summary>
    public GenericFunction.Enums.ValueType ValueType { get; set; }

    /// <summary>
    /// Description of key and value
    /// </summary>
    [StringLength(5000, ErrorMessage = "Provided value exceed the required limit.")]
    public string Description { get; set; } = string.Empty;
}
