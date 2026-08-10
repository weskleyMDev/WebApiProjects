using System.ComponentModel.DataAnnotations;

namespace AlunosAPI.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    public int Age { get; set; }

    [Required]
    [StringLength(100)]
    public string? Email { get; set; }
}