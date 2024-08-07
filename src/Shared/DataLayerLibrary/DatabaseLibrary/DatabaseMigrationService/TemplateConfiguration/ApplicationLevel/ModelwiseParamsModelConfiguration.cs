//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;


//namespace DataLayer.TemplateConfiguration.DatabaseSelection;

//public class ModelwiseParamsModelConfiguration : IEntityTypeConfiguration<ModelwiseParamsModel>
//{

//    public void Configure(EntityTypeBuilder<ModelwiseParamsModel> builder)
//    {
//        //Common configuration
//        builder.Property(c => c.Id).IsRequired().HasMaxLength(100);
//        builder.Property(c => c.IsActive).IsRequired();
//        builder.Property(c => c.RecordDate).IsRequired();

//        //Sepecific Configuration
//        builder.Property(c => c.DefaultValue).IsRequired();
//        builder.Property(c => c.CustomValue).IsRequired();
//        builder.Property(c => c.ParamType).IsRequired();



//        //for Application Params Model 
//        builder.HasOne(ss => ss.AppParams)
//            .WithMany()
//            .HasForeignKey(ss => ss.AppParamId).OnDelete(DeleteBehavior.NoAction);

//        //for ApplicationUser
//        builder.HasOne(ss => ss.User)
//            .WithMany()
//            .HasForeignKey(ss => ss.UserId).OnDelete(DeleteBehavior.NoAction);
//    }
//}