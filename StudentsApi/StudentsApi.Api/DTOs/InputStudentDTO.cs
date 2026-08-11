using System.ComponentModel.DataAnnotations;

namespace StudentsApi.Api.DTOs;

public class InputStudentDto
{
    [Required(ErrorMessage = "Name is required!")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string? Name { get; set; }

    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120 years!")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Invalid email format!")]
    public string? Email { get; set; }
}