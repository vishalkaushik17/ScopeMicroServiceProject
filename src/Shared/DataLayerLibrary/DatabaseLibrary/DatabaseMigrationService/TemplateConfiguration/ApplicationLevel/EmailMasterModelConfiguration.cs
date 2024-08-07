using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Core.Model;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class EmailMasterModelConfiguration : IEntityTypeConfiguration<EmailMaster>
{
    public void Configure(EntityTypeBuilder<EmailMaster> builder)
    {
        // We are using int here because of the change on the PK
        builder.Property(m => m.UserId).HasMaxLength(450);
        builder.Property(m => m.EmailNotificationType).IsRequired();
        builder.Property(m => m.ToEmail).HasMaxLength(255);
        builder.Property(m => m.FromEmail).HasMaxLength(255);
        builder.Property(m => m.CCEmail).HasMaxLength(500);
        builder.Property(m => m.Subject).HasMaxLength(1000);
        builder.Property(m => m.Body).HasMaxLength(10000);
        builder.Property(m => m.ClientId).HasMaxLength(540);

        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}