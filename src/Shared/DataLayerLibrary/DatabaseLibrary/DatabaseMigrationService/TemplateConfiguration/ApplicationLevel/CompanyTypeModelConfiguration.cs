using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.Company;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Company Type Configurations
/// </summary>
public class CompanyTypeModelConfiguration : IEntityTypeConfiguration<CompanyTypeModel>
{
    public void Configure(EntityTypeBuilder<CompanyTypeModel> builder)
    {
        //TypeName is 100 chars max length
        builder.Property(c => c.TypeName).IsRequired().HasMaxLength(15);

        //type name has index with unique constraints.
        builder.HasAlternateKey(m => m.TypeName);

        //Common for all
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
