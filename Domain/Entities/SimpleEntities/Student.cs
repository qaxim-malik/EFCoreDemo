using EFCoreDemo.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreDemo.Domain.Entities.SimpleEntities;

public class Student : BaseEntity
{
    //[Key]
    public int Id { get; set; }

    [Column("Student_Name", TypeName = "nvarchar(50)")]
    public string StudentName { get; set; } = null!;

    public string? Email { get; set; }

    //[ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}