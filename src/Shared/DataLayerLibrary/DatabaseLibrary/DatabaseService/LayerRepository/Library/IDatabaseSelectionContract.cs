using ModelTemplates.EntityModels.DatabaseConfig;

namespace DataBaseServices.LayerRepository.Library;

public interface IDatabaseSelectionContract //: IGenericContract<DatabaseConnection, DatabaseConnection>
{
    //define such methods which are not common
    //object New();
    Task<DatabaseConnection?> GetCatelog(string? catalogName = "");
    Task<DatabaseConnection?> GetById(string? id);

}