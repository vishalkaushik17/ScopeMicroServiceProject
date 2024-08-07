namespace VModelLayer.EmployeeModule;

/// <summary>
/// this dto class represents designations which is used to represents employee designation
/// </summary>
public class DesignationsVM : BaseVMTemplate
{
    public string Name { get; set; }
    public string DepartmentId { get; set; }
    public List<DepartmentVM> Department { get; set; } = new List<DepartmentVM>();
    public byte AllowedSeats { get; set; } = 0; //here Zero 0 - represents n nos of seats. and fixed number represents designation can't exceeds for given allowedSeats.
    /// <summary>
    /// this property reflect that Designation which is alloted to user is Head of the department if value is true, 
    /// only one true value is allowed for once department.
    /// </summary>
    public bool IsHeadPosition { get; set; } = false;
}
