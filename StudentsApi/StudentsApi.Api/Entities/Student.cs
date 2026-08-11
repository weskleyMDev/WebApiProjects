namespace StudentsApi.Api.Entities;
public class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Email { get; set; } = string.Empty;
}