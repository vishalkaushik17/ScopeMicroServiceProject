using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.CommonModels;

/// <summary>
/// Language Model is derived by Generic template which comprises of
/// completed and incomplete methods for Language Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class LanguageModel : BaseTemplate
{
    /// <summary>
    /// Name of the Language. eg. Hindi
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Native language name. eg. हिंदी
    /// </summary>
    public string NativeName { get; set; } = string.Empty;


    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
        NativeName = NativeName.RemoveSpaces();
    }

    public new void Update(string userId)
    {
        this.Save(userId);
        base.Update();
    }

}

