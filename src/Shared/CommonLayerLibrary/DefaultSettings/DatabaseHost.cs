using GenericFunction.Enums;
using System.Data;

namespace GenericFunction.DefaultSettings
{
    /// <summary>
    /// Database host server settings for default connection.
    /// </summary>
    public class DatabaseHost
    {
        public string Domain { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public EnumDBType DbType { get; set; }
    }
}
