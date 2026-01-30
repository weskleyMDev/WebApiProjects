using System.ComponentModel.DataAnnotations;

namespace CatalogoAPI.DTOs;

public class ProductDTOUpdate : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "ProductId must be between {1} and {2}.")]
    public float Stock { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedAt.Date < DateTime.UtcNow.Date)
        {
            yield return new ValidationResult("CreatedAt must be a future date.", [nameof(CreatedAt)]);
        }
    }
}