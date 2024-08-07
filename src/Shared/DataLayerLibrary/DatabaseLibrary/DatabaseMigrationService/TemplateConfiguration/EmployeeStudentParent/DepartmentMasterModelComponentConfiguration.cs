using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// Designation Category Master Model Component Configuration to design table for Database .
/// </summary>
public class DepartmentMasterModelComponentConfiguration : IEntityTypeConfiguration<DepartmentModel>
{
    public void Configure(EntityTypeBuilder<DepartmentModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.HasMany(x => x.Designations).WithOne(b => b.Department).HasForeignKey(b => b.DepartmentId)
        .OnDelete(DeleteBehavior.NoAction);
        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}