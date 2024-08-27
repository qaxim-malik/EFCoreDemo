using EFCoreDemo.Domain.Entities.Common;

namespace EFCoreDemo.Domain.Entities.SimpleEntities;

public class Teacher : BaseEntity
{
    public Guid Id { get; set; }
    public string TeacherName { get; set; } = null!;
    public virtual ICollection<Student>? Students { get; set; }
}