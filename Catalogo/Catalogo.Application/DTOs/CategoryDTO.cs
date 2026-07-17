using System.ComponentModel.DataAnnotations;

namespace Catalogo.Application.DTOs;

public class CategoryDTO
{
    public int Id {get; set;}

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name {get; set;}

    [Required(ErrorMessage = "Image is required")]
    [MinLength(5)]
    [MaxLength(255)]
    public string? ImageUrl {get; set;}
}