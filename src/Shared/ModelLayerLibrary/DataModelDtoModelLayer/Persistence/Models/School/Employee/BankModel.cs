using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Employee;

/// <summary>
/// this model represent name of bank, in which employee/student/parents has its account
/// In UI it will shows as list 
/// </summary>
public class BankModel : BaseTemplate
{
    public string Name { get; set; }
    public List<EmployeeStudentParentModel> EmployeeStudentParentsMaster { get; set; } = new List<EmployeeStudentParentModel>();

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
