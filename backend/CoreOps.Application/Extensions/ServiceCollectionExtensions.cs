using CoreOps.Application.Operations.CreateRequest;
using Microsoft.Extensions.DependencyInjection;

namespace CoreOps.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Handlers
        services.AddScoped<CreateRequestHandler>();

        return services;
    }
}
