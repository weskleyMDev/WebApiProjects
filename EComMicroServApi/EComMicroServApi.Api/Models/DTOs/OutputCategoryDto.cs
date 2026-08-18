namespace EComMicroServApi.Api.Models.DTOs;

public class OutputCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<OutputProductDto> Products { get; set; } = [];
}