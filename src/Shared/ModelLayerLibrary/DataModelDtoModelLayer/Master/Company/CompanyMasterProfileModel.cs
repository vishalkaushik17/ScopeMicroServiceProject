using GenericFunction;
using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Master.Company;

public class CompanyMasterProfileModel : BaseTemplate
{
    public string Name { get; set; } = string.Empty;
    public int NoOfStudents { get; set; } = 0;
    public int NoOfEmployees { get; set; } = 0;
    public int NoOfUsers { get; set; } = 0;
    public string DatabaseName { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    //public CompanyMasterModel CompanyMaster { get; set; }

    public new void Save(string userId)
    {
        UserId = userId;
        Name = Name.ToCamelCase();
        DatabaseName = DatabaseName.ToLower();
        HostName = HostName.ToLower();
        DatabaseName = DatabaseName.RemoveSymbols();
    }
    public void Save(string id, int noOfEmployees, int noOfStudents, int noOfUsers, string hostName,
        string name, string userId,string username, DateTime createdOn, bool isEditable, EnumRecordStatus recordStatus,
        string databaseName)
    {
        Id = id;
        NoOfEmployees = noOfEmployees;
        NoOfStudents = noOfStudents;
        NoOfUsers = noOfUsers;
        HostName = hostName;
        Name = name;
        UserId = userId;
        Username = username;
        CreatedOn = createdOn;
        IsEditable = isEditable;
        RecordStatus = recordStatus;
        DatabaseName = databaseName;
        Save(userId);
    }

    public void SetUser(string userId)
    {
        UserId = userId;
    }
}