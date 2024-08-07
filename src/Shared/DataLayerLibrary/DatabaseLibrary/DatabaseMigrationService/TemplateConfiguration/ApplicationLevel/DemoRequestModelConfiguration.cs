using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Master.Company;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class DemoRequestModelConfiguration : IEntityTypeConfiguration<DemoRequestModel>
{
    public void Configure(EntityTypeBuilder<DemoRequestModel> builder)
    {
        //company name is required.
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        //email id is must for every company to communicate.
        builder.Property(c => c.Email).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Website).IsRequired().HasMaxLength(50);
        builder.Property(c => c.ContactNo).IsRequired().HasMaxLength(20);
        builder.Property(c => c.ReferenceCode).HasMaxLength(450);
        builder.Property(c => c.City).IsRequired().HasMaxLength(20);

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);

        //Company name is unique
        builder.HasAlternateKey(c => c.Name);
        builder.HasAlternateKey(c => c.Website);
        builder.HasAlternateKey(c => c.Email);
        builder.HasAlternateKey(c => c.ContactNo);

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}