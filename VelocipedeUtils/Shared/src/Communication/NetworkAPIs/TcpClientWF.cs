using System.Net.Sockets;

namespace VelocipedeUtils.NetworkAPIs; 

/// <summary>
/// TCP client.
/// </summary>
public class TcpClientWF
{
    #region Configuration properties
    private TcpClient Client { get; set; } = null;
    private NetworkStream _NetworkStream { get; set; } = null;

    public string Ip { get; }
    public string ServerName { get; private set; }
    private int Port { get; }
    #endregion  // Configuration properties
}