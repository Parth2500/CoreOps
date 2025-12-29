using CoreOps.Application.Operations.Repositories;
using CoreOps.Infrastructure.Persistence;
using CoreOps.Infrastructure.Persistence.Repositories.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreOps.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CoreOpsDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("CoreOpsDb"));
        });

        services.AddScoped<IRequestRepository, RequestRepository>();

        return services;
    }
}
