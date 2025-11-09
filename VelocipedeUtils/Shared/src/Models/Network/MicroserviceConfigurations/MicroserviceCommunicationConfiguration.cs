namespace VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

/// <summary>
/// Microservice communication configuration.
/// </summary>
public class MicroserviceCommunicationConfiguration
{
    /// <summary>
    /// ID of microservice communication configuration.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Endpoint which is going to start the communication.
    /// </summary>
    public required Endpoint EndpointFrom { get; set; }

    /// <summary>
    /// Endpoint that is going to be called.
    /// </summary>
    public required Endpoint EndpointTo { get; set; }

    /// <summary>
    /// Record that stores information about the network interaction.
    /// </summary>
    public required NetworkInteractionDetails NetworkInteractionDetails { get; set; }
    
    /// <summary>
    /// Timout limit.
    /// </summary>
    public int? TimeoutLimit { get; set; }

    /// <summary>
    /// Limit of attempts to establish network communication.
    /// </summary>
    public int? AttemptsLimit { get; set; }
}
