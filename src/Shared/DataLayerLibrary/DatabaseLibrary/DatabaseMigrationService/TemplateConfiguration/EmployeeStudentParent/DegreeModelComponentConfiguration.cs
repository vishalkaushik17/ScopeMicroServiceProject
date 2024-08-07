using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// Educational Master Qualification Model Component Configuration to design table for Database .
/// </summary>
public class DegreeModelComponentConfiguration : IEntityTypeConfiguration<DegreeModel>
{
    public void Configure(EntityTypeBuilder<DegreeModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.HasMany(x => x.Qualifications).WithOne(b => b.DegreeMaster).HasForeignKey(b => b.DegreeId)
        .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_DegreeId");

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
