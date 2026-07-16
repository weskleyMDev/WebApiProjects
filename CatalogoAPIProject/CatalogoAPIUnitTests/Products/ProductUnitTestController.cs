using AutoMapper;
using CatalogoAPI.Context;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Repositories;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatalogoAPIUnitTests.Products;

public class ProductUnitTestController
{
    public IUnitOfWork _repository;
    public IMapper _mapper;

    public static DbContextOptions<AppDbContext> DbContextOptions { get; }

    public static string ConnectionString { get; } = "";

    static ProductUnitTestController()
    {
        DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
            .Options;
    }

    public ProductUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductDTOMappingProfile>();
        }, LoggerFactory.Create(builder => { }));
        _mapper = config.CreateMapper();
        var context = new AppDbContext(DbContextOptions);
        _repository = new UnitOfWork(context);
    }
}