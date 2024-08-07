namespace ModelTemplates.DtoModels.BaseDtoContract;

public class BaseDtoTemplate
{
    protected const string DefaultStringValue = "N/A";
    protected const int DefaultIntValue = 0;

    //[Required(ErrorMessage = "User identity is required!")]
    public virtual string? Id { get; set; } = string.Empty;


    //[Required(ErrorMessage = "Record creation date is required!")]
    //public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

    //public virtual DateTime ModifiedOn { get; set; } = DateTime.Now;

    public virtual string UserId { get; set; } = "Default";

    public bool IsEditable { get; set; } = false;


}