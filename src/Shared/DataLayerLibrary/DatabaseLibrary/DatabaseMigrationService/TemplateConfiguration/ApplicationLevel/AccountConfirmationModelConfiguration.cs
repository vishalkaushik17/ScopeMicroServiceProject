using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.UserAccount;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class AccountConfirmationModelConfiguration : IEntityTypeConfiguration<AccountConfirmationModel>
{
    public void Configure(EntityTypeBuilder<AccountConfirmationModel> builder)
    {
        builder.Property(c => c.HostId).IsRequired();
        builder.Property(c => c.ExpiredDate).IsRequired();
        builder.Property(c => c.UserName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.IsConfirmed).IsRequired();


        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}