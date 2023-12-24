using UnityEngine;
using TMPro;
using System.Net;
using System.Net.NetworkInformation;

public class GetIPFromTheDevice : MonoBehaviour
{
    public TMP_Text ipText;

    void Start()
    {
        ipText.text = GetLocalIPAddress();
    }

    string GetLocalIPAddress()
    {
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
            }
        }
        return "Unable to find local IP address";
    }
}
