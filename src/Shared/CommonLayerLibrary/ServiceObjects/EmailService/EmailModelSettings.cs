namespace GenericFunction.ServiceObjects.EmailService;

public class EmailModelSettings
#pragma warning restore RCS1102 // Make class static.
{
    public string? Mail { get; set; }
    public string? DisplayName { get; set; }
    public string? Password { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
}