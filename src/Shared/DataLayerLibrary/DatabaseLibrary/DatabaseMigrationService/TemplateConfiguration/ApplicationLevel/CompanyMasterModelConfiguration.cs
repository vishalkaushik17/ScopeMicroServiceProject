using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Master.Company;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Company Master Model configuration settings
/// </summary>
public class CompanyMasterModelConfiguration : IEntityTypeConfiguration<CompanyMasterModel>
{
    public void Configure(EntityTypeBuilder<CompanyMasterModel> builder)
    {
        //company name is required.
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        //email id is must for every company to communicate.
        builder.Property(c => c.Email).IsRequired().HasMaxLength(50);

        builder.Property(c => c.DemoRequestId).IsRequired().HasMaxLength(450);

        //suffix domain is required for every record
        builder.Property(c => c.SuffixDomain).IsRequired().HasMaxLength(30);
        builder.Property(c => c.Website).IsRequired().HasMaxLength(50);

        //suffix domain is required for every record
        builder.Property(c => c.ReferenceCode).IsRequired().HasMaxLength(450);
        //builder.HasIndex(c => c.ReferenceCode).IsUnique(true);


        //company must have its type
        builder.HasOne(c => c.CompanyTypeEntityModel).WithMany().HasForeignKey(c => c.CompanyTypeId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.HasOne(c => c.CompanyMasterProfile).WithOne().HasForeignKey<CompanyMasterProfileModel>(c => c.Id);
        //Company name is unique
        builder.HasIndex(c => c.Name).IsUnique();

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}