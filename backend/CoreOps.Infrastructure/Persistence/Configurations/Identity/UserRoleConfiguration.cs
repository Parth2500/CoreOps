using CoreOps.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreOps.Infrastructure.Persistence.Configurations.Identity;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles", "identity");

        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.RoleId).IsRequired();
    }
}
