namespace VelocipedeUtils.Ubp.Core.Dto;

public class EmployeeDto
{
    public string FullName { get; set; }

    public int AgeMin { get; set; }

    public int AgeMax { get; set; }

    public System.DateTime DateMin
    {
        get
        {
            return System.DateTime.Now.AddYears(-AgeMax);
        }
    }

    public System.DateTime DateMax
    {
        get
        {
            return System.DateTime.Now.AddYears(-AgeMin);
        }
    }

    public string Gender { get; set; }

    public string JobTitle { get; set; }

    public string Department { get; set; }
}