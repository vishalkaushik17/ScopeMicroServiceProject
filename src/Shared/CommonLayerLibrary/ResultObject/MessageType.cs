namespace GenericFunction.ResultObject;

public static class MessageType
{
    public static string ExceptionCache { get; set; } = "Cache Exception";
    public static string Exception { get; set; } = "Exception";
    public static string Message { get; set; } = "Message";
    public static string Information { get; set; } = "Information";
    public static string Warning { get; set; } = "Warning";
    public static string ModelState { get; set; } = "ModelState";
    public static string DefaultRecordWarning { get; set; } = "DefaultRecordWarning";
}
