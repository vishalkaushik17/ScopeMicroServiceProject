using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.Application;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class ApplicationUserTokenModelConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {



        //Role must belogs to company
        builder.HasOne(c => c.Company).WithMany().HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);


        // We are using int here because of the change on the PK
        builder.Property(m => m.UserId).HasMaxLength(450);
        builder.Property(m => m.LoginProvider).HasMaxLength(255);
        builder.Property(m => m.Name).HasMaxLength(255);
        //builder.HasKey(c => new { c.UserId, c.LoginProvider, c.Name });
        //is record editable or locked.
        //builder.Property(c => c.Id).HasMaxLength(450).IsRequired();
        //        builder.Property(c => c.Value).HasMaxLength(65535).IsRequired();
        builder.Property(c => c.IsEditable).IsRequired();

        //on which date record is created.
        builder.Property(c => c.CreatedOn).IsRequired();

        //on which date record is modified.
        builder.Property(c => c.ModifiedOn).IsRequired(false);

        //is record active or deleted.
        builder.Property(c => c.RecordStatus).IsRequired();

        //CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);

    }
}