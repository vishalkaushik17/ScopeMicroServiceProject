using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ModelTemplates.EntityModels.Application;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static GenericFunction.CommonMessages;
namespace DataBaseServices.LayerRepository;

public class JwtUtils : IJwtUtils
{
	private readonly ApplicationDbContext context;
	private readonly RoleManager<UserRoles> _roleManager;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly MailConfiguration _mailConfiguration;
	private readonly bool _isTracingRequired;

	//private readonly IDataLayerAccountService _dataLayerAccountService;
	public JwtUtils(
		ApplicationDbContext _context,
		RoleManager<UserRoles> roleManager,
		UserManager<ApplicationUser> userManager
		/*IDataLayerAccountService dataLayerAccountService*/)
	{
		context = _context;
		_roleManager = roleManager;
		_userManager = userManager;
		//_dataLayerAccountService = dataLayerAccountService;
	}

	public async Task<string> GenerateJwtToken(ApplicationUser user, IList<string> roles, string sessionId, string scopeId)
	{
		// generate token that is valid for 15 minutes
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes((string)SettingsConfigHelper.AppSetting("TokenConfig", "Secret"));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(UserId, user.Id), //user specific id eg USR-XXXX
                new Claim(ClientId, user.CompanyId), // eg CMP-XXX
                new Claim(UserName, user.UserName), // vishalkaushk@cc.in
                new Claim(ClientName, user.Company.Name), // coding company 
                new Claim(UserEmail, user.Email), // user personal email
                new Claim(ClientEmail, user.Company.Email), //company email id
                new Claim(CompanyTypeId, user.CompanyTypeId), //CPT-XXXX
                new Claim(TokenSessionId, sessionId), //sesion from which user is going to access the webapi 
                new Claim(TokenScopeId, scopeId), // it used for tracing and other activity of user
                new Claim(IsDemoExpired, user.Company.IsDemoExpired == true?"true":"false"), // it used take decision for client to make read/write or readonly connection.
                

            }),
			Expires = DateTime.UtcNow.AddDays(15),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		roles.ToList().ForEach((role) =>
		{
			tokenDescriptor.Subject.AddClaims(new Claim[] { new Claim(Role, role) });
			//tokenDescriptor.Subject.AddClaims(new Claim[] { new Claim(Role, "TestRole") });
		});

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return await Task.Run(() => tokenHandler.WriteToken(token));
	}

	public async Task<JtwTokenContainerResponse?> ValidateJwtToken(string? token, ITrace trace)
	{
		if (token == null)
			return null;
		trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), "Validating Auth Token!");
		JtwTokenContainerResponse response = new JtwTokenContainerResponse();
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes((string)SettingsConfigHelper.AppSetting("TokenConfig", "Secret"));
		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			response.UserId = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == UserId).Value;
			response.ClientId = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == ClientId).Value;
			response.CompanyTypeId = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == CompanyTypeId).Value;
			response.UserName = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == UserName).Value;
			response.ClientName = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == ClientName).Value;
			response.UserEmail = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == UserEmail).Value;
			//response.PersonalEmailId = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == PersonalEmailId).Value;
			response.ClientEmail = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == ClientEmail).Value;
			//response.FirstName = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == FirstName).Value;
			//response.LastName = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == LastName).Value;
			//response.ClientEmail = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == ClientEmail).Value;
			response.TokenScopeId = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == TokenScopeId).Value;
			response.TokenSessionId = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == TokenSessionId).Value;
			response.IsDemoExpired = Enumerable.First<Claim>(jwtToken.Claims, x => x.Type == IsDemoExpired).Value == "true" ? true : false;

			//adding roles
			jwtToken.Claims.Where(x => x.Type == Role).ToList().ForEach((role) =>
			{
				response.Roles.Add(role.Value);
			});

			//response.Roles = jwtToken.Claims.Where(x=>x.Type== Role).ToArray();

			trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{response.UserName} : User logged in with Roles - {response.Roles.ToString()}");
			trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), "Valid Auth Token Found!");

			// return user id from JWT token if validation successful
			return await Task.Run(() => response);
		}
		catch (Exception ex)
		{
			trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Error while validating Auth Token! {ex.Message}");
			ex.SendExceptionMailAsync().GetAwaiter().GetResult();
			// return null if validation fails
			return null;
		}
	}

	public async Task<RefreshToken> GenerateRefreshToken(string userId, string companyId)
	{
		var refreshToken = new RefreshToken();

		refreshToken.GenerateRefreshToken(userId, companyId);

		// ensure token is unique by checking against db
		//var tokenIsUnique = await _dataLayerAccountService.IsRefreshTokenAvaiable(refreshToken.Token);
		var tokenIsUnique = !context.User.Any(a => a.RefreshTokens.Any(t => t.Token == refreshToken.Token));

		if (!tokenIsUnique)
			return await GenerateRefreshToken(userId, companyId);

		return refreshToken;
	}


}