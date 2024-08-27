namespace EFCoreDemo.Domain.Entities.Common;

public interface IHasCreationTime
{
    DateTime CreatedAt { get; set; }
}