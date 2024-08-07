using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.Application;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

/// <summary>
/// Refresh Token Model configuration settings
/// </summary>
public class RefreshTokenModelConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.Property(c => c.Created).IsRequired();
        builder.Property(c => c.CreatedByIp).IsRequired();
        builder.Property(c => c.Expires).IsRequired();
        builder.Property(c => c.Token).IsRequired();

        builder.HasOne(c => c.CompanyMasterEntityModel).WithMany().HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}