using EFCoreDemo.Domain.Entities.Common;

namespace EFCoreDemo.Domain.Entities.SimpleEntities;

public class Student : BaseEntity
{
    public int Id { get; set; }
    public string StudentName { get; set; } = null!;
    public Guid TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; } = null!;
}