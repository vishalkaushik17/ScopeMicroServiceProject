using GenericFunction.Enums;

namespace ModelTemplates.Core.GenericModel;

/// <summary>
/// Generic Template Abstract class is responsible to follows strict rules on 
/// Identity Model Templates.
/// </summary>


public abstract class BaseTemplate// : IGenericContract
{
    protected string DefaultStringValue = "N/A";
    /// <summary>
    /// To instantiate GenericTemplate and perform certain operations before accessing the Template.
    /// </summary>
    protected BaseTemplate()
    {

        //Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString("D") : Id;
        //Id = Guid.NewGuid().ToString("D"); //need to change from db
        CreatedOn = DateTime.Now;
        RecordStatus = EnumRecordStatus.Active;
        IsEditable = false;
        UserId = "Default";


    }
    /// <summary>
    /// Record Identity.  Size: 450 Chars
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Record RecordStatus True/False.
    /// </summary>
    public EnumRecordStatus RecordStatus { get; set; }

    /// <summary>
    /// Date of record first time inserted in table.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Date of record modified.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }


    /// <summary>
    /// Which user is responsible for adding/modifying the record.
    /// </summary>
    public string UserId { get; set; }

    ///// <summary>
    ///// Record belongs to which school.
    ///// </summary>
    //public string SchoolId { get; set; }


    /// <summary>
    /// Record is permitted for Modification / Delectation.
    /// </summary>
    public bool IsEditable { get; set; } = false;

    public void Save(string userId)
    {
        Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString("D") : Id;
        CreatedOn = string.IsNullOrWhiteSpace(Id) ? DateTime.Now : CreatedOn;
        ModifiedOn = DateTime.Now;
        RecordStatus = EnumRecordStatus.Active;
        UserId = userId;
        IsEditable = false;

    }
    public virtual void Delete(string userId)
    {
        UserId = userId;
        RecordStatus = EnumRecordStatus.Deleted;
        IsEditable = false;
    }
    public virtual void Update()
    {
        ModifiedOn = DateTime.Now;
        IsEditable = false;
    }

    //work on this.
    public virtual void Update(string userId)
    {
        //Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString("D") : Id;
        ModifiedOn = DateTime.Now;
        UserId = UserId;
    }

    public virtual void CheckIsEditable(int _modificationInDays)
    {
        IsEditable = CreatedOn <= DateTime.Now && CreatedOn >= DateTime.Now.AddDays(-_modificationInDays);
    }
}
