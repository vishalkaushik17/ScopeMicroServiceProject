using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.Core.Contracts.UsersAndRoles
{
    public interface IDataLayerRoleContract
    {
        Task<List<UserRoles>> GetAll();
    }
}
