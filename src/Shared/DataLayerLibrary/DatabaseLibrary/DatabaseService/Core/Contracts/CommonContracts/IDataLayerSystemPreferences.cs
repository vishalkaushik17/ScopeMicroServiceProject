using ModelTemplates.Persistence.Models.AppLevel;

namespace DataBaseServices.Core.Contracts.CommonContracts;

public interface IDataLayerSystemPreferences
{
    Task<SystemPreferencesModel?> GetAsync(string id);
    Task<List<SystemPreferencesModel>?> GetAllAsync();
}
