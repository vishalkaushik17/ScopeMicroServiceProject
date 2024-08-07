using DataCacheLayer.CacheRepositories.Repositories;
using DBOperationsLayer.Data.Constants;
using DBOperationsLayer.TemplateConfiguration.ApplicationLevel;
using DBOperationsLayer.TemplateConfiguration.EmployeeStudentParent;
using DBOperationsLayer.TemplateConfiguration.Inventory;
using DBOperationsLayer.TemplateConfiguration.SchoolLibrary;
using GenericFunction;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EncryptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelTemplates.Core.Model;
using ModelTemplates.EntityModels.AppConfig;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.EntityModels.Company;
using ModelTemplates.EntityModels.DatabaseConfig;
using ModelTemplates.EntityModels.UserAccount;
using ModelTemplates.Master.Company;
using ModelTemplates.Persistence.Component.School.Library;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.Persistence.Models.School.CommonModels;
using ModelTemplates.Persistence.Models.School.Employee;
using ModelTemplates.Persistence.Models.School.Inventory;
using ModelTemplates.Persistence.Models.School.Library;
using ModelTemplates.RequestNResponse.Accounts;
using static GenericFunction.CommonMessages;


namespace DBOperationsLayer.Data.Context;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbContextOptions<ApplicationDbContext> _Options { get; }
    protected ApplicationSettings _applicationSettings;
    private List<DbConnectionStringRecord>? domainList;
    private CacheRepositoryService _cacheData;
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly ITrace _trace;

    private readonly bool _isTracingRequired;

    public EnumDBType? _dbTypeFromSession;// = string.Empty;
    private DbConnectionStringRecord? _dbConn;
    //    public EnumDBType? _dbType;
    private bool applicationDbContextReinit = false;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, ITrace trace)
        : base(options)
    {
        _Options = options;
        _httpContextAccessor = httpContextAccessor;
        this._trace = trace;


        _cacheData = new CacheRepositoryService(httpContextAccessor, trace);
        _isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");

        //read application related default settings
        _applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
        //_dbTypeFromSession = _httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.DatabaseType);
        _dbTypeFromSession = _httpContextAccessor?.HttpContext?.GetContextItemAsJson<EnumDBType>(ContextKeys.DatabaseType);
        //when _dbTypeFromSession is null or 0 it mean jwt is not performed its operation.
        if (_dbTypeFromSession == null || _dbTypeFromSession == 0)
        {
            //read default connection type from appsettings.
            _dbTypeFromSession = SettingsConfigHelper.AppSetting("Database", "Type").ToEnum<EnumDBType>();
        }

        //when _dbType is null

        //bool? firstInit = _httpContextAccessor?.HttpContext?.GetContextItemAsJson<bool>(ContextKeys.FirstInit);
        //if (firstInit != null && firstInit == true)
        //{
        //    _httpContextAccessor?.HttpContext?.SetContextItemAsJson(ContextKeys.FirstInit, false);
        //    //if (_dbTypeFromSession == null)

        //    //DbContextOptionsBuilder optionBuilder = new DbContextOptionsBuilder();
        //    //base.OnConfiguring(optionBuilder);
        //    //OnConfiguring(optionBuilder);
        //    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Application Db Context reinitialized".Information());
        //    //_ = new ApplicationDbContext(this._Options, httpContextAccessor, trace);
        //    //}

        //    //_dbType = (EnumDBType)Enum.Parse(typeof(EnumDBType), _dbTypeFromSession, true);


        //}
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Application Db Context execution started!".MarkInformation());
    }


    /// <summary>
    /// OnConfiguring protected method
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {


        bool dbStringFound = false;
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Execution started!".MarkInformation());

        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
        {
            _dbConn = _httpContextAccessor.HttpContext.GetContextItemAsJson<DbConnectionStringRecord>(ContextKeys.dbConnectionString);
            if (_dbConn != null)
            {
                dbStringFound = true;
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "OnConfiguring".MarkProcess(), $"dbConnectionString {_dbConn.SuffixName} found in httpContext!".MarkInformation());

                if (_dbConn != null)
                {
                    switch (_dbConn.DbType)
                    {
                        case EnumDBType.MYSQL:
                            optionsBuilder.UseMySql(IEncryptionService.Decrypt(_dbConn.DbConnectionString, _applicationSettings.CacheServer.HashKey),
                            new MySqlServerVersion(new Version(8, 0, 29)));

                            break;
                        case EnumDBType.PGSQL:
                        case EnumDBType.PGSQLDOCKER:

                            optionsBuilder.UseNpgsql(IEncryptionService.Decrypt(_dbConn.DbConnectionString, _applicationSettings.CacheServer.HashKey)
                                );
                            //optionsBuilder.UseNpgsql(
                            //           dbConnectionFromRedis,
                            //           options => options.EnableRetryOnFailure(
                            //               maxRetryCount: 3,
                            //               maxRetryDelay: TimeSpan.FromMilliseconds(100),
                            //               errorCodesToAdd: null)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                            break;
                        default:
                            break;
                    }
                }
            }
        }
        if (!dbStringFound)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "OnConfiguring".MarkProcess(), $"ConnectionString not found for httpContext type, reading from appsettings.");
            DbConnection.ConnectionString = SettingsConfigHelper.AppSetting("ConnectionStrings", Key: Enum.GetName(typeof(EnumDBType), _dbTypeFromSession));
            switch (_dbTypeFromSession)
            {
                case EnumDBType.MYSQL:
                    optionsBuilder.UseMySql(DbConnection.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 29)));
                    break;
                case EnumDBType.PGSQL:
                case EnumDBType.PGSQLDOCKER:
                    optionsBuilder.UseNpgsql(DbConnection.ConnectionString);
                    break;
                default:
                    break;
            }
        }
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"OnConfiguring : Connection established for Application Db Context, execution completed!".MarkInformation());
    }
    //System Level model configuration
    public DbSet<SystemPreferencesModel> SystemPreferences { get; set; }
    public DbSet<Sequence> SequenceMaster { get; set; }

    //Models related to Coding Company
    public DbSet<CompanyTypeModel> CompanyTypes { get; set; }
    public DbSet<CompanyMasterModel> CompanyMasters { get; set; }
    public DbSet<ApplicationHostMasterModel> ApplicationHostMasters { get; set; }
    public DbSet<AccountConfirmationModel> AccountConfirmations { get; set; }

    public DbSet<AppDBHostVsCompanyMaster> AppDbHostVsCompanyMasters { get; set; }
    public DbSet<CompanyMasterProfileModel> CompanyMasterProfiles { get; set; }
    //Demo Request Model
    public DbSet<DemoRequestModel> DemoRequests { get; set; }

    public DbSet<ApplicationUser> User { get; set; }
    //public DbSet<IdentityRole> Role { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserRoles> UserRolesMaster { get; set; }
    public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
    public DbSet<DatabaseConnection> DatabaseConnections { get; set; }
    public DbSet<SequenceGenerator> SequenceGenerators { get; set; }
    public DbSet<EmailMaster> EmailMasters { get; set; }

    //employee module
    public DbSet<DepartmentModel> DepartmentMaster { get; set; }
    public DbSet<DesignationModel> DesignationMaster { get; set; }
    public DbSet<BankModel> BankMaster { get; set; }
    public DbSet<DegreeModel> DegreeMaster { get; set; }
    public DbSet<AddressModel> Addresses { get; set; }
    public DbSet<EmployeeQualificationModel> EmployeeQualifications { get; set; }

    public DbSet<EmployeeStudentParentModel> EmployeeStudentParentMaster { get; set; }



    //school library related component

    public DbSet<SchoolLibraryHallModel> SchoolLibraries { get; set; }
    public DbSet<LibraryAuthorModel> LibraryAuthors { get; set; }
    public DbSet<LibraryMediaTypeModel> LibraryMediaTypes { get; set; }
    public DbSet<LibraryRoomModel> LibraryRooms { get; set; }
    public DbSet<LibrarySectionModel> LibrarySections { get; set; }
    public DbSet<LibraryRackModel> LibraryRacks { get; set; }
    public DbSet<LibraryBookshelfModel> LibraryBookshelves { get; set; }
    public DbSet<LibraryBookCollectionModel> LibraryBookCollections { get; set; }
    public DbSet<LibraryBookMasterModel> LibraryBookMasters { get; set; }


    //common model declarations
    public DbSet<VendorModel> Vendors { get; set; }
    public DbSet<CurrencyModel> Currencies { get; set; }
    public DbSet<LanguageModel> Languages { get; set; }


    //Inventory management system
    public DbSet<ProductModel> Products { get; set; }
    //private void RunPGScript(ModelBuilder migrationBuilder)
    //{
    //    var assembly = Assembly.GetExecutingAssembly();
    //    var resourceNames = assembly.GetManifestResourceNames().
    //        Where(str => str.EndsWith(".sql") && str.StartsWith(assembly.GetName() + "PGSQL_"));
    //    foreach (string resourceName in resourceNames)
    //    {
    //        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
    //        using (StreamReader reader = new StreamReader(stream))
    //        {
    //            string sql = reader.ReadToEnd();
    //            Database.ExecuteSqlRaw(sql);
    //            migrationBuilder.sq
    //            //migrationBuilder.Sql(sql);
    //        }
    //    }
    //}
    protected override void OnModelCreating(ModelBuilder builder)
    {


        //handing database type configuration while creating modal on database.
        //if (_context.Database.GetPendingMigrations().Any())
        //{
        //    await _context.Database.MigrateAsync();
        //}
        switch (_dbTypeFromSession)
        {
            case EnumDBType.PGSQL:
            case EnumDBType.PGSQLDOCKER:
                builder.ApplyUtcDateTimeConverter();//Put before seed data and after model creation
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "OnModelCreating".MarkProcess(), $"Adding migration to db : {_dbTypeFromSession}");
                break;
            default:
                break;
        }



        //System preference configuration
        builder.ApplyConfiguration(new SequenceMasterModelConfiguration());
        builder.ApplyConfiguration(new SystemPreferencesModelConfiguration());

        //company related configurations

        builder.ApplyConfiguration(new CompanyTypeModelConfiguration());
        builder.ApplyConfiguration(new CompanyMasterModelConfiguration());

        //builder.Entity<ApplicationHostMasterEntityModel>().ToTable(nameof(ApplicationHostMasterEntityModel), t => t.ExcludeFromMigrations());

        builder.ApplyConfiguration(new ApplicationHostMasterModelConfiguration());
        builder.ApplyConfiguration(new ApplicationHostMasterVSCompanyMasterModelConfiguration());
        builder.ApplyConfiguration(new CompanyMasterProfilesModelConfiguration());

        //for Demo Request Model Configuration
        builder.ApplyConfiguration(new DemoRequestModelConfiguration());
        ////app level configurations
        builder.ApplyConfiguration(new SequenceGeneratorModelConfiguration());
        builder.ApplyConfiguration(new ApplicationUserModelConfiguration());
        builder.ApplyConfiguration(new AccountConfirmationModelConfiguration());
        builder.ApplyConfiguration(new UserRolesModelConfiguration());
        builder.ApplyConfiguration(new ApplicationUserTokenModelConfiguration());
        builder.ApplyConfiguration(new EmailMasterModelConfiguration());
        builder.ApplyConfiguration(new RefreshTokenModelConfiguration());

        ////db level configurations
        builder.ApplyConfiguration(new DatabaseConnectionModelConfiguration());

        //employee model related configuration

        builder.ApplyConfiguration(new EmployeeStudentParentMasterComponentConfiguration());
        builder.ApplyConfiguration(new EmployeeQualificationsModelComponentConfiguration());
        builder.ApplyConfiguration(new BankModelComponentConfiguration());
        builder.ApplyConfiguration(new DepartmentMasterModelComponentConfiguration());
        builder.ApplyConfiguration(new DesignationsModelComponentConfiguration());
        builder.ApplyConfiguration(new DegreeModelComponentConfiguration());
        builder.ApplyConfiguration(new AddressMasterModelComponentConfiguration());
        //School Related Configuration

        builder.ApplyConfiguration(new SchoolLibraryComponentConfiguration());
        builder.ApplyConfiguration(new AuthorComponentConfiguration());
        builder.ApplyConfiguration(new MediaTypeComponentConfiguration());
        builder.ApplyConfiguration(new RoomModelComponentConfiguration());
        builder.ApplyConfiguration(new SectionModelComponentConfiguration());
        builder.ApplyConfiguration(new RackModelComponentConfiguration());
        builder.ApplyConfiguration(new BookShelfModelComponentConfiguration());
        builder.ApplyConfiguration(new SectionModelComponentConfiguration());
        builder.ApplyConfiguration(new LibraryBookCollectionModelComponentConfiguration());
        builder.ApplyConfiguration(new LanguageModelComponentConfiguration());
        builder.ApplyConfiguration(new CurrencyModelComponentConfiguration());
        builder.ApplyConfiguration(new BookMasterModelComponentConfiguration());

        //common model configurations
        builder.ApplyConfiguration(new VendorModelComponentConfiguration());
        builder.ApplyConfiguration(new CurrencyModelComponentConfiguration());
        builder.ApplyConfiguration(new LanguageModelComponentConfiguration());

        //Inventory model configurations
        builder.ApplyConfiguration(new ProductModelComponentConfiguration());

        //don't delte below line, it is used for IdenityUser to build default table
        //builder.Seed();

        base.OnModelCreating(builder);
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "OnModelCreating".MarkProcess(), $"Migration completed : {_dbTypeFromSession}");
    }


}


