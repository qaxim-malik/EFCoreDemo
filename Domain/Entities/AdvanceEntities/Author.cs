using EFCoreDemo.Domain.Entities.Common;

namespace EFCoreDemo.Domain.Entities.AdvanceEntities;

public class Author : BaseEntity, ICreationAudited, IModificationAudited, ISoftDelete
{
    public Guid Id { get; set; }
    public string AuthorName { get; set; } = null!;
    public List<Book>? Books { get; set; }
    public bool IsDeleted { get; set; }
}