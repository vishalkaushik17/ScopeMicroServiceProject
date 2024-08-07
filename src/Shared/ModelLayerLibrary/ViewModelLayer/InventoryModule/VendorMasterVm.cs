using GenericFunction.Enums;
using VModelLayer.GeneralModule;

namespace VModelLayer.InventoryModule;

public class VendorMasterVm : BaseVMTemplate
{
    public string CompanyName { get; set; } = string.Empty;
    public string GSTNo { get; set; } = DefaultStringValue;
    public string CSTNo { get; set; } = DefaultStringValue;
    public string ContactPerson { get; set; } = DefaultStringValue;
    public string ContactNo { get; set; } = DefaultStringValue;
    public string EmailId { get; set; } = DefaultStringValue;
    public string Website { get; set; } = DefaultStringValue;
    public EnumVendorTypes Type { get; set; } = EnumVendorTypes.Vendor;

    public string AddressId { get; set; }
    public AddressMasterVm Address { get; set; } = new AddressMasterVm();
}