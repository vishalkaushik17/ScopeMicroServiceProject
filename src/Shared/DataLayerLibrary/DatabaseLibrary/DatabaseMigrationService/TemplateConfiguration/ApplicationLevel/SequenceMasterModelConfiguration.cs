using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.AppLevel;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Sequence Master Model configuration settings
/// </summary>
public class SequenceMasterModelConfiguration : IEntityTypeConfiguration<Sequence>
{
    public void Configure(EntityTypeBuilder<Sequence> builder)
    {
        builder.Property(c => c.Suffix).IsRequired().HasMaxLength(3);
        builder.Property(c => c.Prefix).IsRequired().HasMaxLength(3);
        builder.Property(c => c.DoRepeat).IsRequired();
        builder.Property(c => c.AddMonth).IsRequired();
        builder.Property(c => c.AddYear).IsRequired();
        builder.Property(c => c.IncrementBy).IsRequired();
        builder.Property(c => c.SequenceLength).IsRequired();
        builder.Property(c => c.TableName).IsRequired().HasMaxLength(50);

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
