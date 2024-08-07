using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBOperationsLayer.TemplateConfiguration;

/// <summary>
/// Common Configurations for all the tables.
/// </summary>
public static class CommonOperationsOnModelConfiguration
{
    public static void CommonModelConfigurations(EntityTypeBuilder builder)
    {
        //each id is primary key
        builder.HasKey("Id");
        builder.Property("Id").HasMaxLength(450);

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