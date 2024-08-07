using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// Educational Master Qualification Model Component Configuration to design table for Database .
/// </summary>
public class EmployeeQualificationsModelComponentConfiguration : IEntityTypeConfiguration<EmployeeQualificationModel>
{
    public void Configure(EntityTypeBuilder<EmployeeQualificationModel> builder)
    {

        //builder.HasOne(m => m.Employee).WithMany().HasForeignKey(m => m.EmployeeId);
        //builder.HasOne(m => m.Qualification).WithMany().HasForeignKey(m => m.QualificationId);
        //builder.HasOne(m => m.EmployeeStudentParentMaster).WithMany().HasForeignKey(m => m.EmployeeStudentParentId);
        //builder.HasOne(m => m.DegreeMaster).WithMany().HasForeignKey(m => m.DegreeId);

        builder.HasAlternateKey(x => new { x.DegreeId, x.EmployeeParentId });

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
