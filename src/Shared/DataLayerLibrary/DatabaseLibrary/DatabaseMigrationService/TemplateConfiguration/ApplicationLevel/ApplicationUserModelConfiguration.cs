using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.Application;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Model configuration setup for application user for database table
/// </summary>
public class ApplicationUserModelConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(c => c.PersonalEmailId).IsRequired().HasMaxLength(450);
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
        

        ////User must belogs to company
        builder.HasOne(c => c.Company).WithMany().HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        ////User must belogs to company
        builder.HasOne(c => c.CompanyType).WithMany().HasForeignKey(c => c.CompanyTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}