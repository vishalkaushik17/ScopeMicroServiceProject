using GenericFunction;
using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// MediaModel is responsible to handle Media Type base data which is super class 
/// for MediaComponent.
/// It is derived by Generic Template and IMediaContract.
/// Template comprises of few completed and incomplete methods.
/// </summary>
public class LibraryMediaModel : BaseTemplate//, IMediaContract
{
    /// <summary>
    /// Name of the media.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
    }
}
