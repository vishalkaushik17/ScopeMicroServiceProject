using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.CommonModels;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Vendor Model Configuration to design table for Database .
/// </summary>
public class VendorModelComponentConfiguration : IEntityTypeConfiguration<VendorModel>
{
    public void Configure(EntityTypeBuilder<VendorModel> builder)
    {

        builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.ContactPerson).IsRequired().HasMaxLength(100);
        builder.Property(c => c.ContactNo).IsRequired().HasMaxLength(30);
        builder.Property(c => c.GSTNo).HasMaxLength(50);
        builder.Property(c => c.CSTNo).HasMaxLength(50);
        builder.Property(c => c.EmailId).HasMaxLength(50);
        builder.Property(c => c.Website).HasMaxLength(50);
        builder.Property(c => c.Type).IsRequired();

        // builder.HasOne(x => x.Address).WithOne().HasForeignKey(b => b.AddressId)
        //.OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_VendorAddressId");
        builder.HasOne(c => c.Address).WithOne().HasForeignKey<VendorModel>(c => c.AddressId).HasConstraintName("Fk_VendorAddressId");
        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
