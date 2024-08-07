using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Employee;


/// <summary>
/// this is qualification master table which consist of employee qualification list
/// in which year employee has taken degree
/// </summary>
public class EmployeeQualificationModel : BaseTemplate
{
    public string DegreeId { get; set; }
    public DegreeModel DegreeMaster { get; set; }
    public DateTime MonthOfCompletion { get; set; }
    public DateTime YearOfCompletion { get; set; }
    public string Grade { get; set; }
    public string EmployeeParentId { get; set; }
    public EmployeeStudentParentModel EmployeeParentMaster { get; set; }

    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);

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
