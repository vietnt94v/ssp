using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class Location : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Area { get; set; }

    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
