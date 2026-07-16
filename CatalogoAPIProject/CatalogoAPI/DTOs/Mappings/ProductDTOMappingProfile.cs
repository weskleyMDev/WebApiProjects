using AutoMapper;
using CatalogoAPI.Models;

namespace CatalogoAPI.DTOs.Mappings;

public class ProductDTOMappingProfile : Profile
{
    public ProductDTOMappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Product, ProductDTOUpdate>().ReverseMap();
        CreateMap<Product, ProductDTOResponse>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}