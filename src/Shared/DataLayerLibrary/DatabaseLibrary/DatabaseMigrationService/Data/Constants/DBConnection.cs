using GenericFunction.Enums;

namespace DBOperationsLayer.Data.Constants
{
    public static class DbConnection
    {
        /// <summary>
        /// List of Sections in appsettings.json
        /// </summary>

        public const string DatabaseType = "Database:Type";
        public const string MSSQL = "ConnectionStrings:MSSQL";
        public const string MYSQL = "ConnectionStrings:MYSQL";
        public const string ORACLE = "ConnectionStrings:ORACLE";
        public const string PGSQL = "ConnectionStrings:PGSQL";
        public const string PGSQLDOCKER = "ConnectionStrings:PGSQLDOCKER";
        //public const string MssqlConnectionClient = "ConnectionStrings:MySqlConnectionStringClient";

        //public const string MySqlConnection = "ConnectionStrings:MySQLConnectionString";
        //public const string MySqlConnectionClient = "ConnectionStrings:MySqlConnectionStringClient";

        //public const string OracleConnection = "ConnectionStrings:OracleConnection";
        //public const string OracleConnectionClient = "ConnectionStrings:OracleConnectionClient";
        public const string DbMigrationKey = "DbMigration:Allow";

        public static EnumDBType ConnectionType { get; set; }
        public static string ConnectionString { get; set; } = string.Empty;
        public static string ConnectionStringClient { get; set; } = string.Empty;
        public static string DbMigration { get; set; } = string.Empty;

    }

}
