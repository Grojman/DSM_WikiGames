// HARIA-WEB GENERATED sha256:b965b8722e7fdc679d9df7b6d5b170aa708595f0f0f486592d2239ba5a9d1ec3
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WikiGames.Web.Configuration;

public static class HariaApplicationRegistration
{
    public static IServiceCollection AddHariaApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var connectionString = RequireConnectionString(configuration, environment);
        var nhibernate = new NHibernate.Cfg.Configuration();
        nhibernate.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
        nhibernate.SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect");
        nhibernate.SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
        nhibernate.SetProperty("adonet.batch_size", "100");
        nhibernate.SetProperty("show_sql", "true");
        nhibernate.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectionString);
        nhibernate.AddAssembly(typeof(WikiGames.Infrastructure.UnitOfWorkFactory).Assembly);
        services.AddSingleton(nhibernate.BuildSessionFactory());
        services.AddSingleton<WikiGames.Application.Abstractions.IHariaPasswordHasher, WikiGames.Infrastructure.Security.HariaPasswordHasher>();
        services.AddSingleton<WikiGames.Application.Abstractions.IHariaTokenGenerator, WikiGames.Infrastructure.Security.HariaTokenGenerator>();
        services.AddScoped<WikiGames.Application.Abstractions.IUsuarioRepository, WikiGames.Infrastructure.Repositories.UsuarioRepository>();
        services.AddSingleton<WikiGames.Application.Abstractions.IUnitOfWorkFactory, WikiGames.Infrastructure.UnitOfWorkFactory>();
        services.AddScoped<WikiGames.Application.Services.VideojuegoService>();
        services.AddScoped<WikiGames.Application.Services.ContenidoService>();
        services.AddScoped<WikiGames.Application.Services.CategoriaService>();
        services.AddScoped<WikiGames.Application.Services.TemaConversacionService>();
        services.AddScoped<WikiGames.Application.Services.ComentarioService>();
        services.AddScoped<WikiGames.Application.Services.DenunciaService>();
        services.AddScoped<WikiGames.Application.Services.ValoracionService>();
        services.AddScoped<WikiGames.Application.Services.AccionSistemaService>();
        services.AddScoped<WikiGames.Application.Services.RechazoService>();
        services.AddScoped<WikiGames.Application.Services.NotificacionService>();
        services.AddScoped<WikiGames.Application.Services.UsuarioService>();
        return services;
    }

    private static string RequireConnectionString(IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Configure ConnectionStrings:DbConnection in appsettings.Haria.json, user-secrets, or the ConnectionStrings__DbConnection environment variable.");
        }

        var sqlite = new System.Data.Common.DbConnectionStringBuilder { ConnectionString = connectionString };
        var dataSourceKey = sqlite.Keys.Cast<string>().FirstOrDefault(key =>
            string.Equals(key.Replace(" ", ""), "DataSource", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "Filename", StringComparison.OrdinalIgnoreCase));
        if (dataSourceKey is null)
        {
            throw new InvalidOperationException("The SQLite connection string must specify Data Source or Filename.");
        }
        var dataSource = Convert.ToString(sqlite[dataSourceKey]) ?? string.Empty;
        if (dataSource.Length == 0 || dataSource.StartsWith("file:", StringComparison.OrdinalIgnoreCase) || Path.IsPathRooted(dataSource)
            || string.Equals(dataSource, ":memory:", StringComparison.OrdinalIgnoreCase)
            || (sqlite.TryGetValue("Mode", out var mode) && string.Equals(Convert.ToString(mode), "Memory", StringComparison.OrdinalIgnoreCase)))
        {
            return sqlite.ConnectionString;
        }

        sqlite[dataSourceKey] = Path.GetFullPath(Path.Combine(environment.ContentRootPath, dataSource));
        return sqlite.ConnectionString;
    }
}
