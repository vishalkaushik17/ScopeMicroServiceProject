using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Employee;

/// <summary>
/// this model represents address of the employee/students/parents.
/// </summary>
public class AddressModel : BaseTemplate
{
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Milestone { get; set; }
    public string Area { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Pincode { get; set; }
    public string Country { get; set; }
    //public List<EmployeeStudentParentModel> EmployeeStudentParentsMaster { get; set; } = new List<EmployeeStudentParentModel>();
    //public List<VendorMasterModel> Vendors { get; set; } = new List<VendorMasterModel>();

    public new void Save(string userId)
    {
        base.Save(userId);
        Address1 = Address1 != DefaultStringValue ? Address1.ToLower() : Address1;
        Address2 = Address2 != DefaultStringValue ? Address2.ToLower() : Address2;
        City = City != DefaultStringValue ? City.ToLower() : City;
        State = State != DefaultStringValue ? State.ToLower() : State;
        Milestone = Milestone != DefaultStringValue ? Milestone.ToLower() : Milestone;
        Country = Country != DefaultStringValue ? Country.ToLower() : Country;
    }
    public new void Update(string userId)
    {
        this.Save(userId);
    }
}
