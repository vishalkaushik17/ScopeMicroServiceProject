//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;


//namespace DataLayer.TemplateConfiguration.Air;

//public class TestModelConfiguration : IEntityTypeConfiguration<TestModel>
//{

//    public void Configure(EntityTypeBuilder<TestModel> builder)
//    {
//        builder.HasKey(c => c.Id);
//        builder.Property(c => c.Id).HasMaxLength(450);
//        builder.Property(c => c.FromAirportId).HasMaxLength(450);
//        builder.Property(c => c.ToAirportId).HasMaxLength(450);
//        builder.Property(c => c.IsActive).IsRequired();
//        builder.Property(c => c.RecordDate).IsRequired();


//        //for Airport
//        builder.HasOne(ss => ss.FromAirport)
//         .WithMany(s => s.AirFly)
//         .HasForeignKey(ss => ss.FromAirportId).OnDelete(DeleteBehavior.NoAction);

//        //for ApplicationUser
//        builder.HasOne(ss => ss.User)
//         .WithMany()
//         .HasForeignKey(ss => ss.UserId).OnDelete(DeleteBehavior.NoAction);

//    }
//}
