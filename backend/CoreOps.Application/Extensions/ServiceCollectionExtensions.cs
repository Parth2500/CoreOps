using CoreOps.Application.Operations.CreateRequest;
using CoreOps.Application.Operations.GetRequest;
using Microsoft.Extensions.DependencyInjection;

namespace CoreOps.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Handlers
        // Create Request
        services.AddScoped<CreateRequestHandler>();
        // Get Request
        services.AddScoped<GetRequestByIdQueryHandler>();
        // note: add more handlers here

        return services;
    }
}
