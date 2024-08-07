using AutoMapper;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelTemplates.Core.Model;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.RequestNResponse.Accounts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataBaseServices.LayerRepository.Services;

public class DLAccountService : BaseGenericRepository<EmailMaster>, IDataLayerAccountService
{
    //private readonly ApplicationDbContext context;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    ////private readonly MailConfiguration _mailConfiguration;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationUserDtoModel _ApplicationUserDtoModel = new ApplicationUserDtoModel();

    public DLAccountService(
        ApplicationDbContext context,
        IJwtUtils jwtUtils,
        IMapper mapper,
        IEmailService emailService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ITrace trace)
        : base(context, httpContextAccessor, trace)
    {
        //context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        //_mailConfiguration = appSettings.Value;
        _emailService = emailService;
        _userManager = userManager;
    }






    //===============Private fields

    // helper methods

    private ApplicationUser getAccount(int id)
    {
        var user = _dbContext.User.Find(id);
        if (user == null) throw new KeyNotFoundException("Account not found");
        return user;
    }

    private ApplicationUser getAccountByRefreshToken(string token)
    {
        var user = _dbContext.User.Include(a => a.RefreshTokens).FirstOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        if (user == null) throw new AppException("Invalid token");
        return user;
    }

    private ApplicationUser getAccountByResetToken(string token)
    {
        var account = _dbContext.User.SingleOrDefault(x =>
            x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
        if (account == null) throw new AppException("Invalid token");
        return account;
    }

    private string generateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes((string)SettingsConfigHelper.AppSetting("TokenConfig", "Secret"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string generateResetToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        // ensure token is unique by checking against db
        var tokenIsUnique = !_dbContext.User.Any(x => x.ResetToken == token);
        if (!tokenIsUnique)
            return generateResetToken();

        return token;
    }

    private string generateVerificationToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        // ensure token is unique by checking against db
        var tokenIsUnique = !_dbContext.User.Any(x => x.VerificationToken == token);
        if (!tokenIsUnique)
            return generateVerificationToken();

        return token;
    }

    private async Task<RefreshToken> rotateRefreshToken(RefreshToken refreshToken, string userId, string companyId)
    {
        var newRefreshToken = await _jwtUtils.GenerateRefreshToken(userId, companyId);
        revokeRefreshToken(refreshToken, userId, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void removeOldRefreshTokens(ApplicationUser user)
    {
        var tokenlist = user.RefreshTokens.Where(x =>
            !x.IsActive &&
            x.Created.AddDays(int.Parse(SettingsConfigHelper.AppSetting("TokenConfig", "Secret"))) <= DateTime.UtcNow).ToList();

        user.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(int.Parse(SettingsConfigHelper.AppSetting("TokenConfig", "Secret"))) <= DateTime.UtcNow);
    }

    private void revokeDescendantRefreshTokens(RefreshToken? refreshToken, ApplicationUser user, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken?.ReplacedByToken))
        {
            var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken != null && childToken.IsActive)
                revokeRefreshToken(childToken, ipAddress, reason);
            else
                revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    private void sendVerificationEmail(ApplicationUser user, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            // origin exists if request sent from browser single page app (e.g. Angular or React)
            // so send link to verify via single page app
            var verifyUrl = $"{origin}/user/verify-email?token={user.VerificationToken}";
            message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
        }
        else
        {
            // origin missing if request sent directly to api (e.g. from Postman)
            // so send instructions to verify directly with api
            message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                            <p><code>{user.VerificationToken}</code></p>";
        }

        _emailService.Send(
            to: user.Email,
            subject: "Sign-up Verification API - Verify Email",
            html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
        );
    }

    private void sendAlreadyRegisteredEmail(string email, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
            message = $@"<p>If you don't know your password please visit the <a href=""{origin}/user/forgot-password"">forgot password</a> page.</p>";
        else
            message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

        _emailService.Send(
            to: email,
            subject: "Scope - Email Already Registered",
            html: $@"<h4>Email Already Registered</h4>
                        <p>Your email <strong>{email}</strong> is already registered.</p>
                        {message}"
        );
    }

    private void sendPasswordResetEmail(ApplicationUser user, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            var resetUrl = $"{origin}/user/reset-password?token={user.ResetToken}";
            message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
        }
        else
        {
            message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                            <p><code>{user.ResetToken}</code></p>";
        }

        _emailService.Send(
            to: user.Email,
            subject: "Scope - Reset Password",
            html: $@"<h4>Reset Password Email</h4>
                        {message}"
        );
    }

    public Task<ApplicationUserDtoModel> Authenticate(AuthenticateRequest model, string ipAddress)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUserDtoModel> RefreshToken(string token, string ipAddress)
    {
        throw new NotImplementedException();
    }

    public Task RevokeToken(string token, string ipAddress)
    {
        throw new NotImplementedException();
    }

    public Task Register(RegisterRequest model, string origin)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> VerifyEmail(string token)
    {
        return await Task.Run(() => _dbContext.User.SingleOrDefault(x => x.VerificationToken == token));
    }

    public Task<ResponseOnActivation> AccountConfirmation(string confirmId)
    {
        throw new NotImplementedException();
    }

    public Task ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        throw new NotImplementedException();
    }

    public Task ValidateResetToken(ValidateResetTokenRequest model)
    {
        throw new NotImplementedException();
    }

    public Task ResetPassword(ResetPasswordRequest model)
    {
        throw new NotImplementedException();
    }

    public Task<List<AccountResponse>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> GetUserWithCompanyData(string username, string email, ApplicationDbContext? newDbContext)
    {
        try
        {
            if (newDbContext != null)
            {
                this._dbContext = newDbContext;
            }
            ApplicationUser? usr = _dbContext.User.Include(m => m.Company).FirstOrDefault(x => x.UserName == username || x.Email == email);

            return await Task.Run(() => usr);
        }
        catch (Exception ex)
        {
            return default;
        }
        return default;
    }
    public async Task<ApplicationUser?> FindUser(string username, string email)
    {
        return await Task.Run(() => _dbContext.User.FirstOrDefault(x => x.UserName == username || x.Email == email));
    }
    public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);

    }


    public Task<AccountResponse> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<AccountResponse> Create(CreateRequest model)
    {
        throw new NotImplementedException();
    }

    public Task<AccountResponse> Update(int id, UpdateRequest model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken token)
    {
        await _dbContext.RefreshTokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
        return token;
    }

    public async Task UpdateUser(ApplicationUser user)
    {
        await _dbContext.SaveChangesAsync();
    }

    public Task<ApplicationUser?> FindUserById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> GetUserWithRefreshToekn(string token)
    {
        return await _dbContext.User.Include(a => a.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
    }
    public async Task<ApplicationUser?> GetUserbyResetToekn(string token)
    {
        return await _dbContext.User.SingleOrDefaultAsync(x =>
            x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
    }

    public async Task<bool> IsTokenAvaiable(string token)
    {
        return await _dbContext.User.AnyAsync(x => x.ResetToken == token);
    }
    public async Task<bool> IsRefreshTokenAvaiable(string token)
    {
        return await _dbContext.User.AnyAsync(a => a.RefreshTokens.Any(t => t.Token == token));
    }
    public async Task<bool> IsVerificationTokenAvailable(string token)
    {
        return await _dbContext.User.AnyAsync(x => x.VerificationToken == token);
    }
}