//using DBOperationsLayer.Data.Constants;
//using DBOperationsLayer.Data.Context;
//using DBOperationsLayer.Data.StoredProcedure.Air.Params;
//using GenericFunction.Enums;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using System.Data;

//namespace DBOperationsLayer.Data
//{
//    public class DbExecution
//    {
//        private readonly DbContext _dbContext;
//        private readonly DbContext dbContextClient;
//        public DbExecution(ApplicationDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }
//        public DbExecution()
//        {

//        }
//        public Tuple<int, SqlCommand> ADOExecuteDMLSP(string spName, SqlParameter[] parameters)
//        {
//            if (parameters == null)
//            {
//                return Tuple.Create(0, new SqlCommand());
//            }

//            if (DbConnection.ConnectionType == EnumDBType.MSSQL)
//            {

//                using (SqlConnection connection = new SqlConnection(DbConnection.ConnectionString))
//                {
//                    using (SqlCommand cmd = new SqlCommand(spName, connection))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        foreach (var item in parameters)
//                        {
//                            if (item.Direction == ParameterDirection.Input)
//                            {
//                                cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
//                            }
//                            if (item.Direction == ParameterDirection.Output)
//                            {
//                                cmd.Parameters.Add(item.ParameterName, SqlDbType.NVarChar, 450);
//                                cmd.Parameters[item.ParameterName].Direction = ParameterDirection.Output;
//                            }

//                        }
//                        connection.Open();

//                        var a = cmd.ExecuteNonQuery();
//                        return Tuple.Create(a, cmd);
//                    }
//                }
//            }
//            return Tuple.Create(0, new SqlCommand());
//        }

//        public Tuple<int, Dictionary<string, SqlParameter>> EntityExecuteDMLSP(string spName, SqlParameter[] parameters)
//        {
//            string paramString = string.Empty;
//            int countOutParams = 0;
//            foreach (var item in parameters)
//            {
//                if (item.Direction == ParameterDirection.Input)
//                {
//                    paramString += item.ParameterName + " , ";
//                }
//                if (item.Direction == ParameterDirection.Output)
//                {
//                    paramString += item.ParameterName + " OUTPUT, ";
//                    countOutParams++;
//                }

//            }

//            paramString = paramString.Remove(paramString.Length - 2);
//            var rowCount = _dbContext.Database
//              .ExecuteSqlRaw("[dbo].[" + SpSaveAirline.ToName() + "]" + paramString, parameters);

//            Dictionary<string, SqlParameter> outList = new Dictionary<string, SqlParameter>();

//            foreach (var item in parameters)
//            {
//                if (item.Direction == ParameterDirection.Output)
//                {
//                    outList.Add(item.ParameterName, item);
//                }

//            }

//            //var rowCount = _dbContext.Database
//            //  .ExecuteSqlRaw("[dbo].[" + SpSaveAirline.ToName() + "] @Id , @AirportName, @AirportCode,@City,@Country,@OutId OUTPUT ", parameters);

//            return Tuple.Create(rowCount, outList);
//        }

//        public Task<bool> ExecuteCommandOnDB(string queryString)
//        {

//            if (DbConnection.ConnectionType == EnumDBType.MSSQL)
//            {
//                using (var connection = new SqlConnection(DbConnection.MSSQL))
//                {
//                    using (var cmd = new SqlCommand(queryString, connection))
//                    {
//                        cmd.Connection = connection;
//                        connection.Open();

//                        using (var reader = cmd.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//#pragma warning disable S1751 // Loops with at most one iteration should be refactored
//                                return Task.FromResult(true);
//#pragma warning restore S1751 // Loops with at most one iteration should be refactored
//                            }
//                        }
//                        return Task.FromResult(true);

//                    }
//                }

//            }
//            return Task.FromResult(false);
//        }

//    }


//}
