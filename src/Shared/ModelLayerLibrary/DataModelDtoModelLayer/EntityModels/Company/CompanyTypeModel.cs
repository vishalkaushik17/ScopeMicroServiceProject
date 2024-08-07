
using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.EntityModels.Company
{
    /// <summary>
    /// Template represents what type of record is available in Company Master Table
    /// </summary>
    public sealed class CompanyTypeModel : BaseTemplate //<CompanyTypeEntityModel>
    {

        public CompanyTypeModel()
        {
            RecordStatus = EnumRecordStatus.Active;
            CreatedOn = DateTime.Now;
            IsEditable = false;


        }

        /// <summary>
        /// Type of the company. eg. Master, School, Trust, Agency etc.
        /// </summary>
        public string TypeName { get; set; }

        public CompanyTypeModel Save(string id, string userid, string typeName)
        {
            Id = id;
            UserId = userid;
            TypeName = typeName;
            IsEditable = false;
            return this;
        }
    }
}
