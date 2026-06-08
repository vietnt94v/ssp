using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
