//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;


//namespace DataLayer.TemplateConfiguration.DatabaseSelection;

//public class ApplicationParamsModelConfiguration : IEntityTypeConfiguration<ApplicationParametersModel>
//{

//    public void Configure(EntityTypeBuilder<ApplicationParametersModel> builder)
//    {
//        //Common configuration
//        builder.Property(c => c.Id).IsRequired().HasMaxLength(100);
//        builder.Property(c => c.IsActive).IsRequired();
//        builder.Property(c => c.RecordDate).IsRequired();

//        //Sepecific Configuration
//        builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
//        builder.Property(c => c.DefaultValue).IsRequired();
//        builder.Property(c => c.ParamType).IsRequired();
//        builder.Property(c => c.Description).IsRequired();
//        builder.Property(c => c.ApplicableFor).IsRequired();


//        //for ApplicationUser
//        builder.HasOne(ss => ss.User)
//            .WithMany()
//            .HasForeignKey(ss => ss.UserId).OnDelete(DeleteBehavior.NoAction);
//    }
//}
