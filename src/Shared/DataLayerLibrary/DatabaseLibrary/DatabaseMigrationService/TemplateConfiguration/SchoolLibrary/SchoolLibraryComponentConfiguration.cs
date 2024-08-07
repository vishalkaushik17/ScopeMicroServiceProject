using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Component.School.Library;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// School Library Component Configuration to design table for Database .
/// </summary>
public class SchoolLibraryComponentConfiguration : IEntityTypeConfiguration<SchoolLibraryHallModel>
{
    public void Configure(EntityTypeBuilder<SchoolLibraryHallModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Location).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(200);

        //company must have its type
        //builder.HasOne(c => c.School).WithMany().HasForeignKey(c => c.SchoolId)
        //    .OnDelete(DeleteBehavior.NoAction);



        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }


}
