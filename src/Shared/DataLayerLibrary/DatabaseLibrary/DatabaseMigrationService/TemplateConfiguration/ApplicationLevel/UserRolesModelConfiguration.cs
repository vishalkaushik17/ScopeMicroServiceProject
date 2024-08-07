using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.EntityModels.Application;

namespace DBOperationsLayer.TemplateConfiguration.ApplicationLevel;

public class UserRolesModelConfiguration : IEntityTypeConfiguration<UserRoles>
{
    public void Configure(EntityTypeBuilder<UserRoles> builder)
    {

        //Role must belogs to company
        builder.HasOne(c => c.Company).WithMany().HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        //user id is used for which user has added the entry.
        builder.Property("UserId").IsRequired().HasMaxLength(450);

        //is record editable or locked.
        builder.Property("IsEditable").IsRequired();

        //on which date record is created.
        builder.Property("CreatedOn").IsRequired();

        //on which date record is modified.
        builder.Property("ModifiedOn").IsRequired(false);

        //is record active or deleted.
        builder.Property("RecordStatus").IsRequired();

    }
}