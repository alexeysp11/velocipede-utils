using System.ComponentModel.DataAnnotations;

namespace WorkflowLib.Shared.Models.Network.MicroserviceConfigurations
{
    /// <summary>
    /// Type of endpoint deployment.
    /// </summary>
    public enum EndpointDeploymentType
    {
        Monolith,
        HTTP,
        gRPC
    }
}