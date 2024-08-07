using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Master.Company;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class SequenceGeneratorModelConfiguration : IEntityTypeConfiguration<SequenceGenerator>
{
    public void Configure(EntityTypeBuilder<SequenceGenerator> builder)
    {
        //
        builder.Property(c => c.Prefix).IsRequired().HasMaxLength(5);
        builder.Property(c => c.Suffix).IsRequired().HasMaxLength(5);
        builder.Property(c => c.TableName).IsRequired().HasMaxLength(100);

        //Sequence must belongs to company
        builder.HasOne(c => c.Company).WithMany().HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);


        //Common for all
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }


}