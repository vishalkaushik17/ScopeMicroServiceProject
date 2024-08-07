using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Core.Model;

public class EmailMaster : BaseTemplate//<EmployeeEntityTemplate>
{
    //public EmailMaster()
    //{

    //    Id = string.Empty;

    //}

    public EmailNotificationType EmailNotificationType { get; set; }

    public string ToEmail { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string? CCEmail { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string? ClientId { get; set; }

    public virtual void Save(string userId)
    {
        Id = Guid.NewGuid().ToString("D");
        CreatedOn = DateTime.Now;
        RecordStatus = EnumRecordStatus.Active;
        UserId = userId;
    }

    //public override void Update(string userId)
    //{
    //    UserId = userId;
    //    RecordStatus = EnumRecordStatus.Active;
    //}
    //public override void Delete(string userId)
    //{
    //    UserId = userId;
    //    RecordStatus = EnumRecordStatus.InActive;
    //}

}