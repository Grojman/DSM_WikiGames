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
            // Mock data for every entity, inserted in dependency order:
            // Usuario -> Categoria -> Videojuego -> Rechazo -> Valoracion -> TemaConversacion
            // -> Comentario -> Contenido -> Denuncia/Notificacion -> AccionSistema -> N:M relations.
            // A single unit of work (one NHibernate session) is shared by all services so the
            // entities stay attached while the relationships are built.
            await using var uow = unitOfWorkFactory.Create();
            var usuarioService = new UsuarioService(uow);
            var categoriaService = new CategoriaService(uow);
            var videojuegoService = new VideojuegoService(uow);
            var rechazoService = new RechazoService(uow);
            var valoracionService = new ValoracionService(uow);
            var temaService = new TemaConversacionService(uow);
            var comentarioService = new ComentarioService(uow);
            var contenidoService = new ContenidoService(uow);
            var denunciaService = new DenunciaService(uow);
            var notificacionService = new NotificacionService(uow);
            var accionService = new AccionSistemaService(uow);

            var now = DateTime.Now;

            // ---------- Usuario ----------
            async Task<Usuario> NewUsuario(string name, string email, string password, Rol rol) =>
                await usuarioService.AddUsuarioDefaultAsync(new Usuario
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    RolUsuario = rol,
                });

            var admin = await NewUsuario("admin", "admin@wikigames.com", "admin123", Rol.Administrador);
            var moderador = await NewUsuario("moderador", "moderador@wikigames.com", "mod123", Rol.Moderador);
            var ana = await NewUsuario("ana", "ana@wikigames.com", "ana123", Rol.Registrado);
            var pablo = await NewUsuario("pablo", "pablo@wikigames.com", "pablo123", Rol.Registrado);
            var lucia = await NewUsuario("lucia", "lucia@wikigames.com", "lucia123", Rol.Registrado);
            Ensure((await usuarioService.ReadAllAsync()).Count >= 5, "Usuario mock data inserted.");
            Ensure(await usuarioService.LoginAsync(ana.Id, "ana123") is not null, "Usuario login works with hashed password.");

            // ---------- Categoria (with hierarchy) ----------
            var catAccion = await categoriaService.NewAsync("Acción");
            var catAventura = await categoriaService.NewAsync("Aventura");
            var catRpg = await categoriaService.NewAsync("RPG");
            var catEstrategia = await categoriaService.NewAsync("Estrategia");
            var catShooter = await categoriaService.NewAsync("Shooter");
            var catPlataformas = await categoriaService.NewAsync("Plataformas");
            var catRpgAccion = await categoriaService.NewAsync("RPG de acción");
            await categoriaService.AddHijoAsync(catAccion.Id, new[] { catShooter.Id, catPlataformas.Id });
            await categoriaService.AddPadreAsync(catRpgAccion.Id, catRpg.Id);
            Ensure(catShooter.Padre?.Id == catAccion.Id, "Categoria hierarchy created.");

            // ---------- Videojuego ----------
            async Task<Videojuego> NewVideojuego(string titulo, string texto, string enlace, string imagen,
                EstadoContenido estado, Usuario creador, int diasAtras, params Categoria[] categorias)
            {
                var juego = new Videojuego
                {
                    Titulo = titulo,
                    Texto = texto,
                    Enlace = enlace,
                    Imagen = imagen,
                    Estado = estado,
                    PuntuacionMedia = 0f,
                    NumeroMeGusta = 0,
                    FechaCreacion = now.AddDays(-diasAtras),
                    UsuarioCreador = creador,
                };
                foreach (var categoria in categorias)
                {
                    juego.Categorias.Add(categoria);
                    categoria.Videojuegos.Add(juego);
                }
                creador.ContenidoGenerado.Add(juego);
                return await videojuegoService.AddVideojuegoDefaultAsync(juego);
            }

            var starfall = await NewVideojuego("Starfall Odyssey", "Exploración espacial con combates por turnos.",
                "https://example.com/starfall", "https://example.com/img/starfall.png",
                EstadoContenido.Aprobado, ana, 60, catRpg, catAventura);
            var neonRush = await NewVideojuego("Neon Rush", "Shooter arcade frenético en una ciudad cyberpunk.",
                "https://example.com/neonrush", "https://example.com/img/neonrush.png",
                EstadoContenido.Aprobado, pablo, 30, catAccion, catShooter);
            var kingdoms = await NewVideojuego("Kingdoms of Ash", "Estrategia en tiempo real con gestión de reinos.",
                "https://example.com/kingdoms", "https://example.com/img/kingdoms.png",
                EstadoContenido.Aprobado, lucia, 15, catEstrategia);
            var pixelJump = await NewVideojuego("Pixel Jump", "Plataformas 2D de precisión.",
                "https://example.com/pixeljump", "https://example.com/img/pixeljump.png",
                EstadoContenido.Pendiente, ana, 2, catPlataformas);
            var cloneWars = await NewVideojuego("Copia Barata", "Juego sin información relevante.",
                "https://example.com/copia", "https://example.com/img/copia.png",
                EstadoContenido.Rechazado, pablo, 5, catAccion);
            Ensure((await videojuegoService.ReadAllAsync()).Count >= 5, "Videojuego mock data inserted.");

            // ---------- Rechazo ----------
            var rechazo = await rechazoService.NewAsync(now.AddDays(-4),
                "Ficha incompleta y enlace no oficial.", cloneWars.Id, moderador.Id);
            cloneWars.Rechazo = rechazo;
            moderador.Rechazos.Add(rechazo);
            await videojuegoService.ModifierVideojuegoDefaultAsync(cloneWars);
            Ensure((await rechazoService.ReadAllAsync()).Count >= 1, "Rechazo mock data inserted.");

            // ---------- Valoracion ----------
            async Task<Valoracion> NewValoracion(Videojuego juego, Usuario autor, float puntuacion, string texto, int diasAtras)
            {
                var valoracion = new Valoracion
                {
                    Texto = texto,
                    Puntuacion = puntuacion,
                    FechaCreacion = now.AddDays(-diasAtras),
                    NumeroMeGusta = 0,
                    UsuarioCreador = autor,
                    Videojuego = juego,
                };
                juego.Valoraciones.Add(valoracion);
                autor.ContenidoGenerado.Add(valoracion);
                return await valoracionService.AddValoracionDefaultAsync(valoracion);
            }

            await NewValoracion(starfall, pablo, 9.5f, "Una obra maestra del género.", 50);
            await NewValoracion(starfall, lucia, 8.0f, "Muy bueno, aunque algo largo.", 40);
            await NewValoracion(neonRush, ana, 7.0f, "Divertido para partidas cortas.", 20);
            await NewValoracion(neonRush, lucia, 4.5f, "Repetitivo después de unas horas.", 18);
            await NewValoracion(kingdoms, pablo, 6.5f, "Buena idea, mala interfaz.", 10);
            await NewValoracion(kingdoms, ana, 2.0f, "No me ha gustado nada.", 8);
            foreach (var juego in new[] { starfall, neonRush, kingdoms })
            {
                juego.PuntuacionMedia = juego.Valoraciones.Average(v => v.Puntuacion);
                await videojuegoService.ModifierVideojuegoDefaultAsync(juego);
            }
            Ensure((await valoracionService.ReadAllAsync()).Count >= 6, "Valoracion mock data inserted.");

            // ---------- TemaConversacion ----------
            var temaStarfall = await temaService.NewAsync("¿Cuál es la mejor build para el capitán?", now.AddDays(-35), 0,
                "Builds de Starfall", starfall.Id, lucia.Id);
            var temaNeon = await temaService.NewAsync("Compartid vuestras mejores puntuaciones.", now.AddDays(-12), 0,
                "Récords de Neon Rush", neonRush.Id, pablo.Id);
            var temaKingdoms = await temaService.NewAsync("Guía para principiantes de Kingdoms of Ash.", now.AddDays(-3), 0,
                "Guía de inicio", kingdoms.Id, ana.Id);
            Ensure((await temaService.ReadAllAsync()).Count >= 3, "TemaConversacion mock data inserted.");

            // ---------- Comentario (with replies) ----------
            var com1 = await comentarioService.NewAsync("Yo voy full ingeniería, va genial.", now.AddDays(-34), 0, temaStarfall.Id, pablo.Id);
            var com2 = await comentarioService.NewAsync("¡Gracias! Lo probaré esta noche.", now.AddDays(-33), 0, temaStarfall.Id, lucia.Id);
            var com3 = await comentarioService.NewAsync("La ingeniería está muy rota, la nerfearán.", now.AddDays(-32), 0, temaStarfall.Id, ana.Id);
            var com4 = await comentarioService.NewAsync("Mi récord: 1.250.000 puntos.", now.AddDays(-11), 0, temaNeon.Id, ana.Id);
            var com5 = await comentarioService.NewAsync("Eso es imposible, seguro que es trampa.", now.AddDays(-10), 0, temaNeon.Id, lucia.Id);
            var com6 = await comentarioService.NewAsync("Empieza siempre construyendo granjas.", now.AddDays(-2), 0, temaKingdoms.Id, pablo.Id);
            await comentarioService.AddRespuestaAsync(com1.Id, new[] { com2.Id, com3.Id });
            await comentarioService.AddComentarioAsociadoAsync(com5.Id, com4.Id);
            Ensure(com2.ComentarioAsociado?.Id == com1.Id, "Comentario replies created.");
            Ensure((await comentarioService.ReadAllAsync()).Count >= 6, "Comentario mock data inserted.");

            // ---------- Contenido (plain) ----------
            var contenidoAnuncio = await contenidoService.NewCustomizedAsync("¡Bienvenidos a WikiGames!", admin.Id);
            var contenidoNormas = await contenidoService.NewCustomizedAsync("Recordad respetar las normas de la comunidad.", moderador.Id);
            Ensure((await contenidoService.ReadAllAsync()).Count > 0, "Contenido mock data inserted.");

            // ---------- Me gusta / Favoritos ----------
            async Task Like(Contenido contenido, params Usuario[] usuarios)
            {
                await contenidoService.AddUsuarioMeGustaAsync(contenido.Id, usuarios.Select(u => u.Id).ToArray());
                contenido.NumeroMeGusta = contenido.UsuariosMeGusta.Count;
                await contenidoService.ModifierContenidoDefaultAsync(contenido);
            }

            await Like(starfall, pablo, lucia, moderador);
            await Like(neonRush, ana, lucia);
            await Like(kingdoms, pablo);
            await Like(temaStarfall, ana, pablo);
            await Like(com1, lucia, ana);
            await Like(com4, pablo);
            await Like(contenidoAnuncio, ana, pablo, lucia);
            await contenidoService.AddUsuarioFavoritoAsync(starfall.Id, new[] { pablo.Id, lucia.Id });
            await contenidoService.AddUsuarioFavoritoAsync(neonRush.Id, new[] { ana.Id });
            await contenidoService.AddUsuarioFavoritoAsync(kingdoms.Id, new[] { ana.Id, pablo.Id });
            Ensure(starfall.UsuariosMeGusta.Count == 3, "Me gusta relations created.");
            Ensure(ana.ContenidoFavorito.Count == 2, "Favorito relations created.");

            // ---------- Denuncia + Notificacion ----------
            // NewCustomizedAsync also creates the "denuncia registrada" notification for the denunciante.
            async Task<Denuncia> NewDenuncia(Usuario denunciante, Contenido contenido, EstadoContenido estado)
            {
                var denuncia = await denunciaService.NewCustomizedAsync(denunciante.Id);
                denuncia.Estado = estado;
                denunciante.DenunciasGeneradas.Add(denuncia);
                if (denuncia.Notificacion is not null)
                {
                    denuncia.Notificacion.Denuncia = denuncia;
                    denunciante.Notificaciones.Add(denuncia.Notificacion);
                }
                await denunciaService.ModifierDenunciaDefaultAsync(denuncia);
                await denunciaService.AddContenidoDenunciadoAsync(denuncia.Id, contenido.Id);
                await denunciaService.AddDenunciadoAsync(denuncia.Id, contenido.UsuarioCreador.Id);
                return denuncia;
            }

            var denunciaTrampa = await NewDenuncia(lucia, com4, EstadoContenido.Pendiente);
            var denunciaInsulto = await NewDenuncia(ana, com5, EstadoContenido.Aprobado);
            var denunciaJuego = await NewDenuncia(ana, cloneWars, EstadoContenido.Pendiente);
            await denunciaService.RechazarDenunciaAsync(denunciaJuego.Id);
            Ensure((await denunciaService.ReadAllAsync()).Count >= 3, "Denuncia mock data inserted.");

            // Extra notifications not tied to a denuncia creation.
            async Task<Notificacion> NewNotificacion(string texto, int diasAtras, params Usuario[] notificados)
            {
                var notificacion = await notificacionService.AddNotificacionDefaultAsync(new Notificacion
                {
                    Texto = texto,
                    Fecha = now.AddDays(-diasAtras),
                });
                foreach (var usuario in notificados)
                {
                    await usuarioService.AddNotificacioneAsync(usuario.Id, new[] { notificacion.Id });
                }
                return notificacion;
            }

            await NewNotificacion("Tu juego \"Copia Barata\" ha sido rechazado.", 4, pablo);
            await NewNotificacion("Tu juego \"Pixel Jump\" está pendiente de aprobación.", 2, ana);
            var notifSancion = await NewNotificacion("Se ha revisado una denuncia sobre tu comentario.", 1, lucia, moderador);
            await notificacionService.AddDenunciaAsync(notifSancion.Id, denunciaInsulto.Id);
            Ensure((await notificacionService.ReadAllAsync()).Count >= 6, "Notificacion mock data inserted.");

            // ---------- AccionSistema ----------
            await accionService.NewAsync(now.AddDays(-4), "Videojuego rechazado por el moderador.", moderador.Id, cloneWars.Id);
            await accionService.NewAsync(now.AddDays(-1), "Comentario retirado tras denuncia aprobada.", moderador.Id, com5.Id);
            await accionService.NewAsync(now.AddDays(-55), "Videojuego aprobado.", admin.Id, starfall.Id);
            await accionService.NewAsync(now, "Anuncio publicado en portada.", admin.Id, contenidoAnuncio.Id);
            await accionService.NewAsync(now, "Normas fijadas por el moderador.", moderador.Id, contenidoNormas.Id);
            Ensure((await accionService.ReadAllAsync()).Count >= 5, "AccionSistema mock data inserted.");

            Console.WriteLine("Manual mock data inserted for all entities.");
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