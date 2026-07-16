using System.ComponentModel.DataAnnotations;

namespace CatalogoAPI.Validations;

public class FirstLetterUpperAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success!;
        }

        var firstLetter = value.ToString()![0];
        if (!char.IsUpper(firstLetter))
        {
            return new ValidationResult("The first letter must be uppercase.");
        }

        return ValidationResult.Success!;
    }
}