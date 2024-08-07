namespace VModelLayer.EmployeeModule;

/// <summary>
/// this model represents address of the employee/students/parents/vendors etc.
/// </summary>
public class AddressVM : BaseVMTemplate
{

    public string Address1 { get; set; } = DefaultStringValue;


    public string Address2 { get; set; } = DefaultStringValue;


    public string Milestone { get; set; } = DefaultStringValue;


    public string Area { get; set; } = DefaultStringValue;


    public string City { get; set; } = DefaultStringValue;


    public string State { get; set; } = DefaultStringValue;


    public string Pincode { get; set; } = DefaultStringValue;


    public string Country { get; set; } = DefaultStringValue;
}
