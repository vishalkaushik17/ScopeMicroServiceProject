using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.AppConfig;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Company Master Model configuration settings
/// </summary>
public class ApplicationHostMasterModelConfiguration : IEntityTypeConfiguration<ApplicationHostMasterModel>
{
    public void Configure(EntityTypeBuilder<ApplicationHostMasterModel> builder)
    {

        builder.Property(c => c.UserName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.ConnectionString).HasMaxLength(2000).IsRequired();
        builder.Property(c => c.Domain).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.HashString).HasMaxLength(450).IsRequired();
        builder.Property(c => c.DatabaseType).IsRequired();
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}
