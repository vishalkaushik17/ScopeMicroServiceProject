using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GenericFunction;

public static class Utility
{
    public static string? GetAddressIP() => Dns.GetHostAddresses(Dns.GetHostName())
            .FirstOrDefault(ha => ha.AddressFamily == AddressFamily.InterNetwork)
            ?.ToString();
    public static string? GetHost()
    {
        return Dns.GetHostName();
    }
    public static string? GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return null;
    }
    public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
    {
        List<string> ipAddrList = new List<string>();
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddrList.Add(ip.Address.ToString());
                    }
                }
            }
        }
        return ipAddrList.ToArray();
    }
}
