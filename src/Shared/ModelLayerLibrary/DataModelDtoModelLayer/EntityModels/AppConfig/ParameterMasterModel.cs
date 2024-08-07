using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.EntityModels.AppConfig;

/// <summary>
/// Responsible for parameterize application flow for Company.School.
/// </summary>
public sealed class ParameterMasterModel : BaseTemplate
{
    public string TemplateName { get; set; } = string.Empty;
    public string ParamName { get; set; } = string.Empty;
    public string ParamValue { get; set; } = string.Empty;
    public string ParamType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public new void Save(string userId)
    {
        Id = Guid.NewGuid().ToString("D");
        TemplateName = TemplateName.ToUpper();
        CreatedOn = DateTime.Now;
        RecordStatus = EnumRecordStatus.Active;
        UserId = userId;
    }

    public void Update(string userId)
    {
        TemplateName = TemplateName.ToUpper();
        UserId = userId;
        RecordStatus = EnumRecordStatus.Active;
    }
}
