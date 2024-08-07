namespace DBOperationsLayer.Data.Context;

/// <summary>
/// for postgres sql this is needed.
/// </summary>
public class IsUtcAttribute : Attribute
{
    public IsUtcAttribute(bool isUtc = true) => this.IsUtc = isUtc;
    public bool IsUtc { get; }
}


