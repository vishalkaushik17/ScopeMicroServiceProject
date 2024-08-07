//using DBOperationsLayer.Data.Context;


//namespace UnitOfWork.DbInit;

///// <summary>
///// This class will run to insert required data to database table.
///// </summary>
//public class DatabaseInitRepository : IDatabaseInit
//{
//    private readonly ApplicationDbContext _context;

//    public DatabaseInitRepository(ApplicationDbContext dbContext)
//    {
//        _context = dbContext;
//    }
//    /// <summary>
//    /// First time data insert through this method.
//    /// when data base is created first time and App is running first time.
//    /// </summary>
//    /// <returns></returns>
//    //public async Task AppStartupDataConfiguration()
//    //{
//    //    await _context.Database.EnsureCreatedAsync().ConfigureAwait(false);
//    //    if (_context.Employees != null && !_context.Employees.Any())
//    //    {
//    //        var emp = new EmployeeEntityTemplate()
//    //        {
//    //            Name = "Vishal",
//    //            Id = "a"
//    //        };
//    //        _context.Employees.Add(emp);
//    //        await _context.SaveChangesAsync().ConfigureAwait(false);
//    //    }
//    //}
//    public async Task AppStartupDataConfiguration()
//    {
//        await _context.Database.EnsureCreatedAsync().ConfigureAwait(false);
//    }
//}