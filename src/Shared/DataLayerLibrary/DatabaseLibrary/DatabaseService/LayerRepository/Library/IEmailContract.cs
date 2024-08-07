//using DataBaseServices.Persistence.BaseContract;
//using GenericFunction.ResultObject;
//using GenericFunction.ServiceObjects.EmailService;
//using ModelTemplates.Core.Model;
//using ModelTemplates.DtoModels.Company;
//using ModelTemplates.DtoModels.EmployeeEntityTemplate;

//namespace DataBaseServices.LayerRepository.Library;

//public interface IEmailContract : IGenericContract<EmailMaster, EmailDtoAbstractModel>
//{
//    //define such methods which are not common
//    //object New();
//    //Task GetByName(string? name = "");
//    //Task<string?> Save(EmailMaster model, ExecuteWith exeWith);
//    string GenerateExceptionBody(ResponseDto<object> responseDto, string? id);
//    string GenerateDemoRequestBody(ResponseDto<DemoRequestDtoModel> responseDto, string? id);
//    Task<MailResponse> SendEmailAsync(MailRequest request);

//}