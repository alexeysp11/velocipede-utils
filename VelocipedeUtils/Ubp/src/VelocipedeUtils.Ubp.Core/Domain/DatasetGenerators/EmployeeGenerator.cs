using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;

/// <summary>
/// Randomly generates a set of employees
/// </summary>
public class EmployeeGenerator : IEmployeeGenerator
{
    private AppSettings _appSettings;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public EmployeeGenerator(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    /// <summary>
    /// Generates specified number of employees 
    /// </summary>
    public List<Employee> GenerateEmployees(
        int count, 
        System.Func<System.DateTime, System.DateTime, System.DateTime> generateDate)
    {
        var employees = new List<Employee>();
        for (int i = 0; i < count; i++)
        {
            var employee = new Employee 
            {
                FullName = GenerateFullName(),
                Gender = GenerateEnum<GenderType>(),
                // JobTitle = GenerateEnum<JobTitle>(),
                // Department = GenerateEnum<Department>(),
                BirthDate = GenerateBirthDate(generateDate)
            };
            employees.Add(employee);
        }
        return employees;
    }
    
    /// <summary>
    /// Generate a fullname of the employee 
    /// </summary>
    private string GenerateFullName()
    {
        var random = new Random();
        var finalString = string.Empty;
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[_appSettings.EmployeeFullNameLength];
        for (int i = 0; i < _appSettings.EmployeeFullNameWordsNumber; i++)
        {
            for (int j = 0; j < stringChars.Length; j++)
                stringChars[j] = chars[random.Next(chars.Length)];
            var tmpStr = new String(stringChars);
            finalString += tmpStr.First().ToString().ToUpper() + tmpStr.Substring(1).ToLower() + " ";
        }
        return finalString.Last() == ' ' ? finalString.Remove(finalString.Length - 1) : finalString;
    }

    /// <summary>
    /// Generic method for generating the employee's properties, knowing specified typed of enum 
    /// </summary>
    private T GenerateEnum<T>(int startIndex = 0, int shift = 0) where T : System.Enum
    {
        var length = System.Enum.GetNames(typeof(T)).Length;
        return (T)(object) new Random().Next(startIndex, length + shift);
    }

    /// <summary>
    /// Generates a bith date of an employee 
    /// </summary>
    private System.DateTime GenerateBirthDate(System.Func<System.DateTime, System.DateTime, System.DateTime> generateDate)
    {
        System.DateTime start = System.DateTime.Now.AddYears(-_appSettings.EmployeeMaxAge);
        System.DateTime end = System.DateTime.Now.AddYears(-_appSettings.EmployeeMinAge);
        return generateDate(start, end);
    }
}