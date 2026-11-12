using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WikiGames.Application.Abstractions;
using WikiGames.Infrastructure.Utils.Manual;

namespace WikiGames.Infrastructure.Utils.Generated;

public static class ExternalUtilsServiceCollectionExtensions
{
    public static IServiceCollection AddHariaExternalUtils(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWorkUtils, UnitOfWorkUtils>();
        ExternalUtilRegistration.Register(services, configuration);
        return services;
    }
}