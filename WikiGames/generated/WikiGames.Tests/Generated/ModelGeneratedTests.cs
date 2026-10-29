using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Infrastructure;
using WikiGames.Infrastructure.Logging;
using Xunit.Abstractions;
using WikiGames.Domain.Enums;

namespace WikiGames.Tests.Generated;

public sealed class ModelGeneratedTests
{
    private readonly ITestOutputHelper _output;

    public ModelGeneratedTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Rol_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)Rol.Registrado);
        Assert.Equal(2, (int)Rol.Moderador);
        Assert.Equal(3, (int)Rol.Administrador);
    }

    [Fact]
    public void EstadoContenido_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)EstadoContenido.Pendiente);
        Assert.Equal(2, (int)EstadoContenido.Aprobado);
        Assert.Equal(3, (int)EstadoContenido.Rechazado);
    }

    [Fact]
    [Trait("Category", "Database")]
    public async Task Generated_Crud_And_Associations_Should_Work_Against_Database()
    {
        var cfg = CreateNHibernateConfiguration();
        new SchemaUpdate(cfg).Execute(false, true);
        using var loggerFactory = LoggerFactory.Create(logging => logging.SetMinimumLevel(LogLevel.Warning));
        using var unitOfWorkFactory = new UnitOfWorkFactory(cfg, loggerFactory);

            var usuarioService = new UsuarioService(unitOfWorkFactory);
            var sampleUsuario = await usuarioService.NewAsync("Sample Usuario Name", "Password123!", Rol.Registrado);
            var sampleUsuarioReadBack = await usuarioService.ReadOIDAsync(sampleUsuario.Id);
            Ensure(sampleUsuarioReadBack is not null, "Usuario read by OID returns the generated entity.");
            var sampleUsuarioReadAll = await usuarioService.ReadAllAsync();
            Ensure(sampleUsuarioReadAll.Count > 0, "Usuario read all returns generated data.");
            await usuarioService.ModifyAsync(sampleUsuario.Id, "Sample Usuario Name", "Password123!", Rol.Registrado);
            Ensure(await usuarioService.ReadOIDAsync(sampleUsuario.Id) is not null, "Usuario modify keeps the entity readable.");

            // TODO: No seed generated for Videojuego; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship Categoria.Videojuego manually; it could not be ordered automatically.
            // TODO: No seed generated for Categoria; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship TemaConversacion.Videojuego manually; it could not be ordered automatically.
            // TODO: No seed generated for TemaConversacion; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship Comentario.Tema manually; it could not be ordered automatically.
            // TODO: No seed generated for Comentario; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Denuncia; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship Valoracion.Videojuego manually; it could not be ordered automatically.
            // TODO: No seed generated for Valoracion; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship Rechazo.Videojuego manually; it could not be ordered automatically.
            // TODO: No seed generated for Rechazo; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Contenido; define a non-customized New operation in the service to initialize it.

            var notificacionService = new NotificacionService(unitOfWorkFactory);
            var sampleNotificacion = await notificacionService.NewAsync("Sample Notificacion Texto", new DateTime(2026, 1, 11));
            var sampleNotificacionReadBack = await notificacionService.ReadOIDAsync(sampleNotificacion.Id);
            Ensure(sampleNotificacionReadBack is not null, "Notificacion read by OID returns the generated entity.");
            var sampleNotificacionReadAll = await notificacionService.ReadAllAsync();
            Ensure(sampleNotificacionReadAll.Count > 0, "Notificacion read all returns generated data.");
            await notificacionService.ModifyAsync(sampleNotificacion.Id, "Sample Notificacion Texto", new DateTime(2026, 1, 11));
            Ensure(await notificacionService.ReadOIDAsync(sampleNotificacion.Id) is not null, "Notificacion modify keeps the entity readable.");

            // TODO: Set required relationship AccionSistema.ContenidoAfectado manually; it could not be ordered automatically.
            // TODO: No seed generated for AccionSistema; one or more New arguments reference classes that were not seeded yet.

            await notificacionService.DestroyAsync(sampleNotificacion.Id);
            await usuarioService.DestroyAsync(sampleUsuario.Id);

            Ensure((await usuarioService.ReadAllAsync()).Count == 0, "Usuario seed cleanup completed.");
            Ensure((await notificacionService.ReadAllAsync()).Count == 0, "Notificacion seed cleanup completed.");
    }

    private static Configuration CreateNHibernateConfiguration()
    {
        var configuration = new Configuration();
        var configPath = Path.Combine(AppContext.BaseDirectory, "hibernate.cfg.xml");
        if (File.Exists(configPath))
        {
            configuration.Configure(configPath);
        }
        else
        {
            configuration.Configure();
        }

        configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, "false");
        configuration.AddAssembly(typeof(UnitOfWorkFactory).Assembly);
        configuration.EnableSqlStatementCapture();
        return configuration;
    }

    private const string SuccessColor = "\u001b[32m";
    private const string ErrorColor = "\u001b[31m";
    private const string ResetColor = "\u001b[0m";

    private void Ensure(bool condition, string message)
    {
        WriteTestMessage(condition, message);
        Assert.True(condition, message);
    }

    private void WriteTestMessage(bool condition, string message)
    {
        var color = condition ? SuccessColor : ErrorColor;
        var status = condition ? "[OK] " : "[FAIL] ";
        _output.WriteLine($"{color}{status}{message}{ResetColor}");
    }

}
