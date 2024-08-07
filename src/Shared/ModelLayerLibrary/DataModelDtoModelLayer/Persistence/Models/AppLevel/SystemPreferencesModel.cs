using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.AppLevel;


/// <summary>
/// Application preference table
/// </summary>
public class SystemPreferencesModel : BaseTemplate
{
    /// <summary>
    /// Key belongs to which module
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;
    /// <summary>
    /// Name of the preference key
    /// </summary>
    public string PreferenceName { get; set; } = string.Empty;
    /// <summary>
    /// Default Value of the preference key
    /// </summary>
    public string DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// Custom Value of the preference key
    /// </summary>
    public string CustomValue { get; set; } = string.Empty;

    /// <summary>
    /// Data type of the preference key
    /// </summary>
    public GenericFunction.Enums.ValueType ValueType { get; set; }

    /// <summary>
    /// Description of key and value
    /// </summary>
    public string Description { get; set; } = string.Empty;

    public new void Save(string userId)
    {
        base.Save(userId);
        ModuleName = ModuleName.ToUpper();
        PreferenceName = PreferenceName.ToUpper();
        UserId = userId;

    }
}
