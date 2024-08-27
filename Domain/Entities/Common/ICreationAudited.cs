namespace EFCoreDemo.Domain.Entities.Common;

public interface ICreationAudited : IHasCreationTime
{
    string? CreatedBy { get; set; }
}