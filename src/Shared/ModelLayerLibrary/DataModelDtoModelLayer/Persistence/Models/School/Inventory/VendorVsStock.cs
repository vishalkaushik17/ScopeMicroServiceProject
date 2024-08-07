using ModelTemplates.Persistence.Models.School.CommonModels;

namespace ModelTemplates.Persistence.Models.School.Inventory;

public class VendorVsStock
{
    public required VendorModel Vendor { get; set; }
    public required string VendorId { get; set; }
    public required ProductStock ProductStock { get; set; }
    public required string ProductStockTransactionId { get; set; }


}

