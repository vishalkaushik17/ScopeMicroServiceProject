namespace GenericFunction.ResultObject;

public class TraceEntity
{

    public string? ApiAction { get; set; }
    public string? UserId { get; set; }
    public string? ClientMachine { get; set; }
    public string? ClientIp { get; set; }
    public string? SessionId { get; set; }
    public string? ScopeId { get; set; }
    public Clock? Timer { get; set; }
    public System.Diagnostics.Stopwatch? Stopwatch { get; set; }




}