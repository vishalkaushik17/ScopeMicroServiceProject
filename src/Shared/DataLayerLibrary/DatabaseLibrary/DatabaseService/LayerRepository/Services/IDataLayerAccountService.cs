using DBOperationsLayer.Data.Context;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Identity;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.RequestNResponse.Accounts;

namespace DataBaseServices.LayerRepository.Services;

public interface IDataLayerAccountService
{
    Task<bool> IsRefreshTokenAvaiable(string token);
    Task<ApplicationUserDtoModel> Authenticate(AuthenticateRequest model, string ipAddress);
    Task<ApplicationUserDtoModel> RefreshToken(string token, string ipAddress);
    Task RevokeToken(string token, string ipAddress);
    Task Register(RegisterRequest model, string origin);
    Task<ApplicationUser?> VerifyEmail(string token);
    Task<ResponseOnActivation> AccountConfirmation(string confirmId);
    Task ForgotPassword(ForgotPasswordRequest model, string origin);
    Task ValidateResetToken(ValidateResetTokenRequest model);
    Task ResetPassword(ResetPasswordRequest model);
    Task<List<AccountResponse>> GetAll();
    Task<ApplicationUser> GetUserWithCompanyData(string username, string email, ApplicationDbContext? newContext);
    Task<ApplicationUser?> FindUser(string username, string email);
    Task<ApplicationUser?> FindUserById(string id);
    Task<ApplicationUser?> GetUserbyResetToekn(string token);
    Task<ApplicationUser?> GetUserWithRefreshToekn(string token);
    Task<AccountResponse> GetById(int id);
    Task<IdentityResult> CreateUser(ApplicationUser user, string password);
    Task UpdateUser(ApplicationUser user);
    Task<AccountResponse> Create(CreateRequest model);
    Task<AccountResponse> Update(int id, UpdateRequest model);
    Task Delete(int id);

    //refresh token

    Task<RefreshToken> AddRefreshToken(RefreshToken token);
    Task<bool> IsTokenAvaiable(string token);
    Task<bool> IsVerificationTokenAvailable(string token);
}