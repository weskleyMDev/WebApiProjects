using CatalogoAPI.Controllers;
using CatalogoAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPIUnitTests.Products;

public class PutProductUnitTest(ProductUnitTestController controller) : IClassFixture<ProductUnitTestController>
{
    private readonly ProductsController _controller = new(controller._repository, controller._mapper);

    [Fact]
    public async Task PutProduct_ReturnsOkResult()
    {
        // Arrange
        var productId = 1; // Assuming a product with ID 1 exists in the test database
        var productDTO = new ProductDTO
        {
            ProductId = productId,
            Name = "Updated Test Product",
            Description = "This is an updated test product",
            Price = 19.99m,
            ImageUrl = "https://example.com/updated-test-product.jpg",
            CategoryId = 2
        };

        // Act
        var result = await _controller.Put(productId, productDTO);

        // Assert (xunit)
        // var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // Assert.Equal(200, okResult.StatusCode);

        // Assert (fluentassertions)
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>().Subject.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task PutProduct_ReturnsBadRequestResult()
    {
        // Arrange
        var productId = 1; // Assuming a product with ID 1 exists in the test database
        var productDTO = new ProductDTO
        {
            ProductId = productId,
            Name = "Updated Test Product",
            Description = "This is an updated test product",
            Price = 19.99m,
            ImageUrl = "https://example.com/updated-test-product.jpg",
            CategoryId = 2
        };

        // Act
        var result = await _controller.Put(productId + 1, productDTO); // Mismatched ID to trigger BadRequest

        // Assert (xunit)
        // var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        // Assert.Equal(400, badRequestResult.StatusCode);

        // Assert (fluentassertions)
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.StatusCode.Should().Be(400);
    }
}