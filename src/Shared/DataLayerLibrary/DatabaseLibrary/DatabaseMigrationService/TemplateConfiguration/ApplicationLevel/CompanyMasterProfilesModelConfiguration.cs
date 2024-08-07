using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Master.Company;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Company Master Model configuration settings
/// </summary>
public class CompanyMasterProfilesModelConfiguration : IEntityTypeConfiguration<CompanyMasterProfileModel>
{
    public void Configure(EntityTypeBuilder<CompanyMasterProfileModel> builder)
    {
        //company name is required.
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.DatabaseName).IsRequired().HasMaxLength(5000);
        builder.Property(c => c.HostName).IsRequired().HasMaxLength(5000);
        builder.Property(c => c.Username).IsRequired().HasMaxLength(5000);
        builder.Property(c => c.NoOfUsers).IsRequired();
        builder.Property(c => c.NoOfEmployees).IsRequired();
        builder.Property(c => c.NoOfStudents).IsRequired();
        builder.Property(c => c.NoOfUsers).IsRequired();

        //Company name is unique
        builder.HasAlternateKey(c => c.Name);
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}

