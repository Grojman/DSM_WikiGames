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
    public void Usuario_FindByNameLinQ_Filter_Contract()
    {
        // <haria-protected id="test:filter:Usuario:findByNameLinQ:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByNameLinQ";
        var filterExpression = "usuarios => usuarios.Where(u => u.Name.Contains(p_name)).ToListAsync()";

        // <haria-protected id="test:filter:Usuario:findByNameLinQ:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Usuario.findByNameLinQ declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Usuario.findByNameLinQ declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Usuario_FindByTipoUsuario_Filter_Contract()
    {
        // <haria-protected id="test:filter:Usuario:findByTipoUsuario:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByTipoUsuario";
        var filterExpression = "usuarios => usuarios.Where(u => u.Rol == p_rol).ToListAsync()";

        // <haria-protected id="test:filter:Usuario:findByTipoUsuario:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Usuario.findByTipoUsuario declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Usuario.findByTipoUsuario declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindByNombre_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findByNombre:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByNombre";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Videojuego:findByNombre:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findByNombre declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findByNombre declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindByCategoria_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findByCategoria:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByCategoria";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Videojuego:findByCategoria:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findByCategoria declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findByCategoria declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindRecientes_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findRecientes:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findRecientes";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Videojuego:findRecientes:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findRecientes declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findRecientes declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindAntiguos_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findAntiguos:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findAntiguos";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Videojuego:findAntiguos:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findAntiguos declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findAntiguos declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindPopulares_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findPopulares:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findPopulares";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Videojuego:findPopulares:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findPopulares declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findPopulares declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Categoria_FindByNameLinQ_Filter_Contract()
    {
        // <haria-protected id="test:filter:Categoria:findByNameLinQ:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByNameLinQ";
        var filterExpression = "entity => entity.Name == p_name";

        // <haria-protected id="test:filter:Categoria:findByNameLinQ:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Categoria.findByNameLinQ declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Categoria.findByNameLinQ declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void TemaConversacion_FindByTitulo_Filter_Contract()
    {
        // <haria-protected id="test:filter:TemaConversacion:findByTitulo:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByTitulo";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:TemaConversacion:findByTitulo:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "TemaConversacion.findByTitulo declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "TemaConversacion.findByTitulo declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void TemaConversacion_FindByRecientes_Filter_Contract()
    {
        // <haria-protected id="test:filter:TemaConversacion:findByRecientes:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByRecientes";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:TemaConversacion:findByRecientes:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "TemaConversacion.findByRecientes declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "TemaConversacion.findByRecientes declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Valoracion_FindByPuntuacion_Filter_Contract()
    {
        // <haria-protected id="test:filter:Valoracion:findByPuntuacion:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByPuntuacion";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Valoracion:findByPuntuacion:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Valoracion.findByPuntuacion declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Valoracion.findByPuntuacion declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Valoracion_FindPeores_Filter_Contract()
    {
        // <haria-protected id="test:filter:Valoracion:findPeores:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findPeores";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Valoracion:findPeores:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Valoracion.findPeores declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Valoracion.findPeores declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Valoracion_FindMejores_Filter_Contract()
    {
        // <haria-protected id="test:filter:Valoracion:findMejores:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findMejores";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Valoracion:findMejores:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Valoracion.findMejores declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Valoracion.findMejores declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void AccionSistema_FindByFechaLinQ_Filter_Contract()
    {
        // <haria-protected id="test:filter:AccionSistema:findByFechaLinQ:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByFechaLinQ";
        var filterExpression = "entity => entity.Fecha == p_fecha";

        // <haria-protected id="test:filter:AccionSistema:findByFechaLinQ:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "AccionSistema.findByFechaLinQ declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "AccionSistema.findByFechaLinQ declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Contenido_FindOrderByFecha_Filter_Contract()
    {
        // <haria-protected id="test:filter:Contenido:findOrderByFecha:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findOrderByFecha";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Contenido:findOrderByFecha:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Contenido.findOrderByFecha declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Contenido.findOrderByFecha declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Contenido_FindOrderMeGusta_Filter_Contract()
    {
        // <haria-protected id="test:filter:Contenido:findOrderMeGusta:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findOrderMeGusta";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Contenido:findOrderMeGusta:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Contenido.findOrderMeGusta declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Contenido.findOrderMeGusta declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Contenido_FindBYUserLiked_Filter_Contract()
    {
        // <haria-protected id="test:filter:Contenido:findBYUserLiked:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findBYUserLiked";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Contenido:findBYUserLiked:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Contenido.findBYUserLiked declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Contenido.findBYUserLiked declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Notificacion_FindByFecha_Filter_Contract()
    {
        // <haria-protected id="test:filter:Notificacion:findByFecha:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByFecha";
        var filterExpression = "entity => entity.Texto == p_texto";

        // <haria-protected id="test:filter:Notificacion:findByFecha:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Notificacion.findByFecha declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Notificacion.findByFecha declares the generated filter expression.");
        // </haria-protected>
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
