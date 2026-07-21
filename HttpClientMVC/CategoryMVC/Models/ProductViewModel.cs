using System.ComponentModel.DataAnnotations;

namespace CategoryMVC.Models;

public class ProductViewModel
{
    public int ProductId { get; set; }
    
    [Required(ErrorMessage = "Name is required!")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Description is required!")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required!")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "ImageUrl is required!")]
    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}