namespace VelocipedeUtils.Shared.Models.Business.Reporting;

/// <summary>
/// Report status.
/// </summary>
public enum WfReportStatus
{
    None = 0,
    New = 1,
    Generating = 2,
    Ready = 3,
    Printed = 4,
    Failed = 5
}