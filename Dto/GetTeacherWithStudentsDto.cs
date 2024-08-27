namespace EFCoreDemo.Dto;

public class GetTeacherWithStudentsDto
{
    public Guid Id { get; set; }
    public string TeacherName { get; set; } = null!;
    public List<GetStudentsDto>? Students { get; set; }
}