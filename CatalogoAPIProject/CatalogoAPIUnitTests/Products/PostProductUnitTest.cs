using CatalogoAPI.Controllers;
using CatalogoAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPIUnitTests.Products;

public class PostProductUnitTest(ProductUnitTestController controller) : IClassFixture<ProductUnitTestController>
{
    private readonly ProductsController _controller = new(controller._repository, controller._mapper);

    [Fact]
    public async Task PostProduct_ReturnsCreatedStatusCode()
    {
        // Arrange
        var productDTO = new ProductDTO
        {
            Name = "Test Product",
            Description = "This is a test product",
            Price = 9.99m,
            ImageUrl = "https://example.com/test-product.jpg",
            CategoryId = 2
        };

        // Act
        var result = await _controller.Post(productDTO);

        // Assert (xunit)
        // var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        // Assert.Equal(201, createdResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<CreatedAtRouteResult>().Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduct_ReturnsBadRequestStatusCode()
    {
        // Arrange
        ProductDTO? productDTO = null;

        // Act
        var result = await _controller.Post(productDTO!);

        // Assert (xunit)
        // var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        // Assert.Equal(400, badRequestResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.StatusCode.Should().Be(400);
    }
}