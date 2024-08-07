using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.CommonModels;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Currency Model Configuration to design table for Database .
public class CurrencyModelComponentConfiguration : IEntityTypeConfiguration<CurrencyModel>
/// </summary>
{
    public void Configure(EntityTypeBuilder<CurrencyModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Symbol).HasMaxLength(20);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
