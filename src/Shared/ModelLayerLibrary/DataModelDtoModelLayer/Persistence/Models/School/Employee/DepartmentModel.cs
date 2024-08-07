using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Employee;

/// <summary>
/// This class represents in which category Designation belongs to.
/// </summary>
public class DepartmentModel : BaseTemplate
{
    public string Name { get; set; }
    public List<DesignationModel> Designations { get; set; } = new List<DesignationModel>();

    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
    }

    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Update(string userId)
    {
        this.Save(userId);
        base.Update();
    }


}
