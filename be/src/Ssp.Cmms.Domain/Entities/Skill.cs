using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class Skill : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<TechnicianSkill> TechnicianSkills { get; set; } =
        new List<TechnicianSkill>();
}
