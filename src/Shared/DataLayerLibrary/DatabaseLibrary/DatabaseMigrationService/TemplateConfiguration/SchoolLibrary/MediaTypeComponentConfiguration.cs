using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Library;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// MediaType Model Configuration to design table for Database .
/// </summary>
public class MediaTypeComponentConfiguration : IEntityTypeConfiguration<LibraryMediaTypeModel>
{
    public void Configure(EntityTypeBuilder<LibraryMediaTypeModel> builder)
    {
        //Name has max 30 length
        builder.Property(c => c.Name).IsRequired().HasMaxLength(30);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
