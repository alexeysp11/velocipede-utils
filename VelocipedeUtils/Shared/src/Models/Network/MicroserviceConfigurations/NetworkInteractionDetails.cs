namespace VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

/// <summary>
/// Record that stores details about the network interaction.
/// </summary>
public class NetworkInteractionDetails
{
    /// <summary>
    /// ID of the network interaction details.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Type of the network interaction details.
    /// </summary>
    public NetworkInteractionDetailsType Type { get; set; }

    /// <summary>
    /// API method name.
    /// </summary>
    public string? ApiMethodName { get; set; }

    /// <summary>
    /// Type of the message broker.
    /// </summary>
    public MessageBrokerType? MessageBrokerType { get; set; }

    /// <summary>
    /// Queue name in the message broker.
    /// </summary>
    public string? QueueName { get; set; }
}