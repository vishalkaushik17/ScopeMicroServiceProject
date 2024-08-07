//using DBOperationsLayer.Data.Context;
//using GenericFunction;
//using GenericFunction.Constants.AppConfig;
//using GenericFunction.Constants.Authorization;
//using GenericFunction.Enums;
//using GenericFunction.ServiceObjects.EncryptionService;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using ModelTemplates.EntityModels.AppConfig;
//using ModelTemplates.EntityModels.Application;
//using ModelTemplates.EntityModels.Company;
//using ModelTemplates.Master.Company;
//using ModelTemplates.Persistence.Models.AppLevel;

//namespace SharedLibrary.Services.Initializer;

//public class DbInitializer : IDbInitializer
//{

//    private readonly UserManager<ApplicationUser> _userManager;
//    private readonly RoleManager<UserRoles> _roleManager;
//    private readonly ApplicationDbContext _dbContext;
//    private readonly IEncryptionService encryptionService;
//    private readonly idatalayerapplication encryptionService;

//    public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<UserRoles> roleManager, ApplicationDbContext dbContext)
//    {
//        //_unitOfWork = unitOfWork;
//        _userManager = userManager;
//        _roleManager = roleManager;
//        _dbContext = dbContext;
//        encryptionService = new EncryptionService();

//    }
//    public void Initialize()
//    {
//        var dbHostName = SettingsConfigHelper.AppSetting("DatabaseHost", "Host");

//        var compMaster = new CompanyMasterModel();
//        if (!_dbContext.CompanyTypes.AnyAsync(m => m.TypeName == EnumCompanyTypes.MASTER.GetDisplayNameOfEnum()).Result)
//        {
//            var comps = new[]
//            {
//                new CompanyTypeModel().Save( "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0",
//                     "DEFAULT",
//                     EnumCompanyTypes.MASTER.GetDisplayNameOfEnum()),

//                new CompanyTypeModel().Save(  "1b23cc53-d9c2-415f-9663-e74a370a2e67",
//                    "DEFAULT",
//                    EnumCompanyTypes.AGENCY.GetDisplayNameOfEnum()),

//                new CompanyTypeModel().Save( "1b5c5535-f91c-45f7-8fd5-8fecddc262e0",
//                    "DEFAULT",
//                    EnumCompanyTypes.SUBAGENCY.GetDisplayNameOfEnum()),

//                new CompanyTypeModel().Save("1ccd2511-a262-4f94-89da-10a962493bbb",
//                    "DEFAULT",
//                    EnumCompanyTypes.SCHOOL.GetDisplayNameOfEnum()),

//            };
//            _dbContext.CompanyTypes.AddRange(comps);
//            _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

//            //create default master company account

//            compMaster.Save(
//            "299e1328-1185-4eed-9740-8fa1564efdfe",
//            "DEFAULT",
//            DateTime.Now,
//            DateTime.Now,
//            DateTime.Now,
//            "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0",
//            "@cc.in",
//            "Coding Company",
//            "info@cc.in",
//            DateTime.Now,
//            false,
//            "Default",
//            false,
//            false,
//            EnumRecordStatus.Active,
//            "https://www.codingcompany.in/");


//            _dbContext.CompanyMasters.AddAsync(compMaster).GetAwaiter().GetResult();
//            _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

//            //adding default company profile record.
//            var companyProfile = new CompanyMasterProfileModel();
//            companyProfile.Save(

//                compMaster.Id, 2, 0, 4, dbHostName, "Coding Company", "Default", DateTime.Now, false,
//                EnumRecordStatus.Active, "codingcompanydb");

//            _dbContext.CompanyMasterProfiles.AddAsync(companyProfile).GetAwaiter().GetResult(); ;
//            _dbContext.SaveChangesAsync().GetAwaiter().GetResult();
//        }



//        if (_roleManager.FindByNameAsync(roleName: RoleName.Administrator).Result == null)
//        {

//            UserRoles userRoles = new UserRoles();

//            var allRoles = ExtensionMethods.GetFieldValues(new RoleName());
//            foreach (var role in allRoles)
//            {
//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", role.Value);
//                _roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            }

//            //UserRoles userRoles = new UserRoles();
//            ////adding default roles for CC database
//            //userRoles.Save("DEFAULT", RoleName.Administrator);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.Master);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.Agency);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.Developer);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.Tester);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.School);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.SubAgency);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            //userRoles = new UserRoles();
//            //userRoles.Save("DEFAULT", RoleName.User);
//            //_roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//            // _dbContext.SaveChangesAsync().GetAwaiter().GetResult();
//        }
//        ApplicationUser usr = new ApplicationUser();

//        if (_userManager.FindByNameAsync("master@cc.in").Result == null)
//        {
//            //Create user and add theirs default roles
//            usr.Save("DEFAULT", "master@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//            usr.PasswordHash = ExtensionMethods.HashPassword("Kavya392010@");
//            usr.ConfirmEmail();
//            _dbContext.User.Add(usr);

//            _dbContext.SaveChanges();//Async().GetAwaiter().GetResult();)
//            //            _userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//            _userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Master }).GetAwaiter().GetResult();

//            //  _dbContext.SaveChangesAsync().GetAwaiter().GetResult();


//            usr = new ApplicationUser();
//            usr.Save("DEFAULT", "tester@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//            usr.PasswordHash = ExtensionMethods.HashPassword("Kavya392010@");
//            usr.ConfirmEmail();
//            _dbContext.User.Add(usr);
//            _dbContext.SaveChanges();
//            //          _userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//            _userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Tester }).GetAwaiter().GetResult();

//            // _dbContext.SaveChangesAsync();

//            usr = new ApplicationUser();
//            usr.Save("DEFAULT", "developer@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//            usr.PasswordHash = ExtensionMethods.HashPassword("Kavya392010@");
//            usr.ConfirmEmail();
//            _dbContext.User.Add(usr);
//            _dbContext.SaveChanges();
//            // _userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//            _userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Developer }).GetAwaiter().GetResult();

//            // _dbContext.SaveChangesAsync().GetAwaiter().GetResult();


//            usr = new ApplicationUser();
//            usr.Save("DEFAULT", "admin@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//            usr.PasswordHash = ExtensionMethods.HashPassword("Kavya392010@");
//            usr.ConfirmEmail();
//            _dbContext.User.Add(usr);
//            _dbContext.SaveChanges();//Async().GetAwaiter().GetResult();)
//            //            _userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//            _userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Administrator }).GetAwaiter().GetResult();

//            // _dbContext.SaveChangesAsync().GetAwaiter().GetResult();


//            //usr = new ApplicationUser();
//            //usr.Save("DEFAULT", "user@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//            //_userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//            //_userManager.AddToRolesAsync(usr, new List<string>() { RoleName.User }).GetAwaiter().GetResult();

//            //_dbContext.SaveChanges();//Async().GetAwaiter().GetResult();)
//        }


//        var dbHostRecords = _dbContext.ApplicationHostMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToList();

//        // when no records then add default host master model record for database deployment 
//        if (dbHostRecords?.Count == 0)
//        {

//            var hashString = ExtensionMethods.GeneratePassword();
//            var username = encryptionService.Encrypt("scopeerp", hashString);
//            var password = encryptionService.Encrypt("Kavya392010@", hashString);
//            ApplicationHostMasterModel dbHost = new();
//            dbHost.Password
//            dbHost.Save(dbHost, username, password, hashString, dbHostName, dbHostName);
//            dbHost.Port = 3306;
//            _dbContext.ApplicationHostMasters.Add(dbHost);
//            _dbContext.SaveChanges();

//            AppDBHostVsCompanyMaster dbVsComp = new();
//            dbVsComp.AppHostId = dbHost.Id;
//            dbVsComp.CompanyMasterId = compMaster.Id;
//            dbVsComp.Save("DEFAULT");
//            _dbContext.AppDbHostVsCompanyMasters.Add(dbVsComp);
//            _dbContext.SaveChanges();

//        }

//        //var dbHostingRecords = _dbContext.ApplicationHostMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToList();

//        var schoolProfile =
//            _dbContext.AppDbHostVsCompanyMasters.Include(m => m.CompanyMaster.CompanyMasterProfile).Include(a => a.ApplicationHostMaster).ToList();
//        // Dictionary<string, string> hostCom = new Dictionary<string, string>();
//        foreach (var hostRecord in schoolProfile)
//        {
//            var dbdeploymentUsername =
//                encryptionService.Decrypt(hostRecord.ApplicationHostMaster.UserName, hostRecord.ApplicationHostMaster.HashString);
//            var dbDeploymentPassword =
//                encryptionService.Decrypt(hostRecord.ApplicationHostMaster.Password, hostRecord.ApplicationHostMaster.HashString);

//            var newDBConectionString =
//                $"server={hostRecord.ApplicationHostMaster.Domain};user={dbdeploymentUsername};password={dbDeploymentPassword};" +
//                $"database={hostRecord.CompanyMaster.CompanyMasterProfile.DatabaseName};port={hostRecord.ApplicationHostMaster.Port};";


//            HostDBRecords.CompanyVsDBHosts.Add(hostRecord.CompanyMaster.SuffixDomain, newDBConectionString);
//        }



//    }

//    public void UpdateChangesToDb()
//    {
//        //Adding system preferences default value

//        var defaultPreferences = _dbContext.SystemPreferences.ToList();
//        //if (defaultPreferences.Count > 0)
//        //    return;


//        //Preference Name : ResultPerPage
//        var resultPerPage = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.ResultPerPage, StringComparison.CurrentCultureIgnoreCase));

//        var defaultSystemPreferences = new SystemPreferencesModel();

//        if (resultPerPage == null)
//        {
//            defaultSystemPreferences.PreferenceName = AppPreferences.ResultPerPage;
//            defaultSystemPreferences.DefaultValue = "100";
//            defaultSystemPreferences.CustomValue = "0";
//            defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Int;

//            defaultSystemPreferences.Description = "Display records per page.";
//            defaultSystemPreferences.ModuleName = "All";
//            defaultSystemPreferences.Save("Default");
//            _dbContext.SystemPreferences.Add(defaultSystemPreferences);
//        }


//        //Preference Name : ModificationAllowsInDays
//        var modificationAllowsInDays = defaultPreferences.FirstOrDefault(m => m.PreferenceName.Equals(AppPreferences.ModificationAllowsInDays, StringComparison.CurrentCultureIgnoreCase));// == "ModificationAllowsInDays");

//        defaultSystemPreferences = new SystemPreferencesModel();

//        if (modificationAllowsInDays == null)
//        {
//            defaultSystemPreferences.PreferenceName = AppPreferences.ModificationAllowsInDays;
//            defaultSystemPreferences.DefaultValue = "7";
//            defaultSystemPreferences.CustomValue = "0";
//            defaultSystemPreferences.ValueType = GenericFunction.Enums.ValueType.Int;

//            defaultSystemPreferences.Description = "Modification allows in days. Eg. Update/Delete record.";
//            defaultSystemPreferences.ModuleName = "All";
//            defaultSystemPreferences.Save("Default");
//            _dbContext.SystemPreferences.Add(defaultSystemPreferences);
//        }

//        _dbContext.SaveChangesAsync().GetAwaiter().GetResult();
//    }
//}