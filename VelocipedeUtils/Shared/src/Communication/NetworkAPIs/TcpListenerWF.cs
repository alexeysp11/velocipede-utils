using System;
using System.Net;
using System.Net.Sockets;

namespace VelocipedeUtils.NetworkAPIs;

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
    private TcpClient Client { get; set; } 
    
    /// <summary>
    /// 
    /// </summary>
    public IPAddress Ip { get; }

    /// <summary>
    /// Server name.
    /// </summary>
    public string ServerName { get; private set; }

    /// <summary>
    /// Port.
    /// </summary>
    private int Port { get; }
    
    /// <summary>
    /// 
    /// </summary>
    private byte[] ReceivedBytes;
    
    /// <summary>
    /// 
    /// </summary>
    private byte[] ResponseBytes;

    #region Constructors
    public TcpListenerWF()
    {
        this.Ip = IPAddress.Parse("127.0.0.1");
        this.ServerName = "localhost";
        this.Port = 13000;
        this.Listener = new TcpListener(this.Ip, this.Port);
    }

    /// <summary>
    /// 
    /// </summary>
    public TcpListenerWF(string ip, string serverName, int port)
    {
        this.Ip = IPAddress.Parse(ip);
        this.ServerName = serverName;
        this.Port = port;
        this.Listener = new TcpListener(this.Ip, this.Port);
    }
    #endregion  // Constructors

    /// <summary>
    /// 
    /// </summary>
    public void Listen()
    {
        try
        {
            this.Listener.Start();
            while(true)
            {
                GetMessage();
            }
        }
        finally
        {
            this.Client.Close();        // Shutdown and end connection. 
            this.Listener.Stop();       // Stop listening for new clients.
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetMessage()
    {
        ReceivedBytes = new byte[256];

        this.Client = this.Listener.AcceptTcpClient();
        NetworkStream stream = this.Client.GetStream();
            
        int msgLength = stream.Read(ReceivedBytes, 0, ReceivedBytes.Length);
            
        this.ProcessReceivedBytes(msgLength);
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