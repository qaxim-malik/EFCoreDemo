namespace EFCoreDemo.Domain.Entities.Common;

public interface IModificationAudited : IHasModificationTime
{
    string? ModifiedBy { get; set; }
}