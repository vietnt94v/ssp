using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class Technician : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Department { get; set; }
    public decimal? Rating { get; set; }

    public User? User { get; set; }
    public ICollection<TechnicianSkill> TechnicianSkills { get; set; } =
        new List<TechnicianSkill>();
    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}

public class TechnicianSkill
{
    public Guid TechnicianId { get; set; }
    public Guid SkillId { get; set; }

    public Technician? Technician { get; set; }
    public Skill? Skill { get; set; }
}
