using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.AppConfig;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class ApplicationHostMasterVSCompanyMasterModelConfiguration : IEntityTypeConfiguration<AppDBHostVsCompanyMaster>
{
    public void Configure(EntityTypeBuilder<AppDBHostVsCompanyMaster> builder)
    {

        //company must have its type
        builder.HasOne(c => c.CompanyMaster).WithMany().HasForeignKey(c => c.CompanyMasterId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.ApplicationHostMaster).WithMany().HasForeignKey(c => c.AppHostId)
            .OnDelete(DeleteBehavior.NoAction);

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}