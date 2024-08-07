using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.LayerRepository.Library;

public interface IUserContract //: IGenericContract<ApplicationUser, ApplicationUser>
{
    //define such methods which are not common
    //object New();
    Task<ApplicationUser?> GetByName(string? name = "");
    Task<ApplicationUser?> GetById(string? id);

}