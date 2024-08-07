namespace GenericFunction;

public static class HostDBRecords
{
    static HostDBRecords()
    {
        CompanyVsDBHosts = new Dictionary<string, string>();
    }
    public static Dictionary<string, string> CompanyVsDBHosts { get; set; }
    public static void AddDNS(string key, string value)
    {
        try
        {
            var isRecord = CompanyVsDBHosts.FirstOrDefault(m => m.Key == key);
            if (isRecord.Key == null)
            {
                CompanyVsDBHosts.Add(key, value);
            }

        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            //do nothing
        }

    }
}
