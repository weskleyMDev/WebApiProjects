using System.ComponentModel.DataAnnotations;

namespace CategoryMVC.Models;

public class CategoryViewModel
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Name is required!")]
    public string? Name { get; set; }

    [Required]
    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }
}