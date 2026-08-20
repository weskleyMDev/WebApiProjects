using EComMicroServApi.Api.Models.DTOs;
using Mapster;

namespace EComMicroServApi.Api.Models.Mappings;

public class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InputCategoryDto, Category>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Products);

        config.NewConfig<Category, OutputCategoryDto>();
    }
}