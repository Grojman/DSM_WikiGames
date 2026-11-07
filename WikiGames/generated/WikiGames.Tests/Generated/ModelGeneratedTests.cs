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
        var filterExpression = "entity => entity.Name.Contains(p_name)";

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
        var filterExpression = "entity => entity.RolUsuario == p_rol";

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
        var filterExpression = "entity => entity.Titulo.Contains(p_texto)";

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
        var filterExpression = "entity => entity.Categorias.Any(c => c.Name == p_texto)";

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
        var filterExpression = "query => query.Where(v => v.FechaCreacion > DateTime.Now.AddDays(-7)).OrderByDescending(v => v.FechaCreacion)";

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
        var filterExpression = "query => query.OrderBy(v => v.FechaCreacion)";

        // <haria-protected id="test:filter:Videojuego:findAntiguos:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findAntiguos declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findAntiguos declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindByMeGusta_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findByMeGusta:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByMeGusta";
        var filterExpression = "query => query.OrderByDescending(v => v.NumeroMeGusta)";

        // <haria-protected id="test:filter:Videojuego:findByMeGusta:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Videojuego.findByMeGusta declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Videojuego.findByMeGusta declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Videojuego_FindPopulares_Filter_Contract()
    {
        // <haria-protected id="test:filter:Videojuego:findPopulares:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findPopulares";
        var filterExpression = "query => query.OrderByDescending(v => v.Temas.Count(t => t.FechaCreacion > DateTime.Now.AddDays(-7)) + v.Valoraciones.Count(w => w.FechaCreacion > DateTime.Now.AddDays(-7)))";

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
        var filterExpression = "entity => entity.Name.Contains(p_name)";

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
        var filterExpression = "entity => entity.Titulo.Contains(p_texto)";

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
        var filterExpression = "query => query.Where(t => t.FechaCreacion > DateTime.Now.AddDays(-7)).OrderByDescending(t => t.FechaCreacion)";

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
        var filterExpression = "entity => entity.Puntuacion == puntuacion";

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
        var filterExpression = "query => query.OrderBy(v => v.Puntuacion)";

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
        var filterExpression = "query => query.OrderByDescending(v => v.Puntuacion)";

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
    public void Contenido_FindByTextoLinQ_Filter_Contract()
    {
        // <haria-protected id="test:filter:Contenido:findByTextoLinQ:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findByTextoLinQ";
        var filterExpression = "entity => entity.Texto.Contains(p_texto)";

        // <haria-protected id="test:filter:Contenido:findByTextoLinQ:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Contenido.findByTextoLinQ declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Contenido.findByTextoLinQ declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Contenido_FindOrderByFecha_Filter_Contract()
    {
        // <haria-protected id="test:filter:Contenido:findOrderByFecha:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "findOrderByFecha";
        var filterExpression = "query => query.OrderByDescending(c => c.FechaCreacion)";

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
        var filterExpression = "query => query.OrderByDescending(c => c.NumeroMeGusta)";

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
        var filterExpression = "query => query.Where(c => c.UsuariosMeGusta.Any(u => u.Id == p_user))";

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
        var filterExpression = "entity => entity.Fecha == p_fecha";

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
