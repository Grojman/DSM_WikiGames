using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WikiGames.Application.Abstractions;

namespace WikiGames.Infrastructure.Utils.Manual;

public static class ExternalUtilRegistration
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(SmtpSettings.FromConfiguration(configuration));
        services.AddScoped<IEmailService, EmailService>();
    }
}
