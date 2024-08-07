namespace UnitOfWork.DI;

public interface ITokenService
{

    bool ValidateToken(string key, string issuer, string audience, string token, out string message, out Exception exception);
}