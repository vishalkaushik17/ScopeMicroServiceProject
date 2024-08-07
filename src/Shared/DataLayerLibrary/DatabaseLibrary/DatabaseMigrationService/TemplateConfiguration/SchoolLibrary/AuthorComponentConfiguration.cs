using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Library;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Author Model Configuration to design table for Database .
/// </summary>
public class AuthorComponentConfiguration : IEntityTypeConfiguration<LibraryAuthorModel>
{
    public void Configure(EntityTypeBuilder<LibraryAuthorModel> builder)
    {
        //Name has max 100 length
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}
