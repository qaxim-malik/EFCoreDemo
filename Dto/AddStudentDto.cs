namespace EFCoreDemo.Dto;

public class AddStudentDto
{
    public string StudentName { get; set; } = null!;
    public Guid TeacherId { get; set; }
}