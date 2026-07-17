using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Application.DTOs;

public class ProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MinLength(5)]
    [MaxLength(255)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [MinLength(5)]
    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(1, 9999)]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Date is required")]    
    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }
}