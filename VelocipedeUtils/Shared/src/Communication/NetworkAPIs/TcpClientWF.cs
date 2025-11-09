namespace VelocipedeUtils.Shared.Communication.NetworkAPIs; 

/// <summary>
/// TCP client.
/// </summary>
public class TcpClientWF
{
    public string? Ip { get; }
    public string? ServerName { get; private set; }
}