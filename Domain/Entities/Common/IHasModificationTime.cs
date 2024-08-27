namespace EFCoreDemo.Domain.Entities.Common;

public interface IHasModificationTime
{
    DateTime? ModifiedAt { get; set; }
}