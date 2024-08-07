using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// Designations Master Model Component Configuration to design table for Database .
/// </summary>
public class DesignationsModelComponentConfiguration : IEntityTypeConfiguration<DesignationModel>
{
    public void Configure(EntityTypeBuilder<DesignationModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.AllowedSeats).IsRequired();

        //one to many relation build

        //     builder.HasOne(c => c.Department).WithOne(b=>b.).HasForeignKey<DesignationsMasterModel>(c =>c.DepartmentId);
        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
