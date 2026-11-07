using WikiGames.Application.Services;
using WikiGames.Application.UseCases.Categoria;
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

            var categoriaService = new CategoriaService(unitOfWorkFactory);
            var sampleCategoria = await categoriaService.NewAsync("Sample Categoria Name");
            var sampleCategoriaReadBack = await categoriaService.ReadOIDAsync(sampleCategoria.Id);
            Ensure(sampleCategoriaReadBack is not null, "Categoria read by OID returns the generated entity.");
            var sampleCategoriaReadAll = await categoriaService.ReadAllAsync();
            Ensure(sampleCategoriaReadAll.Count > 0, "Categoria read all returns generated data.");
            var sampleCategoriaFindByNameLinQResult = await categoriaService.FindByNameLinQAsync("Sample Categoria Name");
            Ensure(sampleCategoriaFindByNameLinQResult is not null, "Categoria.findByNameLinQ filter executes.");
            await categoriaService.ModifyAsync(sampleCategoria.Id, "Sample Categoria Name", default!);
            Ensure(await categoriaService.ReadOIDAsync(sampleCategoria.Id) is not null, "Categoria modify keeps the entity readable.");

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

            // TODO: Set required relationship Notificacion.Notificados manually; it could not be ordered automatically.
            // TODO: No seed generated for Notificacion; define a non-customized New operation in the service to initialize it.

            // TODO: Set required relationship AccionSistema.Causante manually; it could not be ordered automatically.
            // TODO: Set required relationship AccionSistema.ContenidoAfectado manually; it could not be ordered automatically.
            // TODO: No seed generated for AccionSistema; one or more New arguments reference classes that were not seeded yet.

            await categoriaService.AddHijoAsync(sampleCategoria.Id, new[] { sampleCategoria.Id });
            Ensure(sampleCategoria.Hijos.Contains(sampleCategoria), "Categoria.addHijo relates Categoria and Categoria.");
            await categoriaService.AddPadreAsync(sampleCategoria.Id, sampleCategoria.Id);
            Ensure(sampleCategoria.Padre?.Equals(sampleCategoria) == true, "Categoria.addPadre relates Categoria and Categoria.");

            await categoriaService.DestroyAsync(sampleCategoria.Id);

            Ensure((await categoriaService.ReadAllAsync()).Count == 0, "Categoria seed cleanup completed.");
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