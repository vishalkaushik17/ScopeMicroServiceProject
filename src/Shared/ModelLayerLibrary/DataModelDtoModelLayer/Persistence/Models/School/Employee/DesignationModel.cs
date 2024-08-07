using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Employee;

/// <summary>
/// this class represents designations which is used to represents employee designation
/// </summary>
public class DesignationModel : BaseTemplate
{
    public string Name { get; set; }
    public string DepartmentId { get; set; }
    public DepartmentModel Department { get; set; }
    public byte AllowedSeats { get; set; } = 0; //here Zero 0 - represents n nos of seats. and fixed number represents designation can't exceeds for given allowedSeats.

    /// <summary>
    /// this property reflect that Designation which is alloted to user is Head of the department if value is true, 
    /// only one true value is allowed for once department.
    /// </summary>
    public bool IsHeadPosition { get; set; }

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
