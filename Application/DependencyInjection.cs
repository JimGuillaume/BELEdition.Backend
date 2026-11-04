using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Enregistrer ici les services de la couche Application (use cases, validators, ...)

        return services;
    }
}
