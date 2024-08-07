using DataBaseServices.Persistence.BaseContract;
using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.Core.Contracts.UsersAndRoles
{
    public interface IDataLayerUserContract : IDefaultInterfaceServices
    {
        Task<ApplicationUser?> Get(string id);
        Task<List<ApplicationUser>?> GetUsersByClientId(string ClientId);

    }
}
