namespace EComMicroServApi.Api.Models.DTOs;

public class OutputCategoryDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<OutputProductDto> Products { get; set; } = [];
}