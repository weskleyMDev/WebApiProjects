using System.ComponentModel.DataAnnotations;

namespace CatalogoAPI.DTOs;

public class RegisterDTO
{
    [Required(ErrorMessage = "The UserName is required.")]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "The Email is not valid.")]
    [Required(ErrorMessage = "The Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The Password is required.")]
    public string? Password { get; set; }
}