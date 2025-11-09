namespace VelocipedeUtils.Shared.Models.ErrorHandling;

/// <summary>
/// Exception that is occurred during execution of the API operation.
/// </summary>
public class WorkflowException
{
    /// <summary>
    /// Message of the exception.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Stack trace of the exception.
    /// </summary>
    public required string StackTrace { get; set; }
    
    /// <summary>
    /// Full message of the exception.
    /// </summary>
    public required string FullMessage { get; set; }
}
