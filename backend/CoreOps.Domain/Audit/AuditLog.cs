using CoreOps.Domain.Common;

namespace CoreOps.Domain.Audit;

public class AuditLog : Entity
{
    public string EntityType { get; private set; } = default!;
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } = default!;
    public Guid PerformedBy { get; private set; }
    public DateTime PerformedAt { get; private set; }
    public string Data { get; private set; } = default!;

    private AuditLog() { }

    public AuditLog(
        Guid tenantId,
        string entityType,
        Guid entityId,
        string action,
        Guid performedBy,
        string data)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        EntityType = entityType;
        EntityId = entityId;
        Action = action;
        PerformedBy = performedBy;
        PerformedAt = DateTime.UtcNow;
        Data = data;
    }
}

