using GenericFunction.Enums;

namespace ModelTemplates.Core.GenericModel;

/// <summary>
/// Generic Contract to restrict Entity template to follow certain rules.
/// </summary>

public interface IGenericContract//<T> where T : class
{

    /// <summary>
    /// Record Identity.  Size: 450 Chars
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Record RecordStatus True/False.
    /// </summary>
    EnumRecordStatus RecordStatus { get; protected set; }

    /// <summary>
    /// Date of record first time inserted in table.
    /// </summary>
    DateTime CreatedOn { get; protected set; }

    /// <summary>
    /// Date of record modified.
    /// </summary>
    DateTime? ModifiedOn { get; protected set; }

    ///// <summary>
    ///// Record Capsule No which can identify that record is exported in which capsule no.
    ///// </summary>
    //string CapsuleNo { get; set; }
    //bool IsCapsuleExported { get; set; }
    //string CapsuleBatchNo { get; set; }

    /// <summary>
    /// Which user is responsible for adding/modifying the record.
    /// </summary>
    public string UserId { get; protected set; }

    /// <summary>
    /// Record is permitted for Modification / Delectation.
    /// </summary>
    public bool IsEditable { get; set; }

    /// <summary>
    /// Perform certain checks before saving record to table.
    /// </summary>
    /// <param name="userId">Currently logged in UserId.  Captured from JWT Token.</param>
    void Save(string userId);

    /// <summary>
    /// Perform check before deleting record from table. Method is performing Soft Delete Action.
    /// </summary>
    /// <param name="userId">Currently logged in UserId.  Captured from JWT Token.</param>
    void Delete(string userId);

    /// <summary>
    /// Perform checks before modifying existing record on table.
    /// </summary>
    /// <param name="userId">Currently logged in UserId.  Captured from JWT Token.</param>
    void Update(string userId);
}