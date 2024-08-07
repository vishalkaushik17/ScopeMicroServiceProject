using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Employee;

namespace DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;

/// <summary>
/// EmployeeStudentParent Master Component to design table for Database .
/// </summary>
public class EmployeeStudentParentMasterComponentConfiguration : IEntityTypeConfiguration<EmployeeStudentParentModel>
{
    public void Configure(EntityTypeBuilder<EmployeeStudentParentModel> builder)
    {

        builder.Property(c => c.RecordType).IsRequired();
        builder.Property(c => c.Gender).IsRequired();
        builder.Property(c => c.MaritalStatus).IsRequired();
        builder.Property(c => c.BloodGroup).IsRequired();
        builder.Property(c => c.EmploymentType).IsRequired();

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(c => c.MiddleName).IsRequired().HasMaxLength(30);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(30);
        builder.Property(c => c.MotherName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Spouse).IsRequired().HasMaxLength(100);
        builder.Property(c => c.ContactNo).IsRequired().HasMaxLength(30);
        builder.Property(c => c.SpouseContactNo).IsRequired().HasMaxLength(30);
        builder.Property(c => c.ProfessionalEmailId).IsRequired().HasMaxLength(60);
        builder.Property(c => c.PersonalEmailId).IsRequired().HasMaxLength(60);
        builder.Property(c => c.EmergencyContactNo).IsRequired().HasMaxLength(30);
        builder.Property(c => c.ResidentContactNo).IsRequired().HasMaxLength(30);
        builder.Property(c => c.Photograph).IsRequired();

        builder.Property(c => c.PanNo).IsRequired().HasMaxLength(12);
        builder.Property(c => c.UIDNo).IsRequired().HasMaxLength(20);
        builder.Property(c => c.ElectionId).IsRequired().HasMaxLength(20);
        builder.Property(c => c.DateOfBirth).IsRequired();
        builder.Property(c => c.PassportNo).IsRequired().HasMaxLength(20);


        builder.Property(c => c.Cast).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Religion).IsRequired();
        builder.Property(c => c.WorkExperience).IsRequired().HasMaxLength(3000);
        builder.Property(c => c.Achievements).IsRequired().HasMaxLength(3000);


        //builder.Property(c => c.BankId).IsRequired();
        //builder.Property(c => c.Bank).IsRequired();
        builder.Property(c => c.Ifsccode).IsRequired().HasMaxLength(20);
        builder.Property(c => c.MICRCode).IsRequired().HasMaxLength(20);
        builder.Property(c => c.BranchName).IsRequired().HasMaxLength(20);
        builder.Property(c => c.BankCity).IsRequired().HasMaxLength(30);
        builder.Property(c => c.BankState).IsRequired().HasMaxLength(30);



        builder.HasOne(c => c.Bank).WithMany(m => m.EmployeeStudentParentsMaster).HasForeignKey(c => c.BankId)
        .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_BankId");


        builder.HasMany(x => x.EmployeeParentVsQualifications).WithOne(b => b.EmployeeParentMaster).HasForeignKey(b => b.EmployeeParentId)
        .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_EmployeeStudentParentId");


        //  builder.HasOne(x => x.PermanentAddress).WithMany(b => b.EmployeeStudentParentsMaster).HasForeignKey(b => b.PermanentAddressId)
        //.OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_EmpStdParentPermanentAddressId");

        //  builder.HasOne(x => x.CommunicationAddress).WithMany(b => b.EmployeeStudentParentsMaster).HasForeignKey(b => b.CommunicationAddressId)
        //.OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_EmpStdParentCommunicationAddressId");
        builder.HasOne(c => c.CommunicationAddress).WithOne().HasForeignKey<EmployeeStudentParentModel>(c => c.CommunicationAddressId).HasConstraintName("Fk_CommunicationAddressId");
        builder.HasOne(c => c.PermanentAddress).WithOne().HasForeignKey<EmployeeStudentParentModel>(c => c.PermanentAddressId).HasConstraintName("Fk_PermanentAddressId");
        //builder.HasOne(m => m.PermanentAddress).WithMany().HasForeignKey(m=>m.PermanentAddressId);
        //builder.HasOne(m => m.CommunicationAddress).WithMany().HasForeignKey(m=>m.CommunicationAddressId);
        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}