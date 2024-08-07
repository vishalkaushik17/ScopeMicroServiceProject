//using GenericFunction;
//using GenericFunction.Enums;
//using GenericFunction.Helpers;
//using GenericFunction.ResultObject;
//using GenericFunction.ServiceObjects.EmailService;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
//using MimeKit.Text;
//using ModelTemplates.DtoModels.Company;
//using ModelTemplates.EntityModels.Application;
//using ModelTemplates.Master.Company;

//namespace DataBaseServices.LayerRepository.Services;

//public class EmailService : IEmailService
//{
//    protected MailConfiguration MailConfiguration;
//    public EmailService()
//    {
//        MailConfiguration = new MailConfiguration();
//        MailConfiguration.EmailFrom = SettingsConfigHelper.AppSetting("MailConfiguration", "EmailFrom");
//        MailConfiguration.SmtpHost = SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpHost");
//        MailConfiguration.SmtpPort = int.Parse(SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpPort"));
//        MailConfiguration.SmtpUser = SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpUser");
//        MailConfiguration.SmtpPass = SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpPass");
//        MailConfiguration.ContactEmail = SettingsConfigHelper.AppSetting("MailConfiguration", "ContactEmail");

//        //MailConfiguration = emailSettings.Value;
//    }

//    public async Task Send(string to, string subject, string html, string? from = null)
//    {
//        // create message
//        var email = new MimeMessage();
//        email.From.Add(MailboxAddress.Parse(from ?? MailConfiguration.EmailFrom));

//        email.To.Add(MailboxAddress.Parse(to));
//        email.Subject = subject;
//        email.Body = new TextPart(TextFormat.Html) { Text = html };

//        // send email
//        using var smtp = new SmtpClient();
//        await smtp.ConnectAsync(MailConfiguration.SmtpHost, MailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
//        await smtp.AuthenticateAsync(MailConfiguration.SmtpUser, MailConfiguration.SmtpPass);
//        await smtp.SendAsync(email);
//        await smtp.DisconnectAsync(true);
//    }

//    //public (string Subject, string Body) GenerateSchoolActivationBody(ResponseDto<CompanyMasterDtoModel> responseDto,UserDto userDto,  string? id, bool generate)
//    //{
//    //    string result;
//    //    string file = @"Templates\DemoRequest\NewDemoRequest.html";
//    //    if (generate)
//    //    {
//    //        file = @"Templates\DemoRequest\DemoRequest.html";
//    //    }

//    //    using (StreamReader reader =
//    //           new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), file)))
//    //    {
//    //        result = reader.ReadToEnd();
//    //    }

//    //    result = result.Replace("{ClientName}", responseDto.Result?.Name);
//    //    result = result.Replace("{Id}", id);
//    //    result = result.Replace("{DateTime}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt"));
//    //    result = result.Replace("{SchoolAdmin}", responseDto.Result?.ContactPerson);
//    //    result = result.Replace("{EmailId}", responseDto.Result?.Email);
//    //    result = result.Replace("{PhoneNo}", responseDto.Result?.ContactNo);
//    //    result = result.Replace("{ContactEmail}", _mailConfiguration.ContactEmail); //CC contact email id.

//    //    if (!generate)
//    //    {
//    //        return (@"We have received your Demo Request for Scope ERP Software.", result);
//    //    }

//    //    return (@"We have already registered your Demo Request for Scope ERP Software. Our team is processing your request!", result);
//    //}

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
//            email.Sender = MailboxAddress.Parse(request.Credential?.Mail ?? MailConfiguration.EmailFrom);


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
//                await smtp.ConnectAsync(request.Credential?.Host ?? MailConfiguration.SmtpHost,
//                    request.Credential?.Port ?? MailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
//                await smtp.AuthenticateAsync(request.Credential?.Mail ?? MailConfiguration.EmailFrom,
//                    request.Credential?.Mail ?? MailConfiguration.SmtpPass);
//                var sent = await smtp.SendAsync(email);
//                await smtp.DisconnectAsync(true);

//                //Set Response
//                returnResponse.FromEmail = MailConfiguration.EmailFrom;
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
//                ToEmail = request.ToEmail.Length == 0 ? new[] { MailConfiguration.EmailFrom } : new[] { request.Credential?.Mail },
//                CCEmail = Array.Empty<string>(),
//                Body = "Email service is not working",
//            };
//            try
//            {
//                var failedEmailStatus = SendEmailAsync(mailRequest);

//                returnResponse.Message = failedEmailStatus.Result.Message;
//                returnResponse.InternalMessage = e.Message;
//                returnResponse.IsSuccess = false;
//            }
//            catch (Exception exception)
//            {
//            }
//            return returnResponse;
//        }

//    }

//    public (string subject, string body) CreateNewUserEmailBody(ResponseDto<ApplicationUser> responseDto, CompanyMasterModel company, string confirmationLink)
//    {
//        string result;
//        string file = @"Templates\NewUserCreation\NewUserAccount.html";
//        using (StreamReader reader =
//               new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), file)))
//        {
//            result = reader.ReadToEnd();
//        }

//        result = result.Replace("{UserName}", responseDto.Result?.UserName);
//        result = result.Replace("{SchoolName}", company.Name);
//        result = result.Replace("{ActivationLink}", confirmationLink);

//        result = result.Replace("{DateTime}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt"));
//        result = result.Replace("{AccountHolderName}", responseDto.Result?.NormalizedUserName);
//        result = result.Replace("{EmailId}", responseDto.Result?.Email);
//        result = result.Replace("{PhoneNo}", responseDto.Result?.PhoneNumber);
//        result = result.Replace("{ContactEmail}", MailConfiguration.ContactEmail); //CC contact email id.

//        return (@"Congratulations! We have accepted your demo request for Scope ERP Software.", result);
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

//        result = result.Replace("{ClientName}", responseDto.ClientName ?? "Default");
//        result = result.Replace("{UserId}", id ?? "Default");
//        result = result.Replace("{ClientId}", id ?? "Default");
//        result = result.Replace("{DateTime}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt"));
//        result = result.Replace("{DocumentNo}", Guid.NewGuid().ToString("D"));
//        result = result.Replace("{ErrorMessage}", responseDto.Message);
//        result = result.Replace("{ClassName}", responseDto.ExceptionRaisedOn.ClassName);
//        result = result.Replace("{MethodName}", responseDto.ExceptionRaisedOn.MethodName);
//        result = result.Replace("{LineNo}", responseDto.ExceptionRaisedOn.AtLineNo.ToString());
//        result = result.Replace("{StackTrace}", responseDto.Exception?.Message);
//        result = result.Replace("{Logs}", responseDto.Log);
//        return result;

//    }
//    public (string Subject, string Body) GenerateDemoRequestBody(ResponseDto<DemoRequestDtoModel> responseDto, string? id, bool generate)
//    {
//        string result;
//        string file = @"Templates\DemoRequest\NewDemoRequest.html";
//        if (generate)
//        {
//            file = @"Templates\DemoRequest\DemoRequest.html";
//        }

//        using (StreamReader reader =
//                new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), file)))
//        {
//            result = reader.ReadToEnd();
//        }

//        result = result.Replace("{ClientName}", responseDto.Result?.Name);
//        result = result.Replace("{Id}", id);
//        result = result.Replace("{DateTime}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt"));
//        result = result.Replace("{ContactPerson}", $"{responseDto.Result?.FirstName} {responseDto.Result?.LastName}");
//        result = result.Replace("{EmailId}", responseDto.Result?.Email);
//        result = result.Replace("{PhoneNo}", responseDto.Result?.ContactNo);
//        result = result.Replace("{ContactEmail}", MailConfiguration.ContactEmail); //CC contact email id.

//        if (!generate)
//        {
//            return (@"We have received your Demo Request for Scope ERP Software.", result);
//        }

//        return (@"We have already registered your Demo Request for Scope ERP Software. Our team is processing your request!", result);
//    }

//    //public async Task AddAsync(EmailMaster emailMaster)
//    //{
//    //    await context.AddAsync(emailMaster);
//    //}
//}