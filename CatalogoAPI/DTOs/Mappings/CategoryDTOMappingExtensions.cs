using CatalogoAPI.Models;

namespace CatalogoAPI.DTOs.Mappings;

public static class CategoryDTOMappingExtensions
{
    public static CategoryDTO? ToDTO(this Category category)
    {
        if (category is null) return null;
        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        };
    }

    public static Category? ToEntity(this CategoryDTO categoryDTO)
    {
        if (categoryDTO is null) return null;
        return new Category
        {
            CategoryId = categoryDTO.CategoryId,
            Name = categoryDTO.Name,
            ImageUrl = categoryDTO.ImageUrl
        };
    }

    public static IEnumerable<CategoryDTO> ToDTOs(this IEnumerable<Category> categories)
    {
        if (categories is null || !categories.Any())
        {
            return [];
        }
        return [.. categories.Select(c => c.ToDTO()).OfType<CategoryDTO>()];
    }
}