using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelTemplates.Persistence.Models.School.Library;

namespace DBOperationsLayer.TemplateConfiguration.SchoolLibrary;

/// <summary>
/// Bookshelf Model Configuration to design table for Database .
/// </summary>
public class BookShelfModelComponentConfiguration : IEntityTypeConfiguration<LibraryBookshelfModel>
{
    public void Configure(EntityTypeBuilder<LibraryBookshelfModel> builder)
    {
        //Name has max 30 length
        builder.Property(c => c.Name).IsRequired().HasMaxLength(30);

        //one to many relation build
        builder.HasOne(c => c.Rack).WithMany().HasForeignKey(c => c.RackId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Section).WithMany().HasForeignKey(c => c.SectionId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Room).WithMany().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Library).WithMany().HasForeignKey(c => c.LibraryId).OnDelete(DeleteBehavior.NoAction);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}

/// <summary>
/// Bookshelf Model Configuration to design table for Database .
/// </summary>
public class BookMasterModelComponentConfiguration : IEntityTypeConfiguration<LibraryBookMasterModel>
{
    public void Configure(EntityTypeBuilder<LibraryBookMasterModel> builder)
    {

        builder.Property(c => c.Title).IsRequired().HasMaxLength(100);
        builder.Property(c => c.SubTitle).HasMaxLength(100);
        builder.Property(c => c.VolumeNo).HasMaxLength(15);
        builder.Property(c => c.BookImage);
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.Property(c => c.ISBN10).HasMaxLength(12);
        builder.Property(c => c.ISBN13).HasMaxLength(15);
        builder.Property(c => c.Snippet).HasMaxLength(2000);
        builder.Property(c => c.Pages).IsRequired();
        builder.Property(c => c.Price).IsRequired();
        builder.Property(c => c.PublishedDate).IsRequired();


        //one to many relation build
        builder.HasOne(c => c.Language).WithMany().HasForeignKey(c => c.LanguageId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Currency).WithMany().HasForeignKey(c => c.CurrencyId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Publisher).WithMany().HasForeignKey(c => c.PublisherId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.Collection).WithMany().HasForeignKey(c => c.CollectionId).OnDelete(DeleteBehavior.NoAction);

        //Common
        CommonOperationsOnModelConfiguration.CommonModelConfigurations(builder);
    }
}