using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.EntityModels.DatabaseConfig
{
    /// <summary>
    /// Responsible for parameterize application flow for Company.School.
    /// </summary>
    public sealed class DatabaseConnection : BaseTemplate//<ParameterMasterModel>
    {
        public string Catalog { get; set; }
        public string DbType { get; set; }
        public string DataSourceServer { get; set; }
    }
}
