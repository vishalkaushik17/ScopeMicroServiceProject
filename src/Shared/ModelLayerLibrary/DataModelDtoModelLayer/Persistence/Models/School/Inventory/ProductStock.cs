using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Inventory;

public class ProductStock : BaseTemplate
{
    public required byte InOut { get; set; }
    public required int Qty { get; set; }
    public required ProductModel Product { get; set; }
    public required string ProductId { get; set; }
    public required DateTime TransactionDate { get; set; }
}

