using WikiGames.Application.Services;
using WikiGames.Application.UseCases.Valoracion;
using WikiGames.Domain.Entities;

using WikiGames.Domain.Enums;
using WikiGames.Infrastructure;
using WikiGames.Infrastructure.Repositories;
using WikiGames.Infrastructure.Security;
using Microsoft.Extensions.Logging;

/*PROTECTED REGION ID(seedDataUsings) ENABLED START*/
// Add custom usings here.
/*PROTECTED REGION END*/

namespace InitializeDB;

public static class SeedData
{
    public static async Task InitializeDataAsync(NHibernate.Cfg.Configuration cfg, ILoggerFactory? loggerFactory = null)
    {
        try
        {
            using var unitOfWorkFactory = new UnitOfWorkFactory(cfg, loggerFactory);

            var usuarioService = new UsuarioService(unitOfWorkFactory);
            var sampleUsuario = await usuarioService.NewAsync("Sample Usuario Name", "Password123!", Rol.Registrado);
            var sampleUsuarioReadBack = await usuarioService.ReadOIDAsync(sampleUsuario.Id);
            Ensure(sampleUsuarioReadBack is not null, "Usuario read by OID returns the generated entity.");
            var sampleUsuarioReadAll = await usuarioService.ReadAllAsync();
            Ensure(sampleUsuarioReadAll.Count > 0, "Usuario read all returns generated data.");
            var sampleUsuarioFindByNameLinQResult = await usuarioService.FindByNameLinQAsync("Sample Usuario Name");
            Ensure(sampleUsuarioFindByNameLinQResult is not null, "Usuario.findByNameLinQ filter executes.");
            var sampleUsuarioFindByTipoUsuarioResult = await usuarioService.FindByTipoUsuarioAsync(Rol.Registrado);
            Ensure(sampleUsuarioFindByTipoUsuarioResult is not null, "Usuario.findByTipoUsuario filter executes.");
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
            var sampleNotificacionFindByFechaResult = await notificacionService.FindByFechaAsync("Sample Notificacion Texto");
            Ensure(sampleNotificacionFindByFechaResult is not null, "Notificacion.findByFecha filter executes.");
            await notificacionService.ModifyAsync(sampleNotificacion.Id, "Sample Notificacion Texto", new DateTime(2026, 1, 11));
            Ensure(await notificacionService.ReadOIDAsync(sampleNotificacion.Id) is not null, "Notificacion modify keeps the entity readable.");

            // TODO: Set required relationship AccionSistema.ContenidoAfectado manually; it could not be ordered automatically.
            // TODO: No seed generated for AccionSistema; one or more New arguments reference classes that were not seeded yet.

            await notificacionService.DestroyAsync(sampleNotificacion.Id);
            await usuarioService.DestroyAsync(sampleUsuario.Id);

            Ensure((await usuarioService.ReadAllAsync()).Count == 0, "Usuario seed cleanup completed.");
            Ensure((await notificacionService.ReadAllAsync()).Count == 0, "Notificacion seed cleanup completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException ?? ex);
            throw;
        }
    }

    public static async Task InitializeDataManualAsync(NHibernate.Cfg.Configuration cfg, ILoggerFactory? loggerFactory = null)
    {
        try
        {
            using var unitOfWorkFactory = new UnitOfWorkFactory(cfg, loggerFactory);

            /*PROTECTED REGION ID(initializeDataManual) ENABLED START*/
            /*PROTECTED REGION END*/
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException ?? ex);
            throw;
        }
    }

    private static void Ensure(bool condition, string message)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = condition ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine((condition ? "[OK] " : "[FAIL] ") + message);
        Console.ForegroundColor = previousColor;

        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}