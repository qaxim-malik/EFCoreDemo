using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Dto;

public class AddStudentDto
{
    public string StudentName { get; set; } = null!;

    [EmailAddress(ErrorMessage = "{0} is invalid")]
    public string? Email { get; set; }
    public Guid TeacherId { get; set; }
}