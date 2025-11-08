namespace Chat.Network.Client; 

/// <summary>
/// Common interface for all protocols using in this application.
/// </summary>
public interface IProtocolClientWF
{
    void SendMessage(string message);
    void CloseConnection();
}