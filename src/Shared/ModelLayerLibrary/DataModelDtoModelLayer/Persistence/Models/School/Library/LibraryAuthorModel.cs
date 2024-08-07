using GenericFunction;
using GenericFunction.Enums;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.DtoModels.SchoolLibrary;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// Author Model is derived by Generic template which comprises of
/// completed and incomplete methods for Author Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class LibraryAuthorModel : BaseTemplate
{
    /// <summary>
    /// Name of the media author like book/cd/dvd/charts for school library
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Default save method, which will set author record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
    }
}

