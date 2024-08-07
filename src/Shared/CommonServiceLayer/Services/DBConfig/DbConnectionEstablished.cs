using DBOperationsLayer.Data.Constants;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Builder;

namespace SharedLibrary.Services.DBConfig
{
    /// <summary>
    /// This class is responsible to find the database type and establish the connection with it.
    /// </summary>
    public static class DbConnectionEstablished
    {
        /// <summary>
        /// Find and request to build the database connection as per the database type selected from appsettings.json
        /// </summary>
        /// <param name="builder">WebApplicationBuilder object</param>
        /// <returns>WebApplicationBuilder object</returns>
        public static WebApplicationBuilder EstablishDbConnection(this WebApplicationBuilder builder)
        {
            EnumDBType dbType = (EnumDBType)Enum.Parse(typeof(EnumDBType), builder.Configuration[DbConnection.DatabaseType], true);

            DatabaseConnection db = new DatabaseConnection();

            builder = db.EstablishConnection(builder, dbType);//, connectionString, connectionStringClient);
            return builder;
        }

    }
}
