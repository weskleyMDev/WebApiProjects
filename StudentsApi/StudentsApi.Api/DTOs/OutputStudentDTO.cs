namespace StudentsApi.Api.DTOs;

public class OutputStudentDto
{
    public int StudentId { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
}