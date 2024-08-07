//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

using GenericFunction.Enums;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GenericFunction.ServiceObjects.EmailService;
#pragma warning disable RCS1102 // Make class static.

/// <summary>
/// Mail Service class
/// </summary>
public class ScopeMailService
{
    public static MailConfiguration MailConfiguration;

    static ScopeMailService()
    {
        MailConfiguration = new MailConfiguration();
        MailConfiguration.EmailFrom = SettingsConfigHelper.AppSetting("MailConfiguration", "EmailFrom");
        MailConfiguration.SmtpHost = SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpHost");
        MailConfiguration.SmtpPort = int.Parse(SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpPort"));
        MailConfiguration.SmtpUser = SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpUser");
        MailConfiguration.SmtpPass = SettingsConfigHelper.AppSetting("MailConfiguration", "SmtpPass");
        MailConfiguration.ContactEmail = SettingsConfigHelper.AppSetting("MailConfiguration", "ContactEmail");


    }

    public ScopeMailService(IOptions<MailConfiguration> appSettings)
    {
        MailConfiguration = appSettings.Value;
    }
    public ScopeMailService(MailConfiguration appSettings)
    {
        MailConfiguration = appSettings;
    }


    /// <summary>
    /// Send Email 
    /// </summary>
    /// <param name="request">Use ResponseDto object for Sender|To|Subject|Body</param>
    /// <returns>Async Boolean</returns>
    public static async Task<MailResponse> SendEmailAsync(MailRequest request)
    {
        MailResponse returnResponse = new MailResponse();
        try
        {

            if (request.ToEmail.Length == 0)
            {
                returnResponse.Message = "No receiver defined!";
                returnResponse.IsSuccess = false;
                return returnResponse;
            }

            var email = new MimeMessage();
            //if request does not have email setup, it will send from
            //default email id
            email.Sender = MailboxAddress.Parse(request.Credential?.Mail ?? MailConfiguration.EmailFrom);


            {
                foreach (var receiver in request.ToEmail)
                {
                    email.To.Add(MailboxAddress.Parse(receiver));
                }

                if (request.CCEmail != null)
                    foreach (var ccReceiver in request.CCEmail)
                    {
                        email.Cc.Add(MailboxAddress.Parse(ccReceiver));
                    }

                email.Subject = request.Subject;

                BodyBuilder builder = new()
                {
                    HtmlBody = request.Body
                };

                if (request.Attachments != null)
                {
                    foreach (var file in request.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            byte[] fileBytes;
                            using (var ms = new MemoryStream())
                            {
                                await file.CopyToAsync(ms);
                                fileBytes = ms.ToArray();
                            }

                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                string? subject = email?.Subject;
                email.Subject = request.EmailType switch
                {
                    EmailNotificationType.EXCEPTION => "Exception occurred!",
                    EmailNotificationType.ALERT => "Alert!",
                    EmailNotificationType.LOGINALERT => "Login Alert!",
                    EmailNotificationType.OTP => "OTP Message!",
                    EmailNotificationType.TEST => "Testing email service!",
                    EmailNotificationType.MESSAGE => "Information!",
                    _ => "Email message!"
                };
                email.Subject += " " + subject;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(request.Credential?.Host ?? MailConfiguration.SmtpHost,
                    request.Credential?.Port ?? MailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(request.Credential?.Mail ?? MailConfiguration.EmailFrom,
                    request.Credential?.Mail ?? MailConfiguration.SmtpPass);
                var sent = await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                //Set Response
                returnResponse.FromEmail = MailConfiguration.EmailFrom;
                returnResponse.Message = "Email sent!";
                returnResponse.InternalMessage = sent;
            }

            returnResponse.IsSuccess = true;
            return returnResponse;

        }
        catch (Exception e)
        {
            //when exception occurred, send exception email to sender email,
            //from system default email

            MailRequest mailRequest = new()
            {
                Subject = e.Message,
                EmailType = EmailNotificationType.ALERT,
                ToEmail = request.ToEmail.Length == 0 ? new[] { MailConfiguration.EmailFrom } : new[] { request.Credential?.Mail },
                CCEmail = Array.Empty<string>(),
                Body = "Email service is not working",
            };

            var failedEmailStatus = SendEmailAsync(mailRequest);

            returnResponse.Message = failedEmailStatus.Result.Message;
            returnResponse.InternalMessage = e.Message;
            returnResponse.IsSuccess = false;
            return returnResponse;
        }

    }
}
