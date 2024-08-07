using System.ComponentModel.DataAnnotations;

namespace GenericFunction.Enums;

/// <summary>
/// Vendor types enum collection
/// </summary>
public enum EnumVendorTypes : byte
{
    [Display(Name = "Vendor")]
    Vendor = 1,
    [Display(Name = "Publisher")]
    Publisher = 2,
    [Display(Name = "Wholesaler")]
    Wholesaler = 3
}
