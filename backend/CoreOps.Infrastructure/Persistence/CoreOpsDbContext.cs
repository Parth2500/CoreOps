using CoreOps.Domain.Audit;
using CoreOps.Domain.Identity;
using CoreOps.Domain.Operations;
using CoreOps.Domain.Workflow;
using Microsoft.EntityFrameworkCore;

namespace CoreOps.Infrastructure.Persistence;

public class CoreOpsDbContext : DbContext
{
    public CoreOpsDbContext(DbContextOptions<CoreOpsDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<WorkflowState> WorkflowStates => Set<WorkflowState>();
    public DbSet<WorkflowTransition> WorkflowTransitions => Set<WorkflowTransition>();

    public DbSet<Request> Requests => Set<Request>();
    public DbSet<RequestStateHistory> RequestStateHistories => Set<RequestStateHistory>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreOpsDbContext).Assembly);
    }
}
