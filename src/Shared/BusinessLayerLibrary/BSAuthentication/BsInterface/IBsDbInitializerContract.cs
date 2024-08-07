using DBOperationsLayer.Data.Context;
using ModelTemplates.Persistence.Models.AppLevel;

namespace BSAuthentication.BsInterface;

public interface IBsDbInitializerContract
{
    string InitializeWithDefaults(ApplicationDbContext _dbContext);
    string InitializeOnNewDb(ApplicationDbContext _dbContext);
    
    
    string Initialize();
    string UpdateChangesToDb(ApplicationDbContext _dbContext);
    
    bool AddDefaultSystemPreferences(List<SystemPreferencesModel> systemPreferences, ApplicationDbContext dbContext);
    List<SystemPreferencesModel> GetDefaultPreferences(ApplicationDbContext dbContext);
}
