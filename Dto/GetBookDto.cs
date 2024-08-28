using EFCoreDemo.Domain.Entities.AdvanceEntities;

namespace EFCoreDemo.Dto;

public class GetBookDto
{
    public Guid Id { get; set; }
    public string BookName { get; set; } = null!;
    public List<Review>? Reviews { get; set; }
}