using WikiGames.Application.Services;
using WikiGames.Application.UseCases.Comentario;
using WikiGames.Application.UseCases.Contenido;
using WikiGames.Application.UseCases.Denuncia;
using WikiGames.Application.UseCases.Notificacion;
using WikiGames.Application.UseCases.Usuario;
using WikiGames.Application.UseCases.Valoracion;
using WikiGames.Application.UseCases.Videojuego;
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

            // TODO: No seed generated for Usuario; define a non-customized New operation in the service to initialize it.

            // TODO: No seed generated for Videojuego; define a non-customized New operation in the service to initialize it.

            // TODO: No seed generated for Categoria; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship TemaConversacion.Videojuego manually; it could not be ordered automatically.
            // TODO: No seed generated for TemaConversacion; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship Comentario.Tema manually; it could not be ordered automatically.
            // TODO: No seed generated for Comentario; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship Denuncia.Denunciante manually; it could not be ordered automatically.
            // TODO: No seed generated for Denuncia; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship Valoracion.Videojuego manually; it could not be ordered automatically.
            // TODO: No seed generated for Valoracion; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship Rechazo.Videojuego manually; it could not be ordered automatically.
            // TODO: Set required relationship Rechazo.Gestor manually; it could not be ordered automatically.
            // TODO: No seed generated for Rechazo; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship Contenido.UsuarioCreador manually; it could not be ordered automatically.
            // TODO: No seed generated for Contenido; define a non-customized New operation in the service to initialize it.

            // TODO: No seed generated for Notificacion; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship AccionSistema.Causante manually; it could not be ordered automatically.
            // TODO: Set required relationship AccionSistema.ContenidoAfectado manually; it could not be ordered automatically.
            // TODO: No seed generated for AccionSistema; one or more New arguments reference classes that were not seeded yet.
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