using System.ComponentModel.DataAnnotations;

namespace StudentsApi.Api.Models;

public class RegisterModel
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 4)]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}