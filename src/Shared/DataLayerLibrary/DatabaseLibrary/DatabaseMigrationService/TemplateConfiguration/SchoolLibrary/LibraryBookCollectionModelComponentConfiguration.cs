using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Library;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Library Book Collection Model Configuration to design table for Database .
/// </summary>
public class LibraryBookCollectionModelComponentConfiguration : IEntityTypeConfiguration<LibraryBookCollectionModel>
{
    public void Configure(EntityTypeBuilder<LibraryBookCollectionModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(30);
        builder.Property(c => c.Abbreviation).IsRequired().HasMaxLength(4);


        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
