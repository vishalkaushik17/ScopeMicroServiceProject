using GenericFunction;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UnitOfWork.DI;

public class TokenService : ITokenService
{
    private const double EXPIRY_DURATION_MINUTES = 30;


    public bool ValidateToken(string key, string issuer, string audience, string access_token, out string Message, out Exception exception)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwthandler = new JwtSecurityTokenHandler();

        var jwttoken = jwthandler.ReadToken(access_token as string);
        //var expDate = jwttoken.ValidTo.ToLocalTime();
        //if (jwttoken.ValidTo < CreatedOn.UtcNow.AddMinutes(EXPIRY_DURATION_MINUTES))
        //{
        //    Message = "Token is expired!";
        //}
        //string token;
        //if (expDate < CreatedOn.UtcNow.AddMinutes(1))
        //    token = GetAccessToken().Result;
        //else
        //    token = Session["access_token"] as string;
        try
        {
            tokenHandler.ValidateToken(access_token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,


                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
        }
        catch (SecurityTokenExpiredException ltException)
        {
            Message = "Token is expired!";
            exception = ltException;
            return false;
        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            Message = ex.Message;
            exception = ex;
            return false;
        }
        Message = "Success!";
        exception = null!;
        return true;
    }
}