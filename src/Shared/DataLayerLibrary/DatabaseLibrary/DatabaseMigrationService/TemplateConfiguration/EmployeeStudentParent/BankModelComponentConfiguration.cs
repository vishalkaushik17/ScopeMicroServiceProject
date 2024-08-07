using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// Bank master Component Configuration to design table for Database .
/// </summary>
public class BankModelComponentConfiguration : IEntityTypeConfiguration<BankModel>
{
    public void Configure(EntityTypeBuilder<BankModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        //        builder.HasMany(x => x.EmployeeStudentParentsMaster).WithOne(b => b.BankMaster).HasForeignKey(b => b.BankId)
        //.OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_BankId");

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
