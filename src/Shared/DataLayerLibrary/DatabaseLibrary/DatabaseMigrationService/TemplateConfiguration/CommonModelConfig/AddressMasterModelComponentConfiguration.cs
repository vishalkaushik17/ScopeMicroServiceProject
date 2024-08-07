using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// Address Model Component Configuration to design table for Database .
/// </summary>
public class AddressMasterModelComponentConfiguration : IEntityTypeConfiguration<AddressModel>
{
    public void Configure(EntityTypeBuilder<AddressModel> builder)
    {

        builder.Property(c => c.Address1).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Address2).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Milestone).IsRequired().HasMaxLength(60);
        builder.Property(c => c.Area).IsRequired().HasMaxLength(60);
        builder.Property(c => c.City).IsRequired().HasMaxLength(50);
        builder.Property(c => c.State).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Country).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Pincode).IsRequired().HasMaxLength(10);



        //  builder.HasMany(x => x.EmployeeStudentParentsMaster).WithOne(b => b.PermanentAddress).HasForeignKey(b => b.PermanentAddressId)
        //.OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_EmpStdParentPermanentAddressId");
        //builder.HasMany(m => m.EmployeeStudentParentsMaster);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
