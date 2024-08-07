//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;


//namespace DataLayer.TemplateConfiguration.Air;

//public class TestModelConfiguration : IEntityTypeConfiguration<TestModel>
//{

//    public void Configure(EntityTypeBuilder<TestModel> builder)
//    {

//        builder.HasKey(c => c.Id);
//        builder.Property(c => c.Id).HasMaxLength(450);
//        builder.Property(c => c.AirportName).HasMaxLength(100);
//        builder.Property(c => c.AirportCode).HasMaxLength(10);
//        builder.Property(c => c.Country).HasMaxLength(50);
//        builder.Property(c => c.City).HasMaxLength(30);
//        builder.Property(c => c.IsActive).IsRequired();
//        builder.Property(c => c.RecordDate).IsRequired();


//        //for ApplicationUser
//        builder.HasOne(ss => ss.User)
//         .WithMany()
//         .HasForeignKey(ss => ss.UserId).OnDelete(DeleteBehavior.NoAction);
//    }


//}

