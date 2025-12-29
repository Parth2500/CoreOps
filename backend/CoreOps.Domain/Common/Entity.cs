namespace CoreOps.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public Guid TenantId { get; protected set; }
}
