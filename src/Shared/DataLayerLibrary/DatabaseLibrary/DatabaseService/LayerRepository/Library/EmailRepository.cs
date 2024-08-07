//using AutoMapper;
//using DataBaseServices.Persistence.BaseContract;
//using DBOperationsLayer.Data.Context;
//using GenericFunction.Enums;
//using GenericFunction.Helpers;
//using GenericFunction.ResultObject;
//using GenericFunction.ServiceObjects.EmailService;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using ModelTemplates.Core.Model;
//using ModelTemplates.DtoModels.Company;
//using ModelTemplates.DtoModels.EmployeeEntityTemplate;

//namespace DataBaseServices.LayerRepository.Library;

//public class EmailRepository : BaseGenericRepository<EmailMaster, EmailDtoAbstractModel>, IEmailContract
//{


//    public EmailRepository(ApplicationDbContext dbContext, IMapper mapper,
//        IOptions<MailConfiguration> appSettings) : base(dbContext, mapper, appSettings)
//    {

//    }
//    public async Task<MailResponse> SendEmailAsync(MailRequest request)
//    {
//        MailResponse returnResponse = new MailResponse();
//        try
//        {

//            if (request.ToEmail.Length == 0)
//            {
//                returnResponse.Message = "No receiver defined!";
//                returnResponse.IsSuccess = false;
//                return returnResponse;
//            }

//            var email = new MimeMessage();
//            //if request does not have email setup, it will send from
//            //default email id
//            email.Sender = MailboxAddress.Parse(request.Credential?.Mail ?? _mailConfiguration.EmailFrom);


//            {
//                foreach (var receiver in request.ToEmail)
//                {
//                    email.To.Add(MailboxAddress.Parse(receiver));
//                }

//                if (request.CCEmail != null)
//                    foreach (var ccReceiver in request.CCEmail)
//                    {
//                        email.Cc.Add(MailboxAddress.Parse(ccReceiver));
//                    }

//                email.Subject = request.Subject;

//                BodyBuilder builder = new()
//                {
//                    HtmlBody = request.Body
//                };

//                if (request.Attachments != null)
//                {
//                    foreach (var file in request.Attachments)
//                    {
//                        if (file.Length > 0)
//                        {
//                            byte[] fileBytes;
//                            using (var ms = new MemoryStream())
//                            {
//                                await file.CopyToAsync(ms);
//                                fileBytes = ms.ToArray();
//                            }

//                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
//                        }
//                    }
//                }

//                email.Subject = request.EmailType switch
//                {
//                    EmailNotificationType.EXCEPTION => "Exception occurred!",
//                    EmailNotificationType.ALERT => "Alert!",
//                    EmailNotificationType.LOGINALERT => "Login Alert!",
//                    EmailNotificationType.OTP => "OTP Message!",
//                    EmailNotificationType.TEST => "Testing email service!",
//                    EmailNotificationType.MESSAGE => "Information!",
//                    _ => "Email message!"
//                };
//                email.Body = builder.ToMessageBody();

//                using var smtp = new SmtpClient();
//                await smtp.ConnectAsync(request.Credential?.Host ?? _mailConfiguration.SmtpHost,
//                    request.Credential?.Port ?? _mailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
//                await smtp.AuthenticateAsync(request.Credential?.Mail ?? _mailConfiguration.EmailFrom,
//                    request.Credential?.Mail ?? _mailConfiguration.SmtpPass);
//                var sent = await smtp.SendAsync(email);
//                await smtp.DisconnectAsync(true);

//                //Set Response
//                returnResponse.FromEmail = _mailConfiguration.EmailFrom;
//                returnResponse.Message = "Email sent!";
//                returnResponse.InternalMessage = sent;
//            }

//            returnResponse.IsSuccess = true;
//            return returnResponse;

//        }
//        catch (Exception e)
//        {
//            //when exception occurred, send exception email to sender email,
//            //from system default email

//            MailRequest mailRequest = new()
//            {
//                Subject = e.Message,
//                EmailType = EmailNotificationType.ALERT,
//                ToEmail = request.ToEmail.Length == 0 ? new[] { _mailConfiguration.EmailFrom } : new[] { request.Credential?.Mail },
//                CCEmail = Array.Empty<string>(),
//                Body = "Email service is not working",
//            };

//            var failedEmailStatus = SendEmailAsync(mailRequest);

//            returnResponse.Message = failedEmailStatus.Result.Message;
//            returnResponse.InternalMessage = e.Message;
//            returnResponse.IsSuccess = false;
//            return returnResponse;
//        }

//    }
//    //public async Task<string?> Save(EmailMaster model, ExecuteWith exeWith)
//    //{
//    //    model.Id = Guid.NewGuid().ToString("D");
//    //    var arrParameters = new[]
//    //    {
//    //        new SqlParameter() {ParameterName = SpGetAirline.Id, SqlDbType = SqlDbType.NVarChar, Value= model.Id = Guid.NewGuid().ToString("D")},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.AirportName, SqlDbType = SqlDbType.NVarChar, Value= model.AirportName},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.Country, SqlDbType = SqlDbType.NVarChar, Value= model.Country},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.City, SqlDbType = SqlDbType.NVarChar, Value= model.City},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.AirportCode, SqlDbType = SqlDbType.NVarChar, Value= model.AirportCode},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.OutId, SqlDbType = SqlDbType.NVarChar, Value= model.AirportCode,Direction=System.Result.ParameterDirection.Output, Size=450}
//    //    };

//    //    if (exeWith == ExecuteWith.Ado)
//    //    {
//    //        DbExecution dbExecute = new();
//    //        (int records, SqlCommand sqlCmd) = dbExecute.ADOExecuteDMLSP(SpSaveAirline.ToName(), arrParameters);

//    //        if (records > 0)
//    //        {
//    //            return sqlCmd?.Parameters[SpGetAirline.OutId].Value.ToString();
//    //        }
//    //    }
//    //    else
//    //    {
//    //        DbExecution dbExecute = new(DbContext);

//    //        (int records, Dictionary<string, SqlParameter> outParams) = dbExecute.EntityExecuteDMLSP(SpSaveAirline.ToName(), arrParameters);

//    //        if (records > 0)
//    //        {
//    //            var myValue = outParams.FirstOrDefault(x => x.Key == SpGetAirline.OutId);
//    //            return (string)myValue.Value.Value;
//    //        }
//    //    }

//    //    return null;
//    //}

//    public string GenerateExceptionBody(ResponseDto<object> responseDto, string? id)
//    {

//        string result;
//        using (StreamReader reader =
//               new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), @"Templates\Exception\ExceptionPage.html")))
//        {
//            result = reader.ReadToEnd();
//        }

//        result = result.Replace("{ClientName}", responseDto.ClientName);
//        result = result.Replace("{UserId}", id);
//        result = result.Replace("{DateTime}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt"));
//        result = result.Replace("{DocumentNo}", Guid.NewGuid().ToString("D"));
//        result = result.Replace("{ErrorMessage}", responseDto.Message);
//        result = result.Replace("{ClassName}", responseDto.ExceptionRaisedOn.ClassName);
//        result = result.Replace("{MethodName}", responseDto.ExceptionRaisedOn.MethodName);
//        result = result.Replace("{LineNo}", responseDto.ExceptionRaisedOn.AtLineNo.ToString());
//        result = result.Replace("{StackTrace}", responseDto.Exception?.Message);
//        result = result.Replace("{CompId}", responseDto.ClientId);
//        return result;

//    }
//    public string GenerateDemoRequestBody(ResponseDto<DemoRequestDtoModel> responseDto, string? id)
//    {

//        string result;

//        using (StreamReader reader =
//               new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), @"Templates\DemoRequest\DemoRequest.html")))
//        {
//            result = reader.ReadToEnd();
//        }

//        result = result.Replace("{ClientName}", responseDto.ClientName);
//        result = result.Replace("{Id}", id);
//        result = result.Replace("{DateTime}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt"));
//        //result = result.Replace("{DocumentNo}", Guid.NewGuid().ToString("D"));
//        result = result.Replace("{ContactPerson}", responseDto.Result.ContactPerson);
//        result = result.Replace("{EmailId}", responseDto.Result.Email);
//        result = result.Replace("{PhoneNo}", responseDto.Result.ContactNo);
//        result = result.Replace("{ContactEmail}", _mailConfiguration.ContactEmail);
//        return result;

//    }
//}