using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EmailService;


namespace GenericFunction.GlobalService.EmailService.Contracts;

public interface IEmailService// : IGenericContract<EmailMaster>
{

    //Task AddAsync(EmailMaster emailMaster);
    Task Send(string to, string subject, string html, string? from = null);
    /// <summary>
    /// To Generate Body for an Response Object when exception occurred.
    /// </summary>
    /// <param name="responseDto"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    string GenerateExceptionBody(ResponseDto<object> responseDto, string? id);

    /// <summary>
    /// To Generate Subject and Body for Demo Request Email.
    /// </summary>
    /// <param name="responseDto">Response object</param>
    /// <param name="id">Record Id</param>
    /// <param name="generate">If request for demo is already generate it will return True, otherwise return False </param>
    /// <returns>Return Tupple with Subject and Body</returns>
    (string Subject, string Body) GenerateDemoRequestBody(ResponseDto<dynamic> responseDto, string? id, bool generate);
    //(string Subject, string Body) GenerateSchoolActivationBody(ResponseDto<CompanyMasterDtoModel> responseDto, string? id, bool generate);

    /// <summary>
    /// Send Email to logged in User/ and its Admin. If User is not logged in, it will send exception email to CC support email id.
    /// </summary>
    /// <param name="request">Mail request object</param>
    /// <returns>Return mail response object</returns>
    Task<MailResponse> SendEmailAsync(MailRequest request);

    (string subject, string body) CreateNewUserEmailBody(ResponseDto<dynamic> responseDto,
         dynamic company, string confirmationLink);
}