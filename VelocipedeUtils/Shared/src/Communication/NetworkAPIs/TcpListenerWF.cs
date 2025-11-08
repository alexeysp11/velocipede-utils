using System.Net;
using System.Net.Sockets;

namespace VelocipedeUtils.Shared.Communication.NetworkAPIs;

/// <summary>
/// TCP listener.
/// </summary>
public class TcpListenerWF
{
    /// <summary>
    /// 
    /// </summary>
    private TcpListener Listener { get; } 
    
    /// <summary>
    /// 
    /// </summary>
    private TcpClient? Client { get; set; } 
    
    /// <summary>
    /// 
    /// </summary>
    public IPAddress? Ip { get; }

    /// <summary>
    /// Server name.
    /// </summary>
    public string? ServerName { get; private set; }

    /// <summary>
    /// Port.
    /// </summary>
    private int Port { get; }
    
    /// <summary>
    /// 
    /// </summary>
    private byte[]? ReceivedBytes;
    
    /// <summary>
    /// 
    /// </summary>
    private byte[]? ResponseBytes;

    #region Constructors
    public TcpListenerWF()
    {
        Ip = IPAddress.Parse("127.0.0.1");
        ServerName = "localhost";
        Port = 13000;
        Listener = new TcpListener(Ip, Port);
    }

    /// <summary>
    /// 
    /// </summary>
    public TcpListenerWF(string ip, string serverName, int port)
    {
        Ip = IPAddress.Parse(ip);
        ServerName = serverName;
        Port = port;
        Listener = new TcpListener(Ip, Port);
    }
    #endregion  // Constructors

    /// <summary>
    /// 
    /// </summary>
    public void Listen()
    {
        try
        {
            Listener.Start();
            while(true)
            {
                GetMessage();
            }
        }
        finally
        {
            Client?.Close();
            Listener.Stop();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetMessage()
    {
        ReceivedBytes = new byte[256];

        Client = Listener.AcceptTcpClient();
        NetworkStream stream = Client.GetStream();
            
        int msgLength = stream.Read(ReceivedBytes, 0, ReceivedBytes.Length);
            
        ProcessReceivedBytes(msgLength);
        ResponseBytes ??= [];
        stream.Write(ResponseBytes, 0, ResponseBytes.Length);

        ReceivedBytes = new byte[1];
        ResponseBytes = new byte[1];
    }

    /// <summary>
    /// 
    /// </summary>
    private void ProcessReceivedBytes(int msgLength)
    {
        throw new NotImplementedException();
    }
}