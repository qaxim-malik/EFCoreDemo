using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Domain.Entities.Common;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [StringLength(450)]
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    [StringLength(450)]
    public string? ModifiedBy { get; set; }
}