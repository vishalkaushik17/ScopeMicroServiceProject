using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Library;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Rack Model Configuration to design table for Database .
/// </summary>
public class RackModelComponentConfiguration : IEntityTypeConfiguration<LibraryRackModel>
{
    public void Configure(EntityTypeBuilder<LibraryRackModel> builder)
    {
        //Name has max 30 length
        builder.Property(c => c.Name).IsRequired().HasMaxLength(30);

        //one to many relation build
        builder.HasOne(c => c.Section).WithMany().HasForeignKey(c => c.SectionId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Room).WithMany().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Library).WithMany().HasForeignKey(c => c.LibraryId).OnDelete(DeleteBehavior.NoAction);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
