using GenericFunction.ResultObject;
using ModelTemplates.RequestNResponse.Accounts;

namespace BSAuthentication.BsInterface.AccountService;
public interface IBsAccountService
{
    Task<ResponseDto<ApplicationUserDtoModel>> Authenticate(AuthenticateRequest model, string ipAddress, bool UseCache);
    Task<ApplicationUserDtoModel> RefreshToken(string token, string ipAddress);
    //void RevokeToken(string token, string ipAddress);
    Task Register(RegisterRequest model, string origin);
    Task VerifyEmail(string token);
    Task<ResponseDto<ResponseOnActivation>> AccountConfirmation(string confirmId);
    Task ForgotPassword(ForgotPasswordRequest model, string origin);
    //void ValidateResetToken(ValidateResetTokenRequest model);
    //void ResetPassword(ResetPasswordRequest model);
    //List<AccountResponse> GetAll();
    //AccountResponse GetById(int id);
    //AccountResponse Create(CreateRequest model);

    //AccountResponse Update(int id, UpdateRequest model);
    //void Delete(int id);
}