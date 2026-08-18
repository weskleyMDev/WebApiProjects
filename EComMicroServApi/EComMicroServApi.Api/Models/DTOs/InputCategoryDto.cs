using System.ComponentModel.DataAnnotations;

namespace EComMicroServApi.Api.Models.DTOs;

public class InputCategoryDto
{
    [Required]
    [StringLength(80, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
}