using BSAuthentication.BsInterface;
using DataCacheLayer.CacheRepositories.Interfaces;
using DataCacheLayer.CacheRepositories.Repositories;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Constants.AppConfig;
using GenericFunction.Constants.Authorization;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EncryptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelTemplates.EntityModels.AppConfig;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.EntityModels.Company;
using ModelTemplates.Master.Company;
using ModelTemplates.Persistence.Models.AppLevel;
using System.Data;
using System.Reflection;
using System.Transactions;
namespace BSAuthentication.BsServices;
public sealed class BsDbInitializerService : IBsDbInitializerContract
{
	private ApplicationDbContext _dbContext;
	private ICacheContract _cacheData;
	private readonly GenericFunction.DefaultSettings.DatabaseHost _databaseHostAppSet;
	private Assembly _assembly;
	//private readonly ApplicationDbContext _dbContext;

	private readonly GenericFunction.DefaultSettings.DatabaseHost _databaseHost;

	IEnumerable<string> _resourceNames;//= new IList<string>();
	private string _dbConnectionString = string.Empty;
	private readonly IHttpContextAccessor? _httpContextAccessor;
	private string? _dbTypeFromSession = string.Empty; //EnvironmentName,
	private EnumDBType _dbType;
	private CompanyDetails _companyDetails;
	private ApplicationSettings _applicationSettings;
	private DateTimeOffset _expirationTime;
	private readonly ITrace _trace;
	private readonly bool _isTracingRequired;
	private CompanyDetails companyDetails;


	public BsDbInitializerService(ApplicationDbContext dbContext,
		 IHttpContextAccessor httpContextAccessor, ITrace trace)
	{
		_dbContext = dbContext;
		_trace = trace;
		_httpContextAccessor = httpContextAccessor;
		_cacheData = new CacheRepositoryService(httpContextAccessor, trace);
		//initializing new db connection string as per the httpContext header
		//dbContext = new ApplicationDbContext(dbContext._Options, httpContextAccessor, trace);
		//first check from session then read from appsettings.json
		_dbTypeFromSession = _httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.DatabaseType);
		_applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
		//read config json as per environment
		_dbTypeFromSession = string.IsNullOrWhiteSpace(_dbTypeFromSession) ? SettingsConfigHelper.AppSetting("Database", "Type") : _dbTypeFromSession;
		//companyDetails = SettingsConfigHelperReadObject<CompanyDetails>.AppSetting(nameof(CompanyDetails)).AppSettingValue;
		_companyDetails = SettingsConfigHelper.GetOptions<CompanyDetails>();
		_databaseHostAppSet = SettingsConfigHelper.GetOptions<DatabaseHost>();
		//what type of database we have chosen for client or coding company 
		_dbType = (EnumDBType)System.Enum.Parse(typeof(EnumDBType), _dbTypeFromSession, true);
		_isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
		//with DBStringName we have created sql file as per database type, so now we are selecting and running those file on database, for adding default records.
		string DbStringName = (_dbType == EnumDBType.PGSQL) || (_dbType == EnumDBType.PGSQLDOCKER) ? "PGSQL" : System.Enum.GetName(typeof(EnumDBType), _dbType);

		//reading assembly name and its file.
		_assembly = Assembly.GetExecutingAssembly();
		//from those assembly we are reading list of files as per database type, which is embedded as resource in assembly project .csproj file.
		_resourceNames = _assembly.GetManifestResourceNames().
		Where(str => str.EndsWith(".sql") && str.Contains(DbStringName)).OrderBy(str => str.ToString());

		//reading connection string for database connection.
		_dbConnectionString = SettingsConfigHelper.AppSetting("ConnectionStrings", _dbTypeFromSession);
	}
	public string InitializeWithDefaults(ApplicationDbContext _dbContext)
	{
		if (_dbContext.Database.GetPendingMigrations().Any())
		{
			_dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
		}

		using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
		{
			try
			{
				foreach (string resourceName in _resourceNames)
				{
					using (Stream stream = _assembly.GetManifestResourceStream(resourceName))
					using (StreamReader reader = new StreamReader(stream))
					{
						string sql = reader.ReadToEnd();
						_dbContext.Database.ExecuteSqlRaw(sql);
					}
				}

				txscope.Complete();

			}
			catch (Exception ex)
			{
				ex.SendExceptionMailAsync().GetAwaiter().GetResult();
				// Log error
				txscope.Dispose();
				return "Default records not inserted due to error " + ex.Message;
			}

			return "Migration with default records done!";
		}
	}
	public string InitializeOnNewDb(ApplicationDbContext _dbContext)
	{
		if (_dbContext.Database.GetPendingMigrations().Any())
		{
			_dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
		}

		foreach (string resourceName in _resourceNames)
		{
			using (Stream stream = _assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				string sql = reader.ReadToEnd();
				_dbContext.Database.ExecuteSqlRawAsync(sql).GetAwaiter().GetResult();
			}
		}

		return "Migration done with triggers and functions!";
	}

	/// <summary>
	/// First migration method for the CC application.
	/// </summary>
	/// <returns>message as string.</returns>
	public string Initialize()
	{


		using (var transaction = _dbContext.Database.BeginTransaction())

		{
			InitializeOnNewDb(_dbContext);
			try
			{

				string? compTypeId = string.Empty;
				CompanyMasterModel? firstCompanyRecord = new CompanyMasterModel();



				//var companyTypes = await _cacheData.ReadFromCacheAsync<CompanyTypeModel>(string.Format(CacheKeys.CompanyTypes, "Default"), 7, true);
				//if (companyTypes == null)
				//{
				//    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.ToCss(), $"Reading users from database!");
				//    companyTypes = _dbContext.CompanyTypes.ToList();

				//    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.ToCss(), "Reading completed!");
				//    _expirationTime = DateTimeOffset.Now.AddMinutes(_applicationSettings.ModuleCacheSettings.CompanyTypes);
				//    await _cacheData.AddCacheAsync(CacheKeys.CompanyTypes.Replace("{0}", "Default"), companyTypes, _expirationTime);
				//}

				var companyTypes = _dbContext.CompanyTypes.ToListAsync().GetAwaiter().GetResult();
				if (companyTypes == null || !companyTypes.Any(m => m.TypeName == EnumCompanyTypes.MASTER.GetDisplayNameOfEnum()))
				{
					CompanyTypeModel[] comps = new[]
					{
				new CompanyTypeModel().Save( "0",
					 "DEFAULT",
					 EnumCompanyTypes.MASTER.GetDisplayNameOfEnum()),

				new CompanyTypeModel().Save(  "1",
					"DEFAULT",
					EnumCompanyTypes.AGENCY.GetDisplayNameOfEnum()),

				new CompanyTypeModel().Save( "2",
					"DEFAULT",
					EnumCompanyTypes.SUBAGENCY.GetDisplayNameOfEnum()),

				new CompanyTypeModel().Save("3",
					"DEFAULT",
					EnumCompanyTypes.SCHOOL.GetDisplayNameOfEnum()),

			};

					_dbContext.CompanyTypes.AddRangeAsync(comps).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();
					companyTypes = _dbContext.CompanyTypes.ToListAsync().GetAwaiter().GetResult();
					//_expirationTime = _applicationSettings.ModuleCacheSettings.CompanyTypes.GetKeyLifeForCacheStorage();
					//_cacheData.AddCacheAsync(string.Format(CacheKeys.CompanyTypes, "Common"), companyTypes, _expirationTime).GetAwaiter().GetResult();

				}

				//get comptypeid for company type default record.

				//var companyMasters = await _cacheData.ReadFromCacheAsync<CompanyMasterModel>(string.Format(CacheKeys.CompanyMaster, "Default"), 7, true);
				//if (companyMasters == null)
				//{
				//    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.ToCss(), $"Reading users from database!");

				//    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.ToCss(), "Reading completed!");
				//}
				var companyMasters = _dbContext.CompanyMasters.ToListAsync().GetAwaiter().GetResult();
				compTypeId = companyTypes.FirstOrDefault(m => m.TypeName == EnumCompanyTypes.MASTER.GetDisplayNameOfEnum())?.Id;
				if (!string.IsNullOrWhiteSpace(compTypeId) && !companyMasters.Any(m => m.Name == _companyDetails.Name))
				{
					//create default master company account

					firstCompanyRecord.Save(
					"0",
					"DEFAULT",
					DateTime.Now,
					DateTime.Now,
					DateTime.Now,
					compTypeId,
					suffixDomain: _companyDetails.SuffixDomain,
					_companyDetails.Name,
					_companyDetails.ContactEmail,
					DateTime.Now,
					false,
					"Default",
					false,
					false,
					EnumRecordStatus.Active,
					_companyDetails.Website, string.Empty);

					_dbContext.CompanyMasters.AddRangeAsync(firstCompanyRecord).GetAwaiter().GetResult();

					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();
					companyMasters = _dbContext.CompanyMasters.ToListAsync().GetAwaiter().GetResult();
					//_expirationTime = _applicationSettings.ModuleCacheSettings.CompanyMaster.GetKeyLifeForCacheStorage();
					//_cacheData.AddCacheAsync(string.Format(CacheKeys.CompanyMaster, "Common"), companyMasters, _expirationTime).GetAwaiter().GetResult();
				}



				//get company id for default record.
				firstCompanyRecord = companyMasters.FirstOrDefault(m => m.Name == _companyDetails.Name);

				if (firstCompanyRecord != null && companyMasters.Any(m => m.Id == firstCompanyRecord.Id))
				{
					//read database setting from AppJsonSettings for saving CC company profile.

					//var connectionString = IEncryptionService.Encrypt(_databaseHostAppSet.ConnectionString, _applicationSettings.CacheServer.HashKey);
					var username = IEncryptionService.Encrypt(_databaseHostAppSet.Username, _applicationSettings.CacheServer.HashKey);
					var domain = IEncryptionService.Encrypt(_databaseHostAppSet.Domain, _applicationSettings.CacheServer.HashKey);
					var databaseName = IEncryptionService.Encrypt(_databaseHostAppSet.DatabaseName, _applicationSettings.CacheServer.HashKey);

					//adding default company profile record.
					var companyProfile = new CompanyMasterProfileModel();
					companyProfile.Save(
						firstCompanyRecord.Id, 2, 0, 4, domain, _companyDetails.Name, "Default", username, DateTime.Now, false,
						EnumRecordStatus.Active, databaseName);
					_dbContext.CompanyMasterProfiles.AddAsync(companyProfile).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					var companyProfiles = _dbContext.CompanyMasterProfiles.ToListAsync().GetAwaiter().GetResult();
					//_expirationTime = _applicationSettings.ModuleCacheSettings.CompanyMasterProfile.GetKeyLifeForCacheStorage();
					// _cacheData.AddCacheAsync(string.Format(CacheKeys.CompanyProfiles, firstCompanyRecord.Id), companyProfiles, _expirationTime).GetAwaiter().GetResult();

				}



				var userRolesList = _dbContext.Roles.ToListAsync().GetAwaiter().GetResult();

				//checking roles are added or not
				if (userRolesList.FirstOrDefault(m => m.Name == RoleName.Administrator) == null)
				{
					UserRoles userRoles = new UserRoles();

					var allRoles = GenericFunction.ExtensionMethods.GetFieldValues(new RoleName());
					int iCount = 1;
					foreach (var role in allRoles)
					{
						userRoles = new UserRoles();
						userRoles.CompanyId = firstCompanyRecord.Id;
						userRoles.Save(iCount.ToString(), role.Value);
						//_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();
						_dbContext.Roles.Add(userRoles);
						iCount++;
					}
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					userRolesList = _dbContext.Roles.ToListAsync().GetAwaiter().GetResult();
					//_expirationTime = _applicationSettings.ModuleCacheSettings.Roles.GetKeyLifeForCacheStorage();
					//_cacheData.AddCacheAsync(string.Format(CacheKeys.Roles, "Default"), userRolesList, _expirationTime).GetAwaiter().GetResult();

				}
				ApplicationUser usr = new ApplicationUser();
				string masterUserId = string.Empty;
				string roleId = string.Empty;

				//get existing users list.
				var users = _dbContext.Users.ToListAsync().GetAwaiter().GetResult();
				if (users.FirstOrDefault(m => m.UserName == "master@cc.in") == null)
				{
					//Create user and add theirs default roles
					usr.Save("DEFAULT", "master@cc.in", firstCompanyRecord.Id, compTypeId, "vishalkaushik@hotmail.com", "MASTER", "USER", "vishalkaushik@hotmail.com");
					usr.PasswordHash = GenericFunction.ExtensionMethods.HashPassword("Kavya392010@");
					usr.ConfirmEmail();
					_dbContext.User.Add(usr);
					_dbContext.SaveChanges();//Async().GetAwaiter().GetResult();)

					//now get the first userid when added
					var defaultUser = _dbContext.Users.FirstOrDefaultAsync(m => m.CompanyId == firstCompanyRecord.Id).GetAwaiter().GetResult();
					roleId = userRolesList.FirstOrDefault(m => m.Name == RoleName.Master).Id;
					IdentityUserRole<string> userRoles = new();
					userRoles.UserId = defaultUser.Id;
					userRoles.RoleId = roleId;
					masterUserId = defaultUser.Id;//  _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					_dbContext.UserRoles.AddAsync(userRoles).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();



					//add roles to master user
					//_userManager.AddToRolesAsync(defaultUser, new List<string>() { RoleName.Master }).GetAwaiter().GetResult();


					usr = new ApplicationUser();
					usr.Save(masterUserId, "tester@cc.in", firstCompanyRecord.Id, compTypeId, "vishalkaushik@hotmail.com", "TEST", "USER", "vishalkaushik@hotmail.com");
					usr.PasswordHash = GenericFunction.ExtensionMethods.HashPassword("Kavya392010@");
					usr.ConfirmEmail();
					_dbContext.User.AddAsync(usr).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					defaultUser = _dbContext.Users.FirstOrDefaultAsync(m => m.UserName == "tester@cc.in").GetAwaiter().GetResult();

					//add roles to tester user
					roleId = userRolesList.FirstOrDefault(m => m.Name == RoleName.Tester).Id;
					userRoles = new();
					userRoles.UserId = defaultUser.Id;
					userRoles.RoleId = roleId;
					masterUserId = defaultUser.Id;//  _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					_dbContext.UserRoles.AddAsync(userRoles).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();


					usr = new ApplicationUser();
					usr.Save(masterUserId, "developer@cc.in", firstCompanyRecord.Id, compTypeId, "vishalkaushik@hotmail.com", "DEVELOPER", "USER", "vishalkaushik@hotmail.com");
					usr.PasswordHash = GenericFunction.ExtensionMethods.HashPassword("Kavya392010@");
					usr.ConfirmEmail();
					_dbContext.User.AddAsync(usr).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();


					defaultUser = _dbContext.Users.FirstOrDefault(m => m.UserName == "developer@cc.in");

					//add roles to developer user
					roleId = userRolesList.FirstOrDefault(m => m.Name == RoleName.Developer).Id;
					userRoles = new();
					userRoles.UserId = defaultUser.Id;
					userRoles.RoleId = roleId;
					masterUserId = defaultUser.Id;//  _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					_dbContext.UserRoles.AddAsync(userRoles).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					usr = new ApplicationUser();
					usr.Save(masterUserId, "admin@cc.in", firstCompanyRecord.Id, compTypeId, "vishalkaushik@hotmail.com", "ADMIN", "USER", "vishalkaushik@hotmail.com");
					usr.PasswordHash = GenericFunction.ExtensionMethods.HashPassword("Kavya392010@");
					usr.ConfirmEmail();
					_dbContext.User.AddAsync(usr).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();
					defaultUser = _dbContext.Users.FirstOrDefault(m => m.UserName == "admin@cc.in");

					//add roles to admin user
					//add roles to developer user
					roleId = userRolesList.FirstOrDefault(m => m.Name == RoleName.Administrator).Id;
					userRoles = new();
					userRoles.UserId = defaultUser.Id;
					userRoles.RoleId = roleId;
					masterUserId = defaultUser.Id;//  _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					_dbContext.UserRoles.AddAsync(userRoles).GetAwaiter().GetResult();
					_dbContext.SaveChangesAsync().GetAwaiter().GetResult();

					users = _dbContext.User.ToListAsync().GetAwaiter().GetResult();
					// _expirationTime = _applicationSettings.ModuleCacheSettings.Users.GetKeyLifeForCacheStorage();
					// _cacheData.AddCacheAsync(string.Format(CacheKeys.Users, "Default"), users, _expirationTime).GetAwaiter().GetResult();

				}

				//checking that Application host master has data or not.
				var dbHostRecords = _dbContext.ApplicationHostMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync().GetAwaiter().GetResult();

				// when no records then add default host master model record for database deployment 
				if (dbHostRecords?.Count == 0)
				{



					var connectionString = IEncryptionService.Encrypt(_databaseHostAppSet.ConnectionString, _applicationSettings.CacheServer.HashKey);
					var username = IEncryptionService.Encrypt(_databaseHostAppSet.Username, _applicationSettings.CacheServer.HashKey);
					var domain = IEncryptionService.Encrypt(_databaseHostAppSet.Domain, _applicationSettings.CacheServer.HashKey);


					ApplicationHostMasterModel? dbHostRecord = new();

					dbHostRecord.ConnectionString = connectionString;
					dbHostRecord.UserName = username;
					dbHostRecord.Domain = domain;
					dbHostRecord.HashString = _applicationSettings.CacheServer.HashKey;
					dbHostRecord.DatabaseType = _databaseHostAppSet.DbType;
					_dbContext.ApplicationHostMasters.Add(dbHostRecord);
					_dbContext.SaveChanges();

					var apphostMasters = _dbContext.ApplicationHostMasters.ToListAsync().GetAwaiter().GetResult();

					//_expirationTime = _applicationSettings.ModuleCacheSettings.ApplicationHostMaster.GetKeyLifeForCacheStorage();
					//_cacheData.AddCacheAsync(string.Format(CacheKeys.ApplicationHosts, "Common"), apphostMasters, _expirationTime).GetAwaiter().GetResult();

					dbHostRecord = apphostMasters.FirstOrDefault(m => m.HashString == _applicationSettings.CacheServer.HashKey);
					if (dbHostRecord != null)
					{
						AppDBHostVsCompanyMaster dbVsComp = new();
						dbVsComp.AppHostId = dbHostRecord.Id;
						dbVsComp.CompanyMasterId = firstCompanyRecord.Id;
						dbVsComp.Save(masterUserId);
						_dbContext.AppDbHostVsCompanyMasters.Add(dbVsComp);
						_dbContext.SaveChanges();


						var appHostVsDbMasters = _dbContext.AppDbHostVsCompanyMasters.ToListAsync().GetAwaiter().GetResult();

						//_expirationTime = _applicationSettings.ModuleCacheSettings.AppDBHostVsCompanyMaster.GetKeyLifeForCacheStorage();
						//_cacheData.AddCacheAsync(string.Format(CacheKeys.AppDBHostVsCompanyMasters, "Common"), appHostVsDbMasters, _expirationTime).GetAwaiter().GetResult();

					}
				}
				UpdateChangesToDb(_dbContext);
				transaction.CommitAsync().GetAwaiter().GetResult();
			}
			catch (Exception ex)
			{
				ex.SendExceptionMailAsync().GetAwaiter().GetResult();
				// Log error
				transaction.RollbackAsync().GetAwaiter().GetResult();
				return "Default records not inserted due to error " + ex.Message;
			}



			////var dbHostingRecords = _dbContext.ApplicationHostMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToList();

			//var schoolProfile =
			//    _dbContext.AppDbHostVsCompanyMasters.Include(m => m.CompanyMaster.CompanyMasterProfile).Include(a => a.ApplicationHostMaster).ToList();
			//// Dictionary<string, string> hostCom = new Dictionary<string, string>();
			//foreach (var hostRecord in schoolProfile)
			//{
			//    var dbdeploymentUsername =
			//        IEncryptionService.Decrypt(hostRecord.ApplicationHostMaster.UserName, hostRecord.ApplicationHostMaster.HashString);
			//    var dbDeploymentPassword =
			//        IEncryptionService.Decrypt(hostRecord.ApplicationHostMaster.Password, hostRecord.ApplicationHostMaster.HashString);
			//    //it will add
			//    var newDBConectionString =
			//        $"server={hostRecord.ApplicationHostMaster.Domain};user={dbdeploymentUsername};password={dbDeploymentPassword};" +
			//        $"database={hostRecord.CompanyMaster.CompanyMasterProfile.DatabaseName};port={hostRecord.ApplicationHostMaster.Port};";

			//    //just updating static class for domain and connection string.
			//    HostDBRecords.CompanyVsDBHosts.Add(hostRecord.CompanyMaster.SuffixDomain, newDBConectionString);

			//}

			return "Database created successful! ";

		}
	}
	//It will execute first time in default database for inserting or updating default preferences records.
	public List<SystemPreferencesModel> GetDefaultPreferences(ApplicationDbContext dbContext)
	{
		return _dbContext.SystemPreferences.ToListAsync().GetAwaiter().GetResult();

	}

	/// <summary>
	/// Adding default values for system preferences.
	/// </summary>
	/// <param name="systemPreferences">SystemPreference object</param>
	/// <param name="dbContext">Application db context</param>
	/// <returns>When adding succeeded it return true.</returns>
	public bool AddDefaultSystemPreferences(List<SystemPreferencesModel> systemPreferences, ApplicationDbContext dbContext)
	{
		try
		{
			_dbContext = dbContext;
			foreach (var item in systemPreferences)
			{
				_dbContext.SystemPreferences.AddAsync(item).GetAwaiter().GetResult();
				_dbContext.SaveChangesAsync().GetAwaiter().GetResult();
			}
		}
		catch (Exception)
		{

			return false;
		}

		return true;
	}

	/// <summary>
	/// Updating default preference value when master database get created.
	/// </summary>
	/// <param name="_dbContext">Application Db context object</param>
	/// <returns>success message as string.</returns>
	public string UpdateChangesToDb(ApplicationDbContext _dbContext)
	{
		var defaultPreferences = _dbContext.SystemPreferences.ToListAsync().GetAwaiter().GetResult();

		//Preference Name : ResultPerPage
		var resultPerPage = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.ResultPerPage, StringComparison.CurrentCultureIgnoreCase));

		var defaultSystemPreferences = new SystemPreferencesModel();

		if (resultPerPage == null)
		{
			defaultSystemPreferences.PreferenceName = AppPreferences.ResultPerPage;
			defaultSystemPreferences.DefaultValue = "100";
			defaultSystemPreferences.CustomValue = "0";
			defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Int;

			defaultSystemPreferences.Description = "Display records per page.";
			defaultSystemPreferences.ModuleName = "All";
			defaultSystemPreferences.Save("Default");

			_dbContext.SystemPreferences.AddAsync(defaultSystemPreferences).GetAwaiter().GetResult();
			_dbContext.SaveChangesAsync().GetAwaiter().GetResult();
		}


		//Preference Name : ModificationAllowsInDays
		var modificationAllowsInDays = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.ModificationAllowsInDays, StringComparison.CurrentCultureIgnoreCase));// == "ModificationAllowsInDays");

		defaultSystemPreferences = new SystemPreferencesModel();

		if (modificationAllowsInDays == null)
		{
			defaultSystemPreferences.PreferenceName = AppPreferences.ModificationAllowsInDays;
			defaultSystemPreferences.DefaultValue = "7";
			defaultSystemPreferences.CustomValue = "0";
			defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Int;

			defaultSystemPreferences.Description = "Modification allows in days. Eg. Update/Delete record.";
			defaultSystemPreferences.ModuleName = "All";
			defaultSystemPreferences.Save("Default");

			_dbContext.SystemPreferences.AddAsync(defaultSystemPreferences).GetAwaiter().GetResult();
			_dbContext.SaveChangesAsync().GetAwaiter().GetResult();
		}


		//Preference Name : AllowWriteOperationUptoDays
		var allowWriteOperationUptoDays = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.AllowWriteOperationUptoDays, StringComparison.CurrentCultureIgnoreCase));// == "ModificationAllowsInDays");

		defaultSystemPreferences = new SystemPreferencesModel();

		if (allowWriteOperationUptoDays == null)
		{
			defaultSystemPreferences.PreferenceName = AppPreferences.AllowWriteOperationUptoDays;
			defaultSystemPreferences.DefaultValue = "7";
			defaultSystemPreferences.CustomValue = "0";
			defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Int;

			defaultSystemPreferences.Description = "Allow Read/Write operation within allow days when demo get expired.";
			defaultSystemPreferences.ModuleName = "All";
			defaultSystemPreferences.Save("Default");

			_dbContext.SystemPreferences.Add(defaultSystemPreferences);
			_dbContext.SaveChanges();
		}

		//Preference Name : AllowLoginAfterExipredUptoDays
		var allowLoginAfterExipredUptoDays = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.AllowLoginAfterExipredUptoDays, StringComparison.CurrentCultureIgnoreCase));

		defaultSystemPreferences = new SystemPreferencesModel();

		if (allowLoginAfterExipredUptoDays == null)
		{
			defaultSystemPreferences.PreferenceName = AppPreferences.AllowLoginAfterExipredUptoDays;
			defaultSystemPreferences.DefaultValue = "7";
			defaultSystemPreferences.CustomValue = "0";
			defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Int;

			defaultSystemPreferences.Description = "Allow login when subscription is expired within allow days.";
			defaultSystemPreferences.ModuleName = "All";
			defaultSystemPreferences.Save("Default");

			_dbContext.SystemPreferences.Add(defaultSystemPreferences);
			_dbContext.SaveChanges();
		}


		//Preference Name : BypassCache
		var bypassCache = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.BypassCache, StringComparison.CurrentCultureIgnoreCase));

		defaultSystemPreferences = new SystemPreferencesModel();

		if (bypassCache == null)
		{
			defaultSystemPreferences.PreferenceName = AppPreferences.BypassCache;
			defaultSystemPreferences.DefaultValue = "false";
			defaultSystemPreferences.CustomValue = "";
			defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Bool;

			defaultSystemPreferences.Description = "It will bypass the cache server for all clients, if its creating trouble on CRUD operation.";
			defaultSystemPreferences.ModuleName = "All";
			defaultSystemPreferences.Save("Default");

			_dbContext.SystemPreferences.Add(defaultSystemPreferences);
			_dbContext.SaveChanges();
		}


		List<SystemPreferenceForSession> systemPreferences = _dbContext.SystemPreferences.
																	Where(m => m.RecordStatus == GenericFunction.Enums.EnumRecordStatus.Active)
																	.Select(m => new SystemPreferenceForSession
																	{
																		PreferenceName = m.PreferenceName,
																		Value = m.CustomValue.ToLower() == "n/a" || m.CustomValue == "0" ? m.DefaultValue : m.CustomValue,
																		ModuleName = m.ModuleName,
																		ValueType = m.ValueType,

																	}).ToList();




		return "Database updated!";

	}
}