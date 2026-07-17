using Catalogo.Domain.Validation;

namespace Catalogo.Domain.Entities;

public sealed class Product : Entity
{
    public Product(
        string name,
        string description,
        decimal price,
        string imageUrl,
        int stock,
        DateTime createdAt
    )
    {
        ValidateDomain(name, description, price, imageUrl, stock, createdAt);
    }

    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public void Update(
        string name,
        string description,
        decimal price,
        string imageUrl,
        int stock,
        DateTime createdAt,
        int categoryId
    )
    {
        ValidateDomain(name, description, price, imageUrl, stock, createdAt);
        CategoryId = categoryId;
    }

    private void ValidateDomain(string name, string description, decimal price, string imageUrl, int stock, DateTime createdAt)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(name), "Invalid name. Name is required.");

        DomainExceptionValidation.When(name.Length < 3, "Invalid name. Too short, minimum 3 characters.");

        DomainExceptionValidation.When(string.IsNullOrEmpty(description), "Invalid description. Description is required.");

        DomainExceptionValidation.When(description.Length < 5, "Invalid description. Too short, minimum 5 characters.");

        DomainExceptionValidation.When(price < 0, "Invalid price value.");

        DomainExceptionValidation.When(string.IsNullOrEmpty(imageUrl), "Invalid image. Image is required.");

        DomainExceptionValidation.When(imageUrl.Length < 5, "Invalid image. Too short, minimum 5 characters.");

        DomainExceptionValidation.When(stock < 0, "Invalid stock value.");

        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        Stock = stock;
        CreatedAt = createdAt;
    }
}