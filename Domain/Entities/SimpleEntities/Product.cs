using EFCoreDemo.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreDemo.Domain.Entities.SimpleEntities;

[Table("products", Schema = "demo")]
public class Product : BaseEntity
{
    public int Id { get; set; }

    [MaxLength(100)]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} length should be between 10 and 100")]
    [Column("Title")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Column(Order = 2)]
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }
}
