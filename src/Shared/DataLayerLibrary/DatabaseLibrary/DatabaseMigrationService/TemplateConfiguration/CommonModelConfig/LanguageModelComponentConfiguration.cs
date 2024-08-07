using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.CommonModels;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Language Model Configuration to design table for Database .
/// </summary>
public class LanguageModelComponentConfiguration : IEntityTypeConfiguration<LanguageModel>
{
    public void Configure(EntityTypeBuilder<LanguageModel> builder)
    {

        builder.Property(c => c.Name).IsRequired().HasMaxLength(20);
        builder.Property(c => c.NativeName).IsRequired().HasMaxLength(50);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}