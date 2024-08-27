using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Dto;

public class ProductDto
{
    [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} length should be between 10 and 100")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    public string? Description { get; set; }
}
