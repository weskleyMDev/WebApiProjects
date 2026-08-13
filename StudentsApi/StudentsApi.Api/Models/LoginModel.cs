using System.ComponentModel.DataAnnotations;

namespace StudentsApi.Api.Models;

public class LoginModel
{
    [Required]
    [StringLength(80, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}