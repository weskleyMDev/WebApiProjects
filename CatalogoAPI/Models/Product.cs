using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CatalogoAPI.Validations;

namespace CatalogoAPI.Models;

[Table("Products")]
public class Product // : IValidatableObject
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "The Name is required")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "The Name must have between 3 and 20 characters")]
    [FirstLetterUpper]
    public string? Name { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The Description must have a maximum of {1} characters")]
    public string? Description { get; set; }

    [Required]
    [Range(1, 999.99, ErrorMessage = "The Price must be between {1} and {2}")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "The ImageUrl must have between 5 and 50 characters")]
    public string? ImageUrl { get; set; }

    [Range(0.01, float.MaxValue, ErrorMessage = "The stock must be greater than zero.")]
    public float Stock { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    // {
    //     if (Stock <= 0)
    //     {
    //         yield return new ValidationResult(
    //             "The stock must be greater than zero.",
    //             new[] { nameof(Stock) }
    //         );
    //     }
    // }
}