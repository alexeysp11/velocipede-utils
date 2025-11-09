using System.Linq.Expressions;
using VelocipedeUtils.Shared.Models.Network.MicroserviceConfigurations;

namespace VelocipedeUtils.Shared.ServiceDiscoveryBpm.DAL;

/// <summary>
/// An interface at the DAL level that performs endpoint-related operations in the database.
/// </summary>
public interface IEndpointDAL
{
    /// <summary>
    /// Method for obtaining endpoint data for a given predicate.
    /// </summary>
    Endpoint GetEndpoint(Expression<Func<EndpointCall, bool>> predicate);
}