using VelocipedeUtils.Shared.Models.ErrorHandling;
using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.Models.Network;

/// <summary>
/// Represents any attempt to communcate with API server as an operation.
/// </summary>
public class ApiOperation
{
    /// <summary>
    /// Client application UID.
    /// </summary>
    public required string ClientAppUid { get; set; }

    /// <summary>
    /// Server application UID.
    /// </summary>
    public required string ServerAppUid { get; set; }

    /// <summary>
    /// Application UID, to which the request will be redirected.
    /// </summary>
    public required string RedirectToAppUid { get; set; }
    
    /// <summary>
    /// Name of the method that should be executed to perform the operation.
    /// </summary>
    public required string MethodName { get; set; }
    
    /// <summary>
    /// Description of the method that should be executed to perform the operation 
    /// (contextual name of the operation, e.g. "adding two values").
    /// </summary>
    public required string MethodDescription { get; set; }

    /// <summary>
    /// String representation of JSON object that is sent as a request to the server.
    /// </summary>
    public required string Request { get; set; }

    /// <summary>
    /// String representation of JSON object that is received as a response from the server.
    /// </summary>
    public required string Response { get; set; }

    /// <summary>
    /// Object that is sent as a request to the server.
    /// </summary>
    public required object RequestObject { get; set; }

    /// <summary>
    /// Object that is received as a response from the server.
    /// </summary>
    public required object ResponseObject { get; set; }

    /// <summary>
    /// Boolean variable that is set by a server to indicate if the request is executed properly.
    /// </summary>
    public bool IsExecuted { get; set; }
    
    /// <summary>
    /// String representation of the status of the operation (e.g. executed, failed, pending).
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Additional information about the status of the operation.
    /// </summary>
    public required string StatusDescription { get; set; }

    /// <summary>
    /// Start time of the operation.
    /// </summary>
    public DateTime DateTimeBegin { get; set; }

    /// <summary>
    /// End time of the operation.
    /// </summary>
    public DateTime DateTimeEnd { get; set; }

    /// <summary>
    /// Configuration of interaction between microservices.
    /// </summary>
    public required MicroserviceCommunicationConfiguration MicroserviceCommunicationConfiguration { get; set; }

    /// <summary>
    /// Exception that is occurred during execution of the API operation.
    /// </summary>
    public WorkflowException? WorkflowException { get; set; }
}