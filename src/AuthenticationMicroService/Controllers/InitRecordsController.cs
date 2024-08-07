
//using DBOperationsLayer.Data.Context;
//using GenericFunction;
//using GenericFunction.Constants.Authorization;
//using GenericFunction.Enums;
////using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ModelTemplates.EntityModels.Application;
//using ModelTemplates.EntityModels.Company;
//using ModelTemplates.Master.Company;

//namespace AuthenticateService.Controllers
//{
//    [EnableCors("MyAllowSpecificOrigins")]

//    [ApiVersion("1.0")]


//    [Route("api/v{version:apiVersion}/[controller]")]

//    [ApiController]

//    public sealed class InitRecordsController : ControllerBase
//    {
//        //private readonly IUnitOfWorkService _unitOfWork;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<UserRoles> _roleManager;
//        private readonly ApplicationDbContext _dbContext;

//        public InitRecordsController(UserManager<ApplicationUser> userManager, RoleManager<UserRoles> roleManager, ApplicationDbContext dbContext)
//        {
//            //_unitOfWork = unitOfWork;
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _dbContext = dbContext;
//        }
//        [AllowAnonymous]
//        [HttpPost]
//        [Route("Init")]

//        public string InitDefaults()
//        {
//            ModelBuilder mb = new ModelBuilder();
//            //ModelBinderExtension.Seed(_dbContext, _userManager, _roleManager);

//            return "success!";
//        }

//    }
//    public static class ModelBinderExtension
//    {

//        public static void Seed(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<UserRoles> roleManager)
//        {


//            if (!dbContext.CompanyTypes.AnyAsync(m => m.TypeName == EnumCompanyTypes.MASTER.GetDisplayNameOfEnum()).Result)
//            {
//                var comps = new[]
//                {
//                    new CompanyTypeEntityModel("DEFAULT"){Id = "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0",
//                        UserId = "DEFAULT",
//                        TypeName = EnumCompanyTypes.MASTER.GetDisplayNameOfEnum()},

//                    new CompanyTypeEntityModel("DEFAULT"){Id = "1b23cc53-d9c2-415f-9663-e74a370a2e67",
//                        UserId = "DEFAULT",
//                        TypeName = EnumCompanyTypes.AGENCY.GetDisplayNameOfEnum()},

//                    new CompanyTypeEntityModel("DEFAULT"){Id = "1b5c5535-f91c-45f7-8fd5-8fecddc262e0",
//                        UserId = "DEFAULT",
//                        TypeName = EnumCompanyTypes.SUBAGENCY.GetDisplayNameOfEnum()},

//                    new CompanyTypeEntityModel("DEFAULT"){Id = "1ccd2511-a262-4f94-89da-10a962493bbb",
//                        UserId = "DEFAULT",
//                        TypeName = EnumCompanyTypes.SCHOOL.GetDisplayNameOfEnum()},

//                };
//                dbContext.CompanyTypes.AddRange(comps);
//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();

//                //create default master company account
//                var compMaster = new CompanyMasterEntityModel("Default")
//                {
//                    Id = "299e1328-1185-4eed-9740-8fa1564efdfe",
//                    UserId = "DEFAULT",
//                    CreatedOn = DateTime.Now,
//                    AccountExpire = DateTime.Now,
//                    DemoExpireDate = DateTime.Now,
//                    CompanyTypeId = "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0",
//                    SuffixDomain = "@codingcompany.in",
//                    Name = "Coding Company",
//                    Email = "info@codingcompany.in",
//                    EnrollmentDate = DateTime.Now,
//                    IsDemo = false,
//                };

//                dbContext.CompanyMasters.AddAsync(compMaster).GetAwaiter().GetResult();
//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();
//            }




//            if (roleManager.FindByNameAsync(roleName: RoleName.Administrator).Result == null)
//            {
//                UserRoles userRoles = new UserRoles();
//                //adding default roles for CC database
//                userRoles.Save("DEFAULT", RoleName.Administrator);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.Master);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.Agency);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.Developer);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.Tester);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.School);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.SubAgency);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                userRoles = new UserRoles();
//                userRoles.Save("DEFAULT", RoleName.User);
//                roleManager.CreateAsync(userRoles).GetAwaiter().GetResult();

//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();
//            }
//            ApplicationUser usr = new ApplicationUser("DEFAULT");

//            if (userManager.FindByNameAsync("master@cc.in").Result == null)
//            {
//                //Create user and add theirs default roles
//                usr.Save("DEFAULT", "master@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//                //userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//                usr.Save("DEFAULT", "developer@cc.in", pwd, "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//                _dbContext.User.Add(usr);
//                _dbContext.SaveChanges();

//                userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Master }).GetAwaiter().GetResult();

//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();


//                usr = new ApplicationUser("DEFAULT");
//                usr.Save("DEFAULT", "tester@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//                userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//                userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Tester }).GetAwaiter().GetResult();

//                dbContext.SaveChangesAsync();

//                usr = new ApplicationUser("DEFAULT");
//                usr.Save("DEFAULT", "developer@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//                userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//                userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Developer }).GetAwaiter().GetResult();

//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();


//                usr = new ApplicationUser("DEFAULT");
//                usr.Save("DEFAULT", "admin@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//                userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//                userManager.AddToRolesAsync(usr, new List<string>() { RoleName.Administrator }).GetAwaiter().GetResult();

//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();


//                usr = new ApplicationUser("DEFAULT");
//                usr.Save("DEFAULT", "user@cc.in", "299e1328-1185-4eed-9740-8fa1564efdfe", "03fc3cb2-daf7-4d1e-918d-cb0cd2032da0", "vishalkaushik@hotmail.com");
//                userManager.CreateAsync(usr, "Kavya392010@").GetAwaiter().GetResult();
//                userManager.AddToRolesAsync(usr, new List<string>() { RoleName.User }).GetAwaiter().GetResult();

//                dbContext.SaveChangesAsync().GetAwaiter().GetResult();
//            }
//        }
//    }
//}
