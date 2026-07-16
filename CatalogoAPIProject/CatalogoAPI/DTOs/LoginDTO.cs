using System.ComponentModel.DataAnnotations;

namespace CatalogoAPI.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "The UserName is required.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "The Password is required.")]
    public string? Password { get; set; }
}