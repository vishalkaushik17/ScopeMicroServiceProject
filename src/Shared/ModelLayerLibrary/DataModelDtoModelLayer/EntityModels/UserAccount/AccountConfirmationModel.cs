using GenericFunction.Enums;
using GenericFunction.ResultObject;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.EntityModels.UserAccount
{
    public class AccountConfirmationModel : BaseTemplate
    {
        public string HostId { get; set; }
        public string UserId { get;  set; }
        public string UserName { get;  set; }
        public DateTime ExpiredDate { get; set; } = DateTime.Now.AddDays(1);
        public bool IsConfirmed { get; set; } = false;
        public DateTime? ConfirmOnDate { get; set; }

        public void Save(string userId, string userName, string hostId)
        {
            HostId = hostId;
            UserName = userName;
            UserId = userId;
        }

        public void ConfirmUpdate()
        {
            IsConfirmed = true;
            ConfirmOnDate = DateTime.Now;
            RecordStatus = EnumRecordStatus.Expired;
            ModifiedOn = DateTime.Now;
        }

        public string GenerateConfirmLink()
        {
            string link = $"{HostId}|{UserId}|{UserName}|{ExpiredDate}";
            return link;
        }
        public ResponseReturn GenerateConfirmJsonObject(string companyMasterId, string databaseName, string hostId)
        {
            ResponseReturn response = new ResponseReturn();
            response.Id = this.Id;
            response.HostId = hostId;

            response.UserName = UserName;
            response.CompanyMasterId = companyMasterId;
            response.ExpiredDate = ExpiredDate.AddDays(1);
            response.DatabaseName = databaseName;

            return response;
        }
    }
}
