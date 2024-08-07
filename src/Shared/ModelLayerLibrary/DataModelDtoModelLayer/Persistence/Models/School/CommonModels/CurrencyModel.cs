using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.CommonModels;

/// <summary>
/// Currency Model is derived by Generic template which comprises of
/// completed and incomplete methods for currency Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class CurrencyModel : BaseTemplate
{
    /// <summary>
    /// Name of the currency
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Currency symbol as per international standards
    /// </summary>

    public string? Symbol { get; set; } = string.Empty;
    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
        Symbol = Symbol?.RemoveSpaces();
    }
    public new void Update(string userId)
    {
        this.Save(userId);
        base.Update();
    }

}

