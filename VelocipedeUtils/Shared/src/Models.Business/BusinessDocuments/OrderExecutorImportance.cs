namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Importance of the executor in the order.
/// </summary>
public enum OrderExecutorImportance
{
    None = 0,
    Responsible = 1,
    Signatory = 2,
    Executor = 3
}