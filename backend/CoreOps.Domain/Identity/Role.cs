using CoreOps.Domain.Common;

namespace CoreOps.Domain.Identity;

public class Role : Entity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private Role() { }

    public Role(Guid tenantId, string code, string name)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Code = code;
        Name = name;
        IsActive = true;
    }
}

