using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.LayerRepository.Library;

public interface IRolesContract //: IGenericContract<UserRoles, UserRoles>
{
    //define such methods which are not common
    //object New();
    Task<UserRoles?> GetByName(string? name = "");
    Task<UserRoles?> GetById(string? id);
    IQueryable<UserRoles> All();

}