using EComMicroServApi.Api.Models.DTOs;
using Mapster;

namespace EComMicroServApi.Api.Models.Mappings;

public class ProductMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, OutputProductDto>()
            .Map(
                dest => dest.CategoryName,
                src => src.Category.Name
            );

        config.NewConfig<InputProductDto, Product>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Category);
    }
}