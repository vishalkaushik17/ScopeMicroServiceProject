using GenericFunction;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.DtoModels.SchoolLibrary;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// MediaTypeTemplate is responsible to handle Media Type base data which is super class for MediaTypeComponent.
/// It is derived by Generic Template and IMediaTypeContract.
/// Template comprises of few completed and incomplete methods.
/// </summary>
public class LibraryMediaTypeModel : BaseTemplate//, IMediaTypeContract
{
    /// <summary>
    /// Name of the media format/type.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
    }
}
