using GenericFunction;
using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.Persistence.Models.School.Employee;

namespace ModelTemplates.Persistence.Models.School.CommonModels;

/// <summary>
/// Vendor Component is derived by SchoolLibraryTemplate which comprises of
/// completed and incomplete methods for SchoolLibrary Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class VendorModel : BaseTemplate//, ISchoolLibraryContract
{
    /// <summary>
    /// Name of the vendor/supplier/publisher company
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;
    public string GSTNo { get; set; } = "n/a";
    public string CSTNo { get; set; } = "n/a";
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactNo { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public EnumVendorTypes Type { get; set; }

    /// <summary>
    /// Default abstract method implementation for Saving library record.
    /// </summary>
    /// <param name="userId"></param>
    public void Save(string addressId, string userId)
    {
        base.Save(userId);
        AddressId = addressId;
        CompanyName = CompanyName.ToCamelCase();
        ContactPerson = ContactPerson != DefaultStringValue ? ContactPerson.ToCamelCase() : ContactPerson;

        EmailId = EmailId != DefaultStringValue ? EmailId.ToLower() : EmailId;
        Website = Website != DefaultStringValue ? Website.ToLower() : Website;

        CSTNo = CSTNo != DefaultStringValue ? CSTNo.ToUpper() : CSTNo;
        GSTNo = GSTNo != DefaultStringValue ? GSTNo.ToUpper() : GSTNo;

    }

    public void Update(string addressId, string userId)
    {
        this.Save(addressId, userId);
        base.Update();
    }
    public string AddressId { get; set; }
    public AddressModel Address { get; set; }

}


