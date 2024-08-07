using AutoMapper;
using BSAuthentication.BsInterface;
using BSCodingCompany.BaseClass;
using BSCodingCompany.BSInterfaces.DemoRequest;
using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Core.Contracts.CommonServices;
using DataCacheLayer.CacheRepositories.Interfaces;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Constants.Authorization;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.Enums;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EncryptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.DtoModels.Company;
using ModelTemplates.EntityModels.AppConfig;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.EntityModels.Company;
using ModelTemplates.EntityModels.UserAccount;
using ModelTemplates.Master.Company;
using ModelTemplates.Persistence.Models.AppLevel;
using System.Data;
using System.Reflection;
using System.Transactions;
using static GenericFunction.CommonMessages;
using static ModelTemplates.RequestNResponse.Accounts.UserIdentity;

namespace BSCodingCompany.BSServices.DemoRequest;

public sealed class BsDemoRequestService : BaseBusinessLayer, IBsDemoRequestContract
{
    private readonly IBsDbInitializerContract _bsDbInitializer;
    private readonly IDataLayerDemoRequestContract _dlDemoRequestService;
    private readonly IDataLayerCompanyMaster _dlCompanyMasterService;
    private readonly IDataLayerCompanyMasterProfile _dlCompanyMasterProfileService;
    private readonly IDataLayerCompanyType _dlCompanyTypeService;
    private readonly IDataLayerCompanyMasterVsDbHost _dataLayerCompanyMasterVsDbHostService;
    private ApplicationDbContext _dbContext;
    private readonly IDataLayerNewDBService _dlNewDBService;
    private readonly IEmailService _emailService;
    //private readonly ICacheContract cache;
    private readonly IMapper iMapper;
    private string? _origin;
    IEnumerable<string> resourceNames;
    Assembly assembly;
    private ApplicationSettings applicationSettings;
    private EnumDBType dbType;
    string? dbTypeFromSession = string.Empty; //EnvironmentName,
    private readonly IDataLayerApplicationHostContract _dlAppHostContract;

    public BsDemoRequestService(IDataLayerDemoRequestContract dlDemoRequestContract,
        IDataLayerApplicationHostContract dlAppHostContract,
        IDataLayerCompanyType dlCompanyTypeService,
        IDataLayerCompanyMaster dlCompanyMasterService,
        IDataLayerCompanyMasterProfile dlCompanyMasterProfileService,
        IDataLayerCompanyMasterVsDbHost dataLayerCompanyMasterVsDbHostService,
        ITrace trace,
        IDataLayerNewDBService dlNewDBService,
        IEmailService emailService,
        ICacheContract cache, IHttpContextAccessor httpContextAccessor,
        IMapper iMapper,
        IBsDbInitializerContract bsDbInitializer
        )
    : base(iMapper, httpContextAccessor, trace, cache)
    {
        //first check from session then read from appsettings.json
        dbTypeFromSession = _httpContextAccessor?.HttpContext?.GetHeader("DatabaseType");
        applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();

        //read config json as per environment
        dbTypeFromSession = string.IsNullOrWhiteSpace(dbTypeFromSession) ? SettingsConfigHelper.AppSetting("Database", "Type") : dbTypeFromSession;
        _dlCompanyTypeService = dlCompanyTypeService;


        _origin = httpContextAccessor?.HttpContext?.GetHeader("origin");
        //_cacheData = cache; //new CacheRepositoryService(_ClientId, _UserId);
        _dlDemoRequestService = dlDemoRequestContract;
        _bsDbInitializer = bsDbInitializer;
        _dlAppHostContract = dlAppHostContract;
        //_dlCompanyTypesService = dlCompanyTypesService;
        _dlCompanyMasterService = dlCompanyMasterService;
        _dlCompanyMasterProfileService = dlCompanyMasterProfileService;
        _dataLayerCompanyMasterVsDbHostService = dataLayerCompanyMasterVsDbHostService;
        //_dlRoleContract = dlRoleContract;
        _dlNewDBService = dlNewDBService;
        _emailService = emailService;
        this.iMapper = iMapper;
        //_dbContext = context;
        dbType = (EnumDBType)System.Enum.Parse(typeof(EnumDBType), dbTypeFromSession, true);
        string DbStringName = System.Enum.GetName(typeof(EnumDBType), dbType) ?? "PGSQL";
        assembly = Assembly.GetExecutingAssembly();
        resourceNames = assembly.GetManifestResourceNames().
            Where(str => str.EndsWith(".sql") && (str.Contains(DbStringName) || str.Contains("PGSQL"))).OrderBy(str => str.ToString());

    }

    public async Task<ResponseDto<CompanyMasterDtoModel>> ActivateDemo(string demoId, string hostId, string? customSuffixDomain, string? refCode)
    {
        //var watch = System.Diagnostics.Stopwatch.StartNew();
        ResponseDto<CompanyMasterDtoModel> responseDto = new(_httpContextAccessor);
        ApplicationHostMasterModel? dbDeploymentRecord = new();
        CompanyMasterProfileModel? newSchoolProfile = new();
        DemoRequestModel? demoRecord = new DemoRequestModel();
        List<CompanyTypeModel> compTypesCC = new List<CompanyTypeModel>();
        IQueryable<CompanyMasterModel> companyMasterReferences;
        List<string> referenceCodes = new List<string>();
        UserRoles userRoles = new UserRoles();
        CompanyMasterModel? demoRequestSchool = new CompanyMasterModel();
        CompanyMasterModel? singleSchoolRecord = new CompanyMasterModel();
        CompanyMasterModel? newSchoolRegistration = new CompanyMasterModel();
        List<CompanyMasterModel>? schoolRecords = new List<CompanyMasterModel>();
        CompanyMasterModel? newSchoolRecord = new CompanyMasterModel();
        List<AppDBHostVsCompanyMaster> appDBHostVsCompanyMasterRecords = new List<AppDBHostVsCompanyMaster>();
        AppDBHostVsCompanyMaster DbHostSchool = new AppDBHostVsCompanyMaster();
        List<SystemPreferencesModel> DefaultSystemPreferences = new List<SystemPreferencesModel>();
        ApplicationDbContext _newDbContext;
        ApplicationUser schoolAdminUser = new();
        Dictionary<string, string>? allRoles = new Dictionary<string, string>();
        AppDBHostVsCompanyMaster? dbHostVsCompMasterRecord = new AppDBHostVsCompanyMaster();
        DbConnectionStringRecord connectedDomainHost = new DbConnectionStringRecord();
        ResponseDto<ApplicationUser> newUseResponseDto = new(_httpContextAccessor);
        IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
        PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
        ResponseReturn response = new ResponseReturn();
        AccountConfirmationModel accountConfirm = new();

        int iCount = 0;
        string newReferenceCodeForSchool = string.Empty;
        string dbdeploymentUsername = string.Empty;
        string dbDeploymentPassword = string.Empty;
        string randomPasswordForSchoolAdminUser = string.Empty;
        string dbHostName = string.Empty;
        //need to work on username default record.
        string dbUsername = string.Empty;
        string dbConnectionString = string.Empty;




        CompanyTypeModel? companyMasterType = new CompanyTypeModel();
        CompanyTypeModel? companySchoolType = new CompanyTypeModel();

        //process starts

        //for testig perpose : using post man - origin comes with null, so set localhost manually
        if (string.IsNullOrWhiteSpace(_origin))
        {
            _origin = "https://localhost";
        }

        //BR - if user demoId is not provided, return Unauthorized
        if (string.IsNullOrWhiteSpace(_userId))
        {
            responseDto = new(_httpContextAccessor)
            {
                Message = "Please provide user information!",
                StatusCode = StatusCodes.Status401Unauthorized,
            };
            return await Task.Run(() => responseDto);
        }

        //get where this school database is hosted.
        dbDeploymentRecord = await _dlAppHostContract.GetAsync(hostId);
        if (dbDeploymentRecord == null)
        {
            _httpContextAccessor?.HttpContext.Items.Remove(ContextKeys.dbConnectionString);
            var ex = new Exception("Hosting domain not found!");
            ex.SendExceptionMailAsync();
            // Log error
            responseDto.Message = "Hosting domain not found!";
            return await Task.Run(() => responseDto);
        }

        //#region Checking Demo request from table as per id and status

        //finding demo record which is requested by school or agency.
        demoRecord = await _dlDemoRequestService.GetByAny(demoId);
        //demoRecord = await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.Id == demoId);

        //start the activation process for requested demo id

        //get all types of company from default database
        compTypesCC = await _dlCompanyTypeService.GetAll();

        companyMasterType = compTypesCC.FirstOrDefault(m => m.TypeName == EnumCompanyTypes.MASTER.GetDisplayNameOfEnum());
        companySchoolType = compTypesCC.FirstOrDefault(m => m.TypeName == EnumCompanyTypes.SCHOOL.GetDisplayNameOfEnum());

        if (demoRecord == null)
        {
            //when no demo is requested for the given demoId
            _httpContextAccessor?.HttpContext.Items.Remove(ContextKeys.dbConnectionString);
            var ex = new Exception("No Demo record available!");

            ex.SendExceptionMailAsync();
            // Log error
            responseDto = new(_httpContextAccessor)
            {
                Message = "No Demo record available!"
            };
            return await Task.Run(() => responseDto);

        }
        //when demo is not activated
        else if (demoRecord.IsDemoActivated == false)
        {
            //when demo is not activated then proceed with activation.
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    //check school record for given demoId
                    //we are checking from master database.
                    //var masterCompanyCC = _dbContext.CompanyMasters.Include(m => m.CompanyMasterProfile).FirstOrDefault(m => m.ReferenceCode == "Default");

                    //this is the default first record.
                    //_dlCompanyMasterService.SetAppliationDbContextRef(_dbcontext)
                    var masterCompanyCC = await _dlCompanyMasterService.GetCompanyByReferenceCodeAsync("Default");

                    newSchoolRecord = new CompanyMasterModel();
                    //create a new school record inside master database.
                    //yet connection is going on as default connection.
                    newSchoolRecord.Save(id: string.Empty, userid: _userId, DateTime.Now,
                        accountExpireDateTime: DateTime.Now.AddMonths(1),
                        demoExpireDateTime: DateTime.Now.AddMonths(1), companyTypeId:
                        companySchoolType.Id, suffixDomain: (string.IsNullOrWhiteSpace(customSuffixDomain)
                            ? demoRecord.Email.Split('@')[1]
                            : customSuffixDomain), name: demoRecord.Name,
                        email: demoRecord.Email, enrollmentDate: DateTime.Now, isDemoMode: true,
                        referenceCode: masterCompanyCC.Id, //need to work
                        isDemoExpired: false, isEditable: false, recordStatus: EnumRecordStatus.Active, website: demoRecord.Website, demoRecord.Id);



                    newSchoolRecord.CompanyMasterProfile = null;
                    newSchoolRecord.CompanyTypeEntityModel = null;

                    demoRecord.IsDemoActivated = true;
                    demoRecord.UserId = _userId;
                    demoRecord.DemoActivatedOn = DateTime.Now;

                    await _dlDemoRequestService.Update(demoRecord);
                    //Converting demo request to live  and saving to master database
                    //still db connection is/should master.
                    newSchoolRecord = await _dlCompanyMasterService.AddAsync(newSchoolRecord);


                    //now time to make profile for the school for both side
                    //dbUsername need to work here.
                    var databaseName = IEncryptionService.Encrypt(newSchoolRecord.SuffixDomain, applicationSettings.CacheServer.HashKey);
                    newSchoolProfile.Save(newSchoolRecord.Id, 0, 0, 1, dbDeploymentRecord.Domain, newSchoolRecord.Name, _userId, dbDeploymentRecord.UserName, DateTime.Now,
                        false, EnumRecordStatus.Active, databaseName);

                    await _dlCompanyMasterProfileService.Add(newSchoolProfile);



                    //adding db host vs company master record for future db connection reference.
                    DbHostSchool = new AppDBHostVsCompanyMaster();

                    DbHostSchool.CompanyMasterId = newSchoolProfile.Id;
                    DbHostSchool.AppHostId = dbDeploymentRecord.Id;

                    dbHostVsCompMasterRecord = await _dataLayerCompanyMasterVsDbHostService.Add(DbHostSchool);


                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    //when no demo is requested for the given demoId
                    _httpContextAccessor?.HttpContext.Items.Remove(ContextKeys.dbConnectionString);

                    ex.SendExceptionMailAsync();
                    // Log error
                    responseDto = new(_httpContextAccessor)
                    {
                        Message = ex.Message
                    };
                    return await Task.Run(() => responseDto);
                }
            }

        }

        ////checking that demo is activated but no database is created for the demo 

        ////now getting newly updated record for school and its associated hosting details, from master db.


        dbHostVsCompMasterRecord = await _dataLayerCompanyMasterVsDbHostService.GetAllWithProfileAndHost(newSchoolRecord.SuffixDomain);
        //dbHostVsCompMasterRecord = await _dbContext.AppDbHostVsCompanyMasters
        //                                .Include(m => m.CompanyMaster)
        //                                    .Include(m => m.CompanyMaster.CompanyMasterProfile)
        //                                    .Include(a => a.ApplicationHostMaster)
        //                                    .FirstOrDefaultAsync(m => m.AppHostId == dbDeploymentRecord.Id);

        //Get all the preference value from master database
        DefaultSystemPreferences = _bsDbInitializer.GetDefaultPreferences(_dbContext);

        //set connection string object for the given demo school and its associated hosting details.
        connectedDomainHost = new DbConnectionStringRecord
        {
            DbConnectionString = dbHostVsCompMasterRecord.ApplicationHostMaster?.ConnectionString,
            SuffixName = dbHostVsCompMasterRecord.CompanyMaster?.SuffixDomain,
            DbType = dbHostVsCompMasterRecord.ApplicationHostMaster.DatabaseType,
            ClientId = dbHostVsCompMasterRecord.CompanyMasterId
        };

        //setting connection string in session
        if (_httpContextAccessor?.HttpContext != null)
        {

            _httpContextAccessor?.HttpContext?.SetContextItemAsJson(ContextKeys.dbConnectionString, connectedDomainHost);
            _httpContextAccessor?.HttpContext?.SetContextItemAsJson(ContextKeys.DatabaseType, dbHostVsCompMasterRecord.ApplicationHostMaster.DatabaseType);
            _httpContextAccessor?.HttpContext?.SetHeader(ContextKeys.Suffix, dbHostVsCompMasterRecord.CompanyMaster.SuffixDomain);
            _httpContextAccessor?.HttpContext?.SetHeader(ContextKeys.ClientId, dbHostVsCompMasterRecord.CompanyMasterId);
        }


        //======= start migration
        //First get current context
        _newDbContext = await _dlCompanyTypeService.GetDbContextAsyncAsync();

        //now re-initialize the dbContext.
        _newDbContext = new ApplicationDbContext(_newDbContext._Options, _httpContextAccessor, _trace);

        try
        {


            //migration db
            _newDbContext.Database.Migrate();

            //_bsDbInitializer.InitializeOnNewDb();


            if (_newDbContext.Database.GetPendingMigrations().Any())
            {
                _newDbContext.Database.Migrate();
            }

            foreach (string resourceName in resourceNames)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    _newDbContext.Database.ExecuteSqlRaw(sql);
                }
            }


            //setup new dbContext for data layer
            await _dlCompanyTypeService.SetDbContextAsync(_newDbContext);

            //add company types to new db
            await _dlCompanyTypeService.AddRange(compTypesCC);


            //set current user id for master company and its profile
            newSchoolRegistration = dbHostVsCompMasterRecord.CompanyMaster;
            var newCompanyProfile = newSchoolRegistration.CompanyMasterProfile;
            newSchoolRegistration.CompanyMasterProfile = null;
            newSchoolRegistration.CompanyTypeEntityModel = null;
            newSchoolRegistration.SetUser(_userId, companySchoolType.Id);
            newSchoolRegistration.DemoRequestId = demoId;

            //setup new dbContext for data layer
            await _dlCompanyMasterService.SetDbContextAsync(_newDbContext);

            newSchoolRegistration = await _dlCompanyMasterService.AddAsync(newSchoolRegistration);

            //get the newly generated record.
            newSchoolRegistration = _newDbContext.CompanyMasters.FirstOrDefault(m => m.Name == newSchoolRegistration.Name);

            //update record for schooltype and its profile
            newSchoolProfile = newCompanyProfile;
            newSchoolProfile.SetUser(_userId);

            //setup new dbContext for data layer
            await _dlCompanyMasterProfileService.SetDbContextAsync(_newDbContext);
            newSchoolProfile = await _dlCompanyMasterProfileService.Add(newSchoolProfile);


            allRoles = GenericFunction.ExtensionMethods.GetFieldValues(new RoleName());
            iCount = 1;
            foreach (var role in allRoles)
            {
                userRoles = new UserRoles();
                userRoles.CompanyId = newSchoolRegistration.Id;
                userRoles.Save(iCount.ToString(), role.Value);
                //_roleManager.Create(userRoles);
                _newDbContext.Roles.Add(userRoles);
                iCount++;
            }
            _newDbContext.SaveChanges();


            UserStore<ApplicationUser> _userManagerForNewDb
                = new UserStore<ApplicationUser>(_newDbContext);


            Guid guid = Guid.NewGuid();

            //Create user and add theirs default roles
            schoolAdminUser.NormalizedUserName = $"{demoRecord.FirstName} {demoRecord.LastName}";
            schoolAdminUser.PhoneNumber = demoRecord.ContactNo;

            schoolAdminUser.CompanyId = newSchoolRegistration.Id;
            schoolAdminUser.CompanyTypeId = companySchoolType.Id;
            schoolAdminUser.Save(_userId, $"schooladmin@{newSchoolRegistration.SuffixDomain}",
                newSchoolRegistration.Id, companySchoolType.Id, newSchoolRegistration.Email, demoRecord.FirstName, demoRecord.LastName, demoRecord.Email);

            //schoolAdminUser.PasswordHash = hasher.HashPassword(schoolAdminUser, "Noting_00");
            schoolAdminUser.SecurityStamp = guid.ToString();


            schoolAdminUser.PasswordHash = GenericFunction.ExtensionMethods.HashPassword("Kavya392010@");

            _newDbContext.User.Add(schoolAdminUser);
            _newDbContext.SaveChanges();

            schoolAdminUser = _newDbContext?.User?.FirstOrDefault(m => m.UserName == schoolAdminUser.UserName && m.PasswordHash == schoolAdminUser.PasswordHash);
            //_userManagerForNewDb.AddToRole(schoolAdminUser, RoleName.ErpAdmin);

            string? roleId = _newDbContext?.Roles?.FirstOrDefault(m => m.Name == RoleName.ErpAdmin)?.Id;
            IdentityUserRole<string> userRoleObject = new();
            userRoleObject.UserId = schoolAdminUser.Id;
            userRoleObject.RoleId = roleId;
            string masterUserId = schoolAdminUser.Id;//  _dbContext.SaveChanges();

            _newDbContext.UserRoles.Add(userRoleObject);
            _newDbContext.SaveChanges();

            //adding system preferences default values.

            _bsDbInitializer.AddDefaultSystemPreferences(DefaultSystemPreferences, _newDbContext);



            #region Shoot Email for SchoolAdmin that, your account is created




            //save in cc db

            accountConfirm.Save(_userId, schoolAdminUser.UserName, dbDeploymentRecord.Id);
            _newDbContext.AccountConfirmations.Add(accountConfirm);
            _newDbContext.SaveChanges();


            //pending DatabaseName is pending

            response = accountConfirm.GenerateConfirmJsonObject(newSchoolRegistration.Id, newSchoolProfile.DatabaseName, dbDeploymentRecord.Id);
            response.UserId = schoolAdminUser.Id; //set userid for schooladmin
            newUseResponseDto.Result = schoolAdminUser;
            var activationResponseJson = response.ToJson();
            var encryptedLink =
                IEncryptionService.Encrypt(activationResponseJson, "CODINGCOMPANY.IN");
            var encurl = System.Web.HttpUtility.UrlEncode(encryptedLink);
            _origin = _origin + $"/apigateway/account/confirm-me?confirmId={encurl}";
            //generate body and subject for email
            ResponseDto<dynamic> dyncNewUser = new ResponseDto<dynamic>(_httpContextAccessor);
            dyncNewUser.Result = newUseResponseDto.Result;
            (string subject, string bodyMessage) =
                _emailService.CreateNewUserEmailBody(dyncNewUser, newSchoolRegistration, _origin);

            //send email
            //it is independent process, do not wait for smtp response.
            _ = Task.Run(() => _emailService.Send(newSchoolRegistration.Email, subject, bodyMessage));


            #endregion Shoot Email for SchoolAdmin that, your account is created





            _newDbContext.SaveChanges();
            _httpContextAccessor?.HttpContext.Items.Remove(ContextKeys.dbConnectionString);
            _httpContextAccessor.HttpContext.SetContextItemAsJson("Default", string.Empty);


            var companyDto = _mapper.Map<CompanyMasterDtoModel>(newSchoolRegistration);
            responseDto.Result = companyDto;
            responseDto.Status = Status.Success;
            responseDto.EmailStatus = true;
            responseDto.Id = companyDto.Id;
            responseDto.StatusCode = StatusCodes.Status201Created;
            responseDto.Message = OperationSuccessful;
            responseDto.RecordCount = 1;
            //    transactionScope.Complete();
        }
        catch (Exception ex)
        {
            //transactionScope.Dispose();
            _httpContextAccessor?.HttpContext.Items.Remove(ContextKeys.dbConnectionString);
            ex.SendExceptionMailAsync();
            // Log error
            //transactionScope.Dispose();
            return default;
        }



        return responseDto;
    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GenerateDemoRequest(DemoRequestDtoModel dto)
    {
        ResponseDto<DemoRequestDtoModel> responseDto;

        #region Check For Reference Code validity
        //check for reference code, if it is not null then it belongs to any agency or sub agency
        //
        if (!string.IsNullOrWhiteSpace(dto.ReferenceCode))
        {
            var isRefCodeAvailable =
                await _dlDemoRequestService.CheckByReferenceCode(dto.ReferenceCode);
            if (!isRefCodeAvailable)
            {
                responseDto = new(_httpContextAccessor)
                {
                    Id = dto.Id,
                    Result = dto,
                    RecordCount = 1,
                    Message = $"Invalid reference code {dto.ReferenceCode} provided or Database not initiated!",
                    MessageType = MessageType.Warning,
                    StatusCode = StatusCodes.Status302Found,
                    Status = Status.Failed,
                };
                return await Task.Run(() => responseDto);
            }
        }

        #endregion

        #region Check for already demo requested

        //get the record for demo request table for the given demo request dtomodel
        //var demoQuery = DbSet.Where(model => model.Email == dto.Email || model.ContactNo == dto.ContactNo ||
        //                                                                  model.Website == dto.Website).AsQueryable();

        //var demoRequestRecord = await demoQuery.FirstOrDefault(model => model.RecordStatus == EnumRecordStatus.Active);

        var demoRequestRecord = await _dlDemoRequestService.IsDemoAlreadyRequested(dto.Website, dto.ContactNo, dto.Email);
        string returnMessage = string.Empty;
        if (demoRequestRecord != null)
        {
            //BR - if Client is restricted
            if (demoRequestRecord.IsRestrictedForDemo)
            {
                var getExpiredDate = demoRequestRecord.DemoActivatedOn;
                demoRequestRecord.RecordStatus = EnumRecordStatus.Expired;
                if (getExpiredDate < DateTime.Now)
                {
                    //                    var demoSchool = await context.CompanyMasters.FirstOrDefault(m => m.DemoRequestId == demoRequestRecord.Id && m.RecordStatus == EnumRecordStatus.Active);
                    var demoSchool = await _dlCompanyMasterService.GetSchoolByDemoRequestIdAsync(demoRequestRecord.Id);
                    if (demoSchool != null)
                    {
                        demoSchool.IsDemoExpired = true;
                        demoSchool.DemoExpireDate = DateTime.Now;
                        demoSchool.UserId = "Default";
                        demoSchool.RecordStatus = EnumRecordStatus.Expired;
                        await _dlCompanyMasterService.AddAsync(demoSchool);
                        //await context.SaveChanges();
                    }
                }
                returnMessage = $"You are not eligible for Scope ERP Demo!";
                responseDto = new(_httpContextAccessor)
                {
                    Id = dto.Id,
                    Result = dto,
                    RecordCount = 1,
                    Message = returnMessage,
                    MessageType = MessageType.Warning,
                    StatusCode = StatusCodes.Status302Found, //record found in db
                    Status = Status.Failed,
                };
                return responseDto;
            }
            //BR - if client is not restricted and demo is activated from our side.
            else if (!demoRequestRecord.IsRestrictedForDemo && demoRequestRecord.IsDemoActivated)
            {

                // var schoolDemoRecord = await context.CompanyMasters.FirstOrDefault(model => !model.IsDemoExpired && model.IsDemoMode && model.RecordStatus == EnumRecordStatus.Active);

                //BR- if demo is not expired and school is running in demo mode
                if (await _dlCompanyMasterService.GetSchoolInDemoModeAsync(dto.Id))
                {
                    returnMessage = $"Demo request for {dto.Name} is already activated, kindly check your registered email for further process!";
                }
                else
                {
                    //BR - if demo is expired and school is running in demo mode
                    //then we have to block next demo request, and save the record in request demo table.
                    demoRequestRecord.IsRestrictedForDemo = true;
                    _dbContext.Update(demoRequestRecord);
                    //DbSet.Update(demoRequestRecord);
                    //await context.SaveChanges();
                    returnMessage = $"Demo for Scope ERP Software is expired, we can not provide another demo for our product!";
                }

            }
            // BR - if client is not restricted for demo and demo is not activated
            else if (!demoRequestRecord.IsRestrictedForDemo && !demoRequestRecord.IsDemoActivated)
            {//record is not null here
                returnMessage = $"Demo request for {dto.Name} is already registered,our Team is processing your request!";
            }

            dto = _mapper.Map<DemoRequestDtoModel>(demoRequestRecord);
            dto.Id = demoRequestRecord.Id;
            responseDto = new(_httpContextAccessor)
            {
                Id = dto.Id,
                Result = dto,
                RecordCount = 1,
                Message = returnMessage,
                MessageType = MessageType.Warning,
                StatusCode = StatusCodes.Status302Found, //record found in db
                Status = Status.Failed,
            };

            //is client is restricted then no need to process further
            if (!demoRequestRecord.IsRestrictedForDemo &&
                demoRequestRecord.IsDemoActivated)
            {
                return await Task.Run(() => responseDto);
            }

        }
        else
        {
            //Note: On first demo request from school 
            //DemoRequestModel record = MapDtoToModel(dto);
            DemoRequestModel record = _mapper.Map<DemoRequestModel>(dto);
            record.Save();
            record = await _dlDemoRequestService.Add(record);


            //await context.Add(record);
            //await context.SaveChanges();
            dto = _mapper.Map<DemoRequestDtoModel>(record);
            responseDto = new(_httpContextAccessor)
            {
                Id = record.Id,
                DateTime = DateTime.Now,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                Result = dto
            };
        }
        #endregion

        #region Generate Message for Email
        ResponseDto<dynamic> dynresponseDto = new ResponseDto<dynamic>(_httpContextAccessor);
        dynresponseDto.Result = responseDto.Result;
        //generate body and subject for email
        (string subject, string bodyMessage) =
            _emailService.GenerateDemoRequestBody(dynresponseDto, responseDto.Result.Id, responseDto.RecordCount > 0 ? true : false);

        //send email
        //it is independent process, do not wait for smtp response.
        _ = Task.Run(() => _emailService.Send(dto.Email, subject, bodyMessage));
        #endregion

        #region Saving email to database
        //EmailMaster model = new()
        //{
        //    ToEmail = string.Join(",", dto.Email),
        //    CCEmail = "",
        //    Body = bodyMessage,
        //    ClientId = "Default",
        //    Subject = subject,
        //    FromEmail = MailConfiguration.EmailFrom,
        //    EmailNotificationType = EmailNotificationType.MESSAGE,
        //};

        //model.Save("Default");
        //await EmailService.Add(model);
        //await context.SaveChanges();

        #endregion


        #region Finally send response to controller

        responseDto.EmailStatus = true;
        responseDto.StatusCode = StatusCodes.Status201Created;
        responseDto.RecordCount = 1; // in both condition count will be 1 but set manually here.

        return await Task.Run(() => responseDto);
        #endregion

    }

    public async Task<ResponseDto<List<DemoRequestDtoModel>>> GetAll(bool isActive)
    {
        // var watch = System.Diagnostics.Stopwatch.StartNew();


        ResponseDto<List<DemoRequestDtoModel>> responseDto = new(_httpContextAccessor);
        var records = await _dlDemoRequestService.GetAll(isActive);
        if (records.Count() == 0)
        {
            return responseDto;
        }

        //watch.Stop();
        var elapsedMs = 0;
        responseDto = new(_httpContextAccessor)
        {
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = records.Count(),
            StatusCode = StatusCodes.Status200OK,
            TimeConsumption = elapsedMs,
            Result = _mapper.Map<List<DemoRequestDtoModel>>(records)
        };
        return await Task.Run(() => responseDto);
    }

    public async Task<ResponseDto<List<DemoRequestDtoModel>>> GetAllByReferenceCode(string referenceCode)
    {
        ResponseDto<List<DemoRequestDtoModel>> responseDto = new(_httpContextAccessor);
        var records = await _dlDemoRequestService.GetAllByReferenceCode(referenceCode);
        if (records.Count() == 0)
        {
            return responseDto;
        }
        responseDto.Result = _mapper.Map<List<DemoRequestDtoModel>>(records);
        return await Task.Run(() => responseDto);
    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GetByAny(string? id = "", string? refCode = "", string? website = "", string? contactNo = "", string? email = "")
    {
        ResponseDto<DemoRequestDtoModel> responseDto = new ResponseDto<DemoRequestDtoModel>(_httpContextAccessor);
        var record = await _dlDemoRequestService.GetByAny(id, refCode, website, contactNo, email);
        if (record == null)
        {
            return responseDto;
        }

        responseDto = new(_httpContextAccessor)
        {
            Id = record.Id,
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = 1,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<DemoRequestDtoModel>(record)
        };
        return responseDto;

    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GetByContactNo(string contactNo)
    {
        ResponseDto<DemoRequestDtoModel> responseDto = new ResponseDto<DemoRequestDtoModel>(_httpContextAccessor);
        var record = await _dlDemoRequestService.GetByContactNo(contactNo);
        if (record == null)
        {
            return responseDto;
        }

        responseDto = new(_httpContextAccessor)
        {
            Id = record.Id,
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = 1,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<DemoRequestDtoModel>(record)
        };
        return responseDto;

    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GetByEmailId(string emailId)
    {
        ResponseDto<DemoRequestDtoModel> responseDto = new ResponseDto<DemoRequestDtoModel>(_httpContextAccessor);
        var record = await _dlDemoRequestService.GetByEmailId(emailId);
        if (record == null)
        {
            return responseDto;
        }

        responseDto = new(_httpContextAccessor)
        {
            Id = record.Id,
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = 1,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<DemoRequestDtoModel>(record)
        };
        return responseDto;

    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GetByReferenceCode(string referenceCode)
    {

        ResponseDto<DemoRequestDtoModel> responseDto = new ResponseDto<DemoRequestDtoModel>(_httpContextAccessor);
        var record = await _dlDemoRequestService.GetByReferenceCode(referenceCode);
        if (record == null)
        {
            return responseDto;
        }

        responseDto = new(_httpContextAccessor)
        {
            Id = record.Id,
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = 1,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<DemoRequestDtoModel>(record)
        };

        return responseDto;
    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GetByWebsite(string website)
    {
        ResponseDto<DemoRequestDtoModel> responseDto = new ResponseDto<DemoRequestDtoModel>(_httpContextAccessor);
        var record = await _dlDemoRequestService.GetByWebsite(website);
        if (record == null)
        {
            return responseDto;
        }

        responseDto = new(_httpContextAccessor)
        {
            Id = record.Id,
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = 1,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<DemoRequestDtoModel>(record)
        };
        return responseDto;
    }

    public async Task<ResponseDto<DemoRequestDtoModel>> GetDemoRequestById(string id)
    {
        ResponseDto<DemoRequestDtoModel> responseDto = new ResponseDto<DemoRequestDtoModel>(_httpContextAccessor);
        var record = await _dlDemoRequestService.GetDemoRequestById(id);
        if (record == null)
        {
            return responseDto;
        }

        responseDto = new(_httpContextAccessor)
        {
            Id = record.Id,
            DateTime = DateTime.Now,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            RecordCount = 1,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<DemoRequestDtoModel>(record)
        };
        return responseDto;

    }

}

