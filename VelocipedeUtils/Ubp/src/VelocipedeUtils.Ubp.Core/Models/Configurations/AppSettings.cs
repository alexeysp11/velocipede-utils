using System.Collections.Generic;

namespace VelocipedeUtils.Ubp.Core.Models.Configurations;

/// <summary>
/// Class for storing commonly used config values.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Connection string.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Interval for deleting filtered datasets.
    /// </summary>
    public int DbSetCollectorInterval { get; set; }

    /// <summary>
    /// Number of generated employees.
    /// </summary>
    public int EmployeeQty { get; set; }
    
    /// <summary>
    /// Specifed pattern of vacation intervals.
    /// </summary>
    public List<int> VacationIntervals { get; set; }
    
    /// <summary>
    /// Number of elements generated inside the collection of vacations.
    /// </summary>
    public int VacationQty
    {
        get 
        {
            return EmployeeQty * VacationIntervals.Count;
        }
    } 

    /// <summary>
    /// Maximum age of a generated employee.
    /// </summary>
    public int EmployeeMaxAge { get; set; }
    
    /// <summary>
    /// Manimum age of a generated employee.
    /// </summary>
    public int EmployeeMinAge { get; set; }

    /// <summary>
    /// Length of a part of a employee's fullname.
    /// </summary>
    public int EmployeeFullNameLength { get; set; }
    
    /// <summary>
    /// Number of parts that the fullname consists of.
    /// </summary>
    public int EmployeeFullNameWordsNumber { get; set; }
}