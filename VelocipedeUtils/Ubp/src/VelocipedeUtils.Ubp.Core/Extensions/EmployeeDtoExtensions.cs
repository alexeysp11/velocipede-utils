using VelocipedeUtils.Ubp.Core.Dto;

namespace VelocipedeUtils.Ubp.Core.Extensions;

public static class EmployeeDtoExtensions
{
    /// <summary>
    /// Helper method for EmployeeDto to check if all filters are empty.
    /// </summary>
    public static bool IsEmpty(this EmployeeDto employeeDto)
    {
        return string.IsNullOrEmpty(employeeDto.FullName)
            && employeeDto.AgeMin <= 0
            && employeeDto.AgeMax <= 0
            && string.IsNullOrEmpty(employeeDto.Gender)
            && string.IsNullOrEmpty(employeeDto.JobTitle)
            && string.IsNullOrEmpty(employeeDto.Department);
    }
}