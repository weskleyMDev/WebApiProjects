
using Catalogo.Domain.Validation;

namespace Catalogo.Domain.Entities;

public sealed class Category : Entity
{
    public Category(string name, string imageUrl)
    {
        ValidateDomain(name, imageUrl);
    }

    public Category(int id, string name, string imageUrl)
    {
        DomainExceptionValidation.When(id < 0, "Invalid Id value.");
        Id = id;
        ValidateDomain(name, imageUrl);
    }

    public string? Name { get; private set; }
    public string? ImageUrl { get; private set; }
    public ICollection<Product>? Products { get; set; }

    private void ValidateDomain(string name, string imageUrl)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(name), "Invalid name. Name is required.");

        DomainExceptionValidation.When(name.Length < 3, "Invalid name. Too short, minimum 3 characters.");

        DomainExceptionValidation.When(string.IsNullOrEmpty(imageUrl), "Invalid image. Image is required.");

        DomainExceptionValidation.When(imageUrl.Length < 5, "Invalid image. Too short, minimum 5 characters.");

        Name = name;
        ImageUrl = imageUrl;
    }
}