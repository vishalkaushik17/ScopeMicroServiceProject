//using Microsoft.EntityFrameworkCore;
//using ModelTemplates.Master.Company;
//using DataCacheLayer.CacheRepositories.Interfaces;
//using DBOperationsLayer.Data.Context;

//namespace DBOperationsLayer.CacheLayer.Contracts
//{
//    public interface ICacheService
//    {
     

//    }

//    public class CacheService : ICacheService
//    {
//        private readonly ApplicationDbContext _dbContext;
//        private readonly ICacheContract _cacheData;
//        public CacheService(ICacheContract _cache, ApplicationDbContext dbContext)
//        {
//            _cacheData = _cache;
//            _dbContext = dbContext;
//        }

        

//        public async Task<T> ReadCache<T>(string key, DateTimeOffset life, bool specificKey,ApplicationDbContext dbContext)
//        {
//            var cacheDataList = await _cacheData.ReadFromCacheAsync<T>(key, 0, specificKey);
//            if (cacheDataList == null)
//            {
//                DbSet<T> model = new DbSet<T>();
//                cacheDataList = await .Include(m => m.CompanyMasterProfile).ToListAsync();
//                await _cacheData.AddCacheAsync<List<CompanyMasterModel>>(key, cacheDataList, life);
//            }
//            return com
//        }

//        public Task<T> WriteCache<T>()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
