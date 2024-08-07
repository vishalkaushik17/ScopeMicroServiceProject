using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.DatabaseConfig;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Connect Database Model configuration settings
/// </summary>
public class DatabaseConnectionModelConfiguration : IEntityTypeConfiguration<DatabaseConnection>
{
    public void Configure(EntityTypeBuilder<DatabaseConnection> builder)
    {
        builder.Property(c => c.Catalog).IsRequired().HasMaxLength(50);
        builder.Property(c => c.DataSourceServer).IsRequired().HasMaxLength(100);
        builder.Property(c => c.DbType).IsRequired().HasMaxLength(50);
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
