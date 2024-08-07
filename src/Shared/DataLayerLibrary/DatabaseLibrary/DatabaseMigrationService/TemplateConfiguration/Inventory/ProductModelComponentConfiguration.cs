using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.CommonModels;
using ModelTemplates.Persistence.Models.School.Inventory;

namespace DBOperationsLayer.TemplateConfiguration.Inventory;

/// <summary>
/// Product Model Configuration to design table for Database .
/// </summary>
public class ProductModelComponentConfiguration : IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> builder)
    {

        builder.Property(c => c.ProductName).IsRequired().HasMaxLength(40);
        builder.Property(c => c.Details).HasMaxLength(1000);
        builder.Property(c => c.CompanyBarCode).HasMaxLength(15);
        builder.Property(c => c.CustomBarCode).HasMaxLength(15);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
