using CatalogoAPI.Controllers;
using CatalogoAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPIUnitTests.Products;

public class GetProductUnitTest(ProductUnitTestController controller) : IClassFixture<ProductUnitTestController>
{
    private readonly ProductsController _controller = new(controller._repository, controller._mapper);

    [Fact]
    public async Task GetProduct_ReturnsOkResult()
    {
        // Arrange
        int productId = 1; // Assuming this product ID exists in the database

        // Act
        var result = await _controller.GetProduct(productId);

        // Assert (xunit)
        // var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // Assert.Equal(200, okResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFoundResult()
    {
        // Arrange
        int productId = 999; // Assuming this product ID does not exist

        // Act
        var result = await _controller.GetProduct(productId);

        // Assert (xunit)
        // var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        // Assert.Equal(404, notFoundResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProduct_ReturnsBadRequestResult()
    {
        // Arrange
        int productId = -1; // Invalid product ID

        // Act
        var result = await _controller.GetProduct(productId);

        // Assert (xunit)
        // var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        // Assert.Equal(400, badRequestResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult()
    {
        // Act
        var result = await _controller.GetProducts();

        // Assert (xunit)
        // var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // Assert.Equal(200, okResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProductDTO>>().And.NotBeNull();
    }

    /* [Fact]
    public async Task GetProducts_ReturnsBadRequestResult()
    {
        // Act
        var result = await _controller.GetProducts();

        // Assert (xunit)
        // var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        // Assert.Equal(400, badRequestResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    } */
}