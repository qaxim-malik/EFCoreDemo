using EFCoreDemo.Domain.Entities.Common;

namespace EFCoreDemo.Domain.Entities.AdvanceEntities;

public class Book : BaseEntity, ICreationAudited, IModificationAudited, ISoftDelete
{
    public Guid Id { get; set; }
    public string BookName { get; set; } = null!;
    public List<Review>? Reviews { get; set; }
    public List<Author>? Authors { get; set; }
    public bool IsDeleted { get; set; }
}