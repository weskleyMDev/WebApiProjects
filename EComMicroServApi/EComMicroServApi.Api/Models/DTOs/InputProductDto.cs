using System.ComponentModel.DataAnnotations;

namespace EComMicroServApi.Api.Models.DTOs;

public class InputProductDto
{
    [Required]
    [StringLength(80, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 99999)]
    public int Stock { get; set; }

    [Required]
    [Url]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}