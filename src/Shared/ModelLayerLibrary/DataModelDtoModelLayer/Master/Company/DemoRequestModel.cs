using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Master.Company;

/// <summary>
/// this is demo request data model class, which is used for registering demo request.
/// </summary>
public class DemoRequestModel : BaseTemplate
{
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactNo { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ReferenceCode { get; set; } = string.Empty;
    public bool IsDemoActivated { get; set; } = false;
    public DateTime? DemoActivatedOn { get; set; }
    public bool IsRestrictedForDemo { get; set; } = false;

    public void Save()
    {
        Id = Guid.NewGuid().ToString("D"); //need to change from db
        Name = Name.ToCamelCase();
        City = City.ToUpper();
        ReferenceCode = ReferenceCode.ToUpper();
        Website = Website.ToLower();
        Email = Email.ToLower();
        FirstName = FirstName.ToCamelCase();
        LastName = LastName.ToCamelCase();
        IsDemoActivated = false;
    }

    public void SetDemoStatus(bool? demoStatus = true)
    {
        IsDemoActivated = demoStatus ?? false;
        DemoActivatedOn = DateTime.Now;
    }
}