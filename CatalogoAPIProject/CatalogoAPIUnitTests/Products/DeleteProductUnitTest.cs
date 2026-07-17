using CatalogoAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPIUnitTests.Products;

public class DeleteProductUnitTest(ProductUnitTestController controller) : IClassFixture<ProductUnitTestController>
{
    private readonly ProductsController _controller = new(controller._repository, controller._mapper);

    [Fact]
    public async Task DeleteProduct_ReturnsOkResult()
    {
        // Arrange
        var productId = 2;

        // Act
        var result = await _controller.Delete(productId);

        // Assert (xunit)
        // var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // Assert.Equal(200, okResult.StatusCode);

        // Assert (fluentassertions)
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>().Subject.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFoundResult()
    {
        // Arrange
        var productId = 999; // Assuming a product with ID 999 does not exist in the test database

        // Act
        var result = await _controller.Delete(productId);

        // Assert (xunit)
        // var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        // Assert.Equal(404, notFoundResult.StatusCode);

        // Assert (fluentassertions)
        result.Result.Should().BeOfType<NotFoundObjectResult>().Subject.StatusCode.Should().Be(404);
    }
}