namespace EFCoreDemo.Dto;

public class GetAuthorDto
{
    public Guid Id { get; set; }
    public string AuthorName { get; set; } = null!;
    public List<GetBookDto>? Books { get; set; }
}