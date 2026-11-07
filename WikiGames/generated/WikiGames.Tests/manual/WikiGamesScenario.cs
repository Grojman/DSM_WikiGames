using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using WikiGames.Application.Abstractions;
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

namespace WikiGames.Tests.Manual;

/// <summary>
/// Doble de prueba (fake) del servicio externo de correo: guarda los correos en memoria.
/// </summary>
public sealed class FakeEmailService : IEmailService
{
    public List<(string Para, string Asunto, string Cuerpo)> Enviados { get; } = new();

    public Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
    {
        Enviados.Add((recipientEmail, subject, body));
        return Task.FromResult(true);
    }
}

/// <summary>
/// IUnitOfWorkUtils de prueba que solo sabe resolver IEmailService.
/// </summary>
public sealed class FakeUnitOfWorkUtils : IUnitOfWorkUtils, IServiceProvider
{
    private readonly IEmailService _emailService;

    public FakeUnitOfWorkUtils(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public IServiceProvider Services => this;

    public object? GetService(Type serviceType) =>
        serviceType == typeof(IEmailService) ? _emailService : null;

    public T GetRequiredService<T>() where T : notnull =>
        (T)(GetService(typeof(T)) ?? throw new InvalidOperationException($"Servicio {typeof(T).Name} no registrado."));

    public T? GetService<T>() => (T?)GetService(typeof(T));
}

/// <summary>
/// Escenario reproducible y aislado (Tema 7): cada prueba usa su propia base de datos SQLite,
/// un UnitOfWork compartido por servicios y casos de uso, y un servicio de correo falso.
/// </summary>
public sealed class WikiGamesScenario : IAsyncDisposable
{
    private readonly string _dbPath;
    private readonly UnitOfWorkFactory _factory;

    private WikiGamesScenario(string dbPath, UnitOfWorkFactory factory)
    {
        _dbPath = dbPath;
        _factory = factory;
        UnitOfWork = factory.Create();
        Email = new FakeEmailService();
        Utils = new FakeUnitOfWorkUtils(Email);
    }

    public IUnitOfWork UnitOfWork { get; }
    public FakeEmailService Email { get; }
    public FakeUnitOfWorkUtils Utils { get; }

    public UsuarioUseCase UsuarioUseCase => new(UnitOfWork, Utils);
    public VideojuegoUseCase VideojuegoUseCase => new(UnitOfWork, Utils);
    public CategoriaUseCase CategoriaUseCase => new(UnitOfWork, Utils);
    public ComentarioUseCase ComentarioUseCase => new(UnitOfWork, Utils);
    public DenunciaUseCase DenunciaUseCase => new(UnitOfWork, Utils);
    public ValoracionUseCase ValoracionUseCase => new(UnitOfWork, Utils);
    public ContenidoUseCase ContenidoUseCase => new(UnitOfWork, Utils);
    public NotificacionUseCase NotificacionUseCase => new(UnitOfWork, Utils);

    public static async Task<WikiGamesScenario> CreateAsync()
    {
        string dbPath = Path.Combine(Path.GetTempPath(), $"wikigames-test-{Guid.NewGuid():N}.db");

        var cfg = new Configuration();
        string cfgPath = Path.Combine(AppContext.BaseDirectory, "hibernate.cfg.xml");
        if (File.Exists(cfgPath))
        {
            cfg.Configure(cfgPath);
        }
        else
        {
            cfg.Configure();
        }

        cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionString, $"Data Source={dbPath}");
        cfg.SetProperty(NHibernate.Cfg.Environment.ShowSql, "false");
        cfg.AddAssembly(typeof(UnitOfWorkFactory).Assembly);
        await new SchemaExport(cfg).CreateAsync(false, true);

        return new WikiGamesScenario(dbPath, new UnitOfWorkFactory(cfg));
    }

    /// <summary>Abre una sesión NUEVA para comprobar lo que realmente quedó persistido.</summary>
    public IUnitOfWork NuevaSesion() => _factory.Create();

    // ---------- Datos de partida (se crean sin notificaciones para no ensuciar los asserts) ----------

    public async Task<Usuario> CrearUsuarioAsync(string nombre, Rol rol = Rol.Registrado)
    {
        var usuario = new Usuario
        {
            Name = nombre,
            Email = $"{nombre.ToLowerInvariant()}@wikigames.test",
            Password = UnitOfWork.PasswordHasher.HashPasswordIfNeeded("1234"),
            RolUsuario = rol,
        };
        await UnitOfWork.UsuarioRepository.SaveAsync(usuario);
        return usuario;
    }

    public async Task<Videojuego> CrearVideojuegoAsync(Usuario dueno, string titulo = "Juego",
        EstadoContenido estado = EstadoContenido.Pendiente, DateTime? fecha = null)
    {
        var videojuego = new Videojuego
        {
            Texto = $"Descripción de {titulo}",
            FechaCreacion = fecha ?? DateTime.Now,
            Titulo = titulo,
            Enlace = "https://example.com",
            Imagen = "imagen.png",
            Estado = estado,
            UsuarioCreador = dueno,
        };
        await UnitOfWork.VideojuegoRepository.SaveAsync(videojuego);
        return videojuego;
    }

    public async Task<Denuncia> CrearDenunciaAsync(Usuario denunciante, EstadoContenido estado)
    {
        var denuncia = new Denuncia { Fecha = DateTime.Now, Estado = estado, Denunciante = denunciante };
        await UnitOfWork.DenunciaRepository.SaveAsync(denuncia);
        return denuncia;
    }

    public Task<TemaConversacion> CrearTemaAsync(Videojuego videojuego, Usuario autor, DateTime? fecha = null) =>
        new TemaConversacionService(UnitOfWork).NewAsync("Texto tema", fecha ?? DateTime.Now, 0, "Tema", videojuego.Id, autor.Id);

    public Task<Comentario> CrearComentarioAsync(TemaConversacion tema, Usuario autor, string texto = "Comentario") =>
        new ComentarioService(UnitOfWork).NewAsync(texto, DateTime.Now, 0, tema.Id, autor.Id);

    // ---------- Consultas de verificación (siempre desde una sesión nueva) ----------

    public sealed record NotificacionLeida(int Id, string Texto, IReadOnlyList<int> Notificados);

    public async Task<IReadOnlyList<NotificacionLeida>> LeerNotificacionesAsync()
    {
        await using IUnitOfWork uow = NuevaSesion();
        IReadOnlyList<Notificacion> notificaciones = await uow.NotificacionRepository.ReadAllAsync();
        return notificaciones
            .Select(n => new NotificacionLeida(n.Id, n.Texto, n.Notificados.Select(u => u.Id).ToList()))
            .ToList();
    }

    public async ValueTask DisposeAsync()
    {
        await UnitOfWork.DisposeAsync();
        _factory.Dispose();
        try
        {
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
            File.Delete(_dbPath);
        }
        catch
        {
            // El fichero temporal se puede quedar si SQLite aún lo tiene abierto; no afecta a la prueba.
        }
    }
}
