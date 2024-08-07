using GenericFunction.Enums;

namespace GenericFunction.DefaultSettings
{
    /// <summary>
    /// it will help to connect database as per client suffix domain. List of DbHosting records save in redis and on login app will decide on which database client has to connect.
    /// </summary>
    public class DbConnectionStringRecord
    {
        public string SuffixName { get; set; } = string.Empty;
        public string DbConnectionString { get; set; } = string.Empty;
        public EnumDBType DbType{ get; set; }
        public string ClientId { get; set; } = string.Empty;
        public bool IsExpired { get; set; }
    }  
}
