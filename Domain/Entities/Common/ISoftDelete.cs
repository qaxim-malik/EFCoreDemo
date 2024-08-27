using System.ComponentModel;

namespace EFCoreDemo.Domain.Entities.Common;

public interface ISoftDelete
{
    [DefaultValue(false)]
    bool IsDeleted { get; set; }
}