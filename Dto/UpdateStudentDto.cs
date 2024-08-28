namespace EFCoreDemo.Dto;

public class UpdateStudentDto
{
    public int Id { get; set; }

    public string StudentName { get; set; } = null!;

    public string? Email { get; set; }
}