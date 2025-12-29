using CoreOps.Domain.Common;

namespace CoreOps.Domain.Identity;

public class User : Entity
{
    public string Email { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private User() { }

    public User(Guid tenantId, Guid id, string email, string displayName)
    {
        TenantId = tenantId;
        Id = id;
        Email = email;
        DisplayName = displayName;
        IsActive = true;
    }
}

