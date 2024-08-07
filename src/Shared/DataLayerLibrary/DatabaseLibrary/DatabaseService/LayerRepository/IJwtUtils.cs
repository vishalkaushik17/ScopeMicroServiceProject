using GenericFunction;
using GenericFunction.ResultObject;
using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.LayerRepository;

public interface IJwtUtils
{
    Task<string> GenerateJwtToken(ApplicationUser user, IList<string> roles,string sessionId,string scopeId);
    Task<JtwTokenContainerResponse?> ValidateJwtToken(string? token,ITrace trace);
    Task<RefreshToken> GenerateRefreshToken(string userId,string companyId);

}