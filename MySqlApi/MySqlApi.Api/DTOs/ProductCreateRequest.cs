using System.ComponentModel.DataAnnotations;

namespace MySqlApi.Api.DTOs;

public class ProductCreateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 99999999.99)]
    public decimal Price { get; set; }
}