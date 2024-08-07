using GenericFunction;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.DtoModels.SchoolLibrary;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// Room Model is derived by Generic template which comprises of
/// completed and incomplete methods for Room Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.

/// </summary>
public class LibraryBookCollectionModel : BaseTemplate
{
    /// <summary>
    /// Name of the book collection
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Abbreviation of book. eg. BK
    /// </summary>
    public string Abbreviation { get; set; } = string.Empty;


    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save( string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
        Abbreviation = Abbreviation.ToUpper().RemoveSpaces();
    }
}

