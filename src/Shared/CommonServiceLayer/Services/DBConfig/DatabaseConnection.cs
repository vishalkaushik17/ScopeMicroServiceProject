using DBOperationsLayer.Data.Constants;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelTemplates.EntityModels.Application;

namespace SharedLibrary.Services.DBConfig
{
    /// <summary>
    /// This class is identify the database type and configure / build connection as per the type.
    /// </summary>

    public class DatabaseConnection
    {

        /// <summary>
        /// To establish database connection as per the database type.
        /// </summary>
        /// <param name="builder">WebApplicationBuild object</param>
        /// <param name="dbType">databaseType enum value.</param>
        /// <returns>return WebApplicationBuilder object.</returns>
        public WebApplicationBuilder EstablishConnection(WebApplicationBuilder builder, EnumDBType dbType)//, string connectionString, string connectionStringClient)
        {
            DbConnection.ConnectionType = dbType;
            string DbConnectionStringNameInAppSettings = $"ConnectionStrings:{Enum.GetName(typeof(EnumDBType), dbType)}";
            switch (dbType)
            {
                case EnumDBType.MSSQL:

                    DbConnection.ConnectionString = builder.Configuration[DbConnection.MSSQL];
                    DbConnection.ConnectionStringClient = builder.Configuration[DbConnection.MSSQL];
                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration[DbConnection.MSSQL]));

                    break;

                case EnumDBType.PGSQL:

                    DbConnection.ConnectionString = builder.Configuration[DbConnection.PGSQL];
                    DbConnection.ConnectionStringClient = builder.Configuration[DbConnection.PGSQL];

                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseNpgsql(builder.Configuration[DbConnection.PGSQL]);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });
                    break;
                case EnumDBType.PGSQLDOCKER:
                    DbConnection.ConnectionString = builder.Configuration[DbConnectionStringNameInAppSettings];
                    DbConnection.ConnectionStringClient = builder.Configuration[DbConnectionStringNameInAppSettings];
                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseNpgsql(builder.Configuration[DbConnectionStringNameInAppSettings]);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });



                    break;
                case EnumDBType.MYSQL:
                    DbConnection.ConnectionString = builder.Configuration[DbConnection.MYSQL];
                    DbConnection.ConnectionStringClient = builder.Configuration[DbConnection.MYSQL];
                    DbConnection.DbMigration = builder.Configuration[DbConnection.DbMigrationKey];

                    if (DbConnection.DbMigration == "Yes")
                    {

                        //builder.Services.AddTransient<ApplicationDbContext>().AddDbContext<ApplicationDbContext>(options =>
                        //{
                        //    options.UseMySql(DbConnection.ConnectionString,
                        //        new MySqlServerVersion(new Version(8, 0, 29)));
                        //    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        //}, ServiceLifetime.Transient);

                        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                              options.UseMySql(DbConnection.ConnectionString,
                              new MySqlServerVersion(new Version(8, 0, 29))));


                        //builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
                        //{
                        //    options.UseMySql(
                        //        DbConnection.ConnectionString,
                        //        ServerVersion.AutoDetect(DbConnection.ConnectionString),
                        //        options => options.EnableRetryOnFailure(
                        //            maxRetryCount: 5,
                        //            maxRetryDelay: System.TimeSpan.FromSeconds(30),
                        //            errorNumbersToAdd: null));
                        //});

                    }

                    //else
                    //{
                    //    builder.Services.AddTransient<ApplicationDbContext>();
                    //}
                    break;

                case EnumDBType.ORACLE:
                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration[DbConnection.ORACLE]));

                    builder.Services.AddDbContext<ApplicationDbContext>();
                    break;
                default:
                    break;
            }

            //type of dependency inject for applicationDbContext
            builder.Services.AddScoped<ApplicationDbContext>();
            IdentityBuilderExtensions.AddDefaultTokenProviders(builder.Services.AddIdentity<ApplicationUser, UserRoles>()
                .AddEntityFrameworkStores<ApplicationDbContext>());

            return builder;
        }
    }
}
