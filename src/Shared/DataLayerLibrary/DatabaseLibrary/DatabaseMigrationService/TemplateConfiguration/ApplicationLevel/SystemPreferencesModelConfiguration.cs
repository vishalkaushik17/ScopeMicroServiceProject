using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.AppLevel;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// System preferences Configurations
/// </summary>
public class SystemPreferencesModelConfiguration : IEntityTypeConfiguration<SystemPreferencesModel>
{
    public void Configure(EntityTypeBuilder<SystemPreferencesModel> builder)
    {

        builder.Property(c => c.ModuleName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.PreferenceName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.ValueType).IsRequired();
        builder.Property(c => c.DefaultValue).IsRequired().HasMaxLength(5000);
        builder.Property(c => c.CustomValue).IsRequired().HasMaxLength(5000).HasDefaultValue("n/a");
        builder.Property(c => c.Description).IsRequired().HasMaxLength(2000);

        //Composite unique key
        builder.HasAlternateKey(m => new { m.PreferenceName, m.ModuleName });


        //Common for all
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}