using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Enregistrer ici les services de la couche Application (use cases, validators, ...)
        services.AddScoped<Application.Auth.IAuthService, Application.Auth.AuthService>();

        return services;
    }
}
