namespace GenericFunction.ResultObject;


[Serializable]
public class ResponseReturn
{
    public string Id { get; set; } = string.Empty;
    public string HostId { get; set; }  = string.Empty;
    public string CompanyMasterId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime ExpiredDate { get; set; }
    public string DatabaseName { get; set; } = string.Empty;
}
