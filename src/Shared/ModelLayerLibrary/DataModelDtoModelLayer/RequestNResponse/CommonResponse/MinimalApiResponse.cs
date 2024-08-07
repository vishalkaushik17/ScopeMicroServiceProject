using GenericFunction;
using System.Globalization;
using System.Text;

namespace ModelTemplates.RequestNResponse.CommonResponse
{
    [Serializable]
    public class MinimalApiResponse
    {
        public bool Status { get; set; } = true;
        public string? AssemblyName { get; set; } = string.Empty;
        public Version? AssemblyVersion { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;
        public double Amount { get; set; } = 123456.44;
        public string IpAddress { get; set; } = Utility.GetLocalIpAddress();
        public string HostName { get; set; } = Utility.GetHost();
        public override string ToString()
        {
            DateTime = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            var formattedAmount =String.Format(CultureInfo, "{0:C}", Amount);
            sb.Append($"Status Success {Status}!\nAssembly Name : {AssemblyName}, Ver : {AssemblyVersion}, " +
                      $"\nCurrent Date Time : {DateTime}\nCulture Language : " +
                      $"{CultureInfo.EnglishName}\nCurrency Format :{formattedAmount}\nHost Ip Address : {IpAddress} \nHost Name : {HostName}");
            
            return sb.ToString();
        }
    }
}
