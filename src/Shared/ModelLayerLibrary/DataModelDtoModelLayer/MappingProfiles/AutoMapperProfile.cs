using ModelTemplates.DtoModels.AppConfig;
using ModelTemplates.DtoModels.CommonDtoModels;
using ModelTemplates.DtoModels.Company;
using ModelTemplates.DtoModels.Employee;
using ModelTemplates.DtoModels.Inventory;
using ModelTemplates.DtoModels.SchoolLibrary;
using ModelTemplates.EntityModels.AppConfig;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.EntityModels.Company;
using ModelTemplates.Master.Company;
using ModelTemplates.Persistence.Component.School.Library;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.Persistence.Models.School.CommonModels;
using ModelTemplates.Persistence.Models.School.Employee;
using ModelTemplates.Persistence.Models.School.Inventory;
using ModelTemplates.Persistence.Models.School.Library;
using ModelTemplates.RequestNResponse.Accounts;

namespace ModelTemplates.MappingProfiles
{
    /// <summary>
    /// this class responsible for mapping model class to dto model class visa-versa 
    /// </summary>
    public sealed class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            //app level mapping
            CreateMap<SystemPreferencesModel, SystemPreferencesDtoModel>().ReverseMap(); //reverse so the both direction
            //for coding company related mapping
            CreateMap<DemoRequestModel, DemoRequestDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<CompanyMasterModel, CompanyMasterDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<CompanyTypeModel, CompanyTypeDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<CompanyMasterProfileModel, CompanyMasterProfileDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<ApplicationHostMasterModel, ApplicationHostMasterDtoModel>().ReverseMap(); //reverse so the both direction


            //CreateMap<EmployeeDtoAbstractModel, EmployeeModel>().ReverseMap(); //reverse so the both direction
            //CreateMap<SchoolLibraryDtoModel, SchoolLibrary>().ReverseMap(); //reverse so the both direction
            //need to work here
            CreateMap<ApplicationUserDtoModel, ApplicationUser>().ReverseMap(); //reverse so the both direction
            CreateMap<RegisterRequest, ApplicationUser>().ReverseMap(); //reverse so the both direction

            //School Library Mapping

            CreateMap<SchoolLibraryHallModel, SchoolLibraryHallDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryAuthorModel, LibraryAuthorDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryMediaTypeModel, LibraryMediaTypeDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryRoomModel, LibraryRoomDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibrarySectionModel, LibrarySectionDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryRackModel, LibraryRackDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryBookshelfModel, LibraryBookshelfDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryBookCollectionModel, LibraryBookCollectionDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LibraryBookMasterModel, LibraryBookMasterDtoModel>().ReverseMap(); //reverse so the both direction

            //common model mapping
            CreateMap<VendorModel, VendorDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<CurrencyModel, CurrencyDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<LanguageModel, LanguageDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<AddressModel, AddressDtoModel>().ReverseMap(); //reverse so the both direction

            //inventory model mapping
            CreateMap<ProductModel, ProductDtoModel>().ReverseMap(); //reverse so the both direction

            //address module mapping
            CreateMap<EmployeeStudentParentModel, EmployeeStudentParentDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<BankModel, BankDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<DepartmentModel, DepartmentDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<DesignationModel, DesignationDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<DegreeModel, DegreeDtoModel>().ReverseMap(); //reverse so the both direction
            CreateMap<EmployeeQualificationModel, EmployeeQualificationDtoModel>().ReverseMap(); //reverse so the both direction

            //            CreateMap<EmployeeEntityTemplate, EmployeeDtoAbstractModel>().ReverseMap(); //reverse so the both direction
        }
    }
}
