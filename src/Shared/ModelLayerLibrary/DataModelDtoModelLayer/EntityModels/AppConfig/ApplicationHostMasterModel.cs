using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.EntityModels.AppConfig;

public sealed class ApplicationHostMasterModel : BaseTemplate
{



    
    public string Domain { get; set; }
    public string ConnectionString { get; set; }
    public string UserName { get; set; }
    public string HashString { get; set; }
    public EnumDBType DatabaseType { get; set; }
    
    public void Save(string userId)
    {
        base.Id = Guid.NewGuid().ToString("D");
        CreatedOn = DateTime.Now;
        RecordStatus = EnumRecordStatus.Active;
        UserId = userId;
        IsEditable = false;
    }

    //public void StringToByte(string username, string password)
    //{
    //    UserNameByte = System.Text.Encoding.UTF8.GetBytes(username);
    //    PasswordByte = System.Text.Encoding.UTF8.GetBytes(password);
    //}
    public override void Delete(string userId)
    {
        UserId = userId;
        RecordStatus = EnumRecordStatus.Deleted;
        IsEditable = false;
    }

    public void Update(string userId)
    {
        UserId = userId;
        ModifiedOn = DateTime.Now;
        IsEditable = false;
    }
}