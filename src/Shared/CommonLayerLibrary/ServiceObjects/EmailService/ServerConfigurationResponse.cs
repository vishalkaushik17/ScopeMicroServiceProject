namespace GenericFunction.ServiceObjects.EmailService;

//do not convert this class to static

[Serializable]
public class ServerConfigurationResponse
{
    public string? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public string? ServerName { get; set; } = "N/A";
    public string? IPAddress { get; set; } = "N/A";

}
