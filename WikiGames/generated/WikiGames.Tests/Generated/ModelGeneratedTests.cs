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

            var videojuegoService = new VideojuegoService(unitOfWorkFactory);
            var sampleVideojuego = await videojuegoService.NewAsync("Sample Videojuego Texto", new DateTime(2026, 1, 2), 2, "Sample Videojuego Titulo", "Sample Videojuego Enlace", 2, "Sample Videojuego Imagen", EstadoContenido.Pendiente, sampleUsuario.Id);
            Ensure(sampleVideojuego.UsuarioCreador?.Equals(sampleUsuario) == true, "Videojuego.new relates sampleVideojuego and sampleUsuario.");
            var sampleVideojuegoReadBack = await videojuegoService.ReadOIDAsync(sampleVideojuego.Id);
            Ensure(sampleVideojuegoReadBack is not null, "Videojuego read by OID returns the generated entity.");
            var sampleVideojuegoReadAll = await videojuegoService.ReadAllAsync();
            Ensure(sampleVideojuegoReadAll.Count > 0, "Videojuego read all returns generated data.");
            await videojuegoService.ModifyAsync(sampleVideojuego.Id, "Sample Videojuego Texto", new DateTime(2026, 1, 2), 2, "Sample Videojuego Titulo", "Sample Videojuego Enlace", 2, "Sample Videojuego Imagen", EstadoContenido.Pendiente, sampleUsuario.Id);
            Ensure(await videojuegoService.ReadOIDAsync(sampleVideojuego.Id) is not null, "Videojuego modify keeps the entity readable.");

            var categoriaService = new CategoriaService(unitOfWorkFactory);
            var sampleCategoria = await categoriaService.NewAsync("Sample Categoria Name", sampleVideojuego.Id);
            Ensure(sampleCategoria.Videojuego?.Equals(sampleVideojuego) == true, "Categoria.new relates sampleCategoria and sampleVideojuego.");
            var sampleCategoriaReadBack = await categoriaService.ReadOIDAsync(sampleCategoria.Id);
            Ensure(sampleCategoriaReadBack is not null, "Categoria read by OID returns the generated entity.");
            var sampleCategoriaReadAll = await categoriaService.ReadAllAsync();
            Ensure(sampleCategoriaReadAll.Count > 0, "Categoria read all returns generated data.");
            await categoriaService.ModifyAsync(sampleCategoria.Id, "Sample Categoria Name", sampleVideojuego.Id);
            Ensure(await categoriaService.ReadOIDAsync(sampleCategoria.Id) is not null, "Categoria modify keeps the entity readable.");

            var temaConversacionService = new TemaConversacionService(unitOfWorkFactory);
            var sampleTemaConversacion = await temaConversacionService.NewAsync("Sample TemaConversacion Texto", new DateTime(2026, 1, 4), 4, "Sample TemaConversacion Titulo", sampleVideojuego.Id, sampleUsuario.Id);
            Ensure(sampleTemaConversacion.UsuarioCreador?.Equals(sampleUsuario) == true, "TemaConversacion.new relates sampleTemaConversacion and sampleUsuario.");
            Ensure(sampleTemaConversacion.Videojuego?.Equals(sampleVideojuego) == true, "TemaConversacion.new relates sampleTemaConversacion and sampleVideojuego.");
            var sampleTemaConversacionReadBack = await temaConversacionService.ReadOIDAsync(sampleTemaConversacion.Id);
            Ensure(sampleTemaConversacionReadBack is not null, "TemaConversacion read by OID returns the generated entity.");
            var sampleTemaConversacionReadAll = await temaConversacionService.ReadAllAsync();
            Ensure(sampleTemaConversacionReadAll.Count > 0, "TemaConversacion read all returns generated data.");
            await temaConversacionService.ModifyAsync(sampleTemaConversacion.Id, "Sample TemaConversacion Texto", new DateTime(2026, 1, 4), 4, "Sample TemaConversacion Titulo", sampleVideojuego.Id, sampleUsuario.Id);
            Ensure(await temaConversacionService.ReadOIDAsync(sampleTemaConversacion.Id) is not null, "TemaConversacion modify keeps the entity readable.");

            var comentarioService = new ComentarioService(unitOfWorkFactory);
            var sampleComentario = await comentarioService.NewAsync("Sample Comentario Texto", new DateTime(2026, 1, 5), 5, sampleTemaConversacion.Id, sampleUsuario.Id);
            Ensure(sampleComentario.UsuarioCreador?.Equals(sampleUsuario) == true, "Comentario.new relates sampleComentario and sampleUsuario.");
            Ensure(sampleComentario.Tema?.Equals(sampleTemaConversacion) == true, "Comentario.new relates sampleComentario and sampleTemaConversacion.");
            var sampleComentarioReadBack = await comentarioService.ReadOIDAsync(sampleComentario.Id);
            Ensure(sampleComentarioReadBack is not null, "Comentario read by OID returns the generated entity.");
            var sampleComentarioReadAll = await comentarioService.ReadAllAsync();
            Ensure(sampleComentarioReadAll.Count > 0, "Comentario read all returns generated data.");
            await comentarioService.ModifyAsync(sampleComentario.Id, "Sample Comentario Texto", new DateTime(2026, 1, 5), 5, sampleTemaConversacion.Id, sampleUsuario.Id);
            Ensure(await comentarioService.ReadOIDAsync(sampleComentario.Id) is not null, "Comentario modify keeps the entity readable.");

            var denunciaService = new DenunciaService(unitOfWorkFactory);
            var sampleDenuncia = await denunciaService.NewAsync(new DateTime(2026, 1, 6), EstadoContenido.Pendiente, sampleUsuario.Id);
            Ensure(sampleDenuncia.Denunciante?.Equals(sampleUsuario) == true, "Denuncia.new relates sampleDenuncia and sampleUsuario.");
            var sampleDenunciaReadBack = await denunciaService.ReadOIDAsync(sampleDenuncia.Id);
            Ensure(sampleDenunciaReadBack is not null, "Denuncia read by OID returns the generated entity.");
            var sampleDenunciaReadAll = await denunciaService.ReadAllAsync();
            Ensure(sampleDenunciaReadAll.Count > 0, "Denuncia read all returns generated data.");
            await denunciaService.ModifyAsync(sampleDenuncia.Id, new DateTime(2026, 1, 6), EstadoContenido.Pendiente, sampleUsuario.Id);
            Ensure(await denunciaService.ReadOIDAsync(sampleDenuncia.Id) is not null, "Denuncia modify keeps the entity readable.");

            var valoracionService = new ValoracionService(unitOfWorkFactory);
            var sampleValoracion = await valoracionService.NewAsync("Sample Valoracion Texto", new DateTime(2026, 1, 7), 7, 7.5f, sampleVideojuego.Id, sampleUsuario.Id);
            Ensure(sampleValoracion.UsuarioCreador?.Equals(sampleUsuario) == true, "Valoracion.new relates sampleValoracion and sampleUsuario.");
            Ensure(sampleValoracion.Videojuego?.Equals(sampleVideojuego) == true, "Valoracion.new relates sampleValoracion and sampleVideojuego.");
            var sampleValoracionReadBack = await valoracionService.ReadOIDAsync(sampleValoracion.Id);
            Ensure(sampleValoracionReadBack is not null, "Valoracion read by OID returns the generated entity.");
            var sampleValoracionReadAll = await valoracionService.ReadAllAsync();
            Ensure(sampleValoracionReadAll.Count > 0, "Valoracion read all returns generated data.");
            await valoracionService.ModifyAsync(sampleValoracion.Id, "Sample Valoracion Texto", new DateTime(2026, 1, 7), 7, 7.5f, sampleVideojuego.Id, sampleUsuario.Id);
            Ensure(await valoracionService.ReadOIDAsync(sampleValoracion.Id) is not null, "Valoracion modify keeps the entity readable.");

            var rechazoService = new RechazoService(unitOfWorkFactory);
            var sampleRechazo = await rechazoService.NewAsync(new DateTime(2026, 1, 9), "Sample Rechazo Motivo", sampleVideojuego.Id, sampleUsuario.Id);
            Ensure(sampleRechazo.Videojuego?.Equals(sampleVideojuego) == true, "Rechazo.new relates sampleRechazo and sampleVideojuego.");
            Ensure(sampleRechazo.Gestor?.Equals(sampleUsuario) == true, "Rechazo.new relates sampleRechazo and sampleUsuario.");
            var sampleRechazoReadBack = await rechazoService.ReadOIDAsync(sampleRechazo.Id);
            Ensure(sampleRechazoReadBack is not null, "Rechazo read by OID returns the generated entity.");
            var sampleRechazoReadAll = await rechazoService.ReadAllAsync();
            Ensure(sampleRechazoReadAll.Count > 0, "Rechazo read all returns generated data.");
            await rechazoService.ModifyAsync(sampleRechazo.Id, new DateTime(2026, 1, 9), "Sample Rechazo Motivo", sampleVideojuego.Id, sampleUsuario.Id);
            Ensure(await rechazoService.ReadOIDAsync(sampleRechazo.Id) is not null, "Rechazo modify keeps the entity readable.");

            var contenidoService = new ContenidoService(unitOfWorkFactory);
            var sampleContenido = await contenidoService.NewAsync("Sample Contenido Texto", new DateTime(2026, 1, 10), 10, sampleUsuario.Id);
            Ensure(sampleContenido.UsuarioCreador?.Equals(sampleUsuario) == true, "Contenido.new relates sampleContenido and sampleUsuario.");
            var sampleContenidoReadBack = await contenidoService.ReadOIDAsync(sampleContenido.Id);
            Ensure(sampleContenidoReadBack is not null, "Contenido read by OID returns the generated entity.");
            var sampleContenidoReadAll = await contenidoService.ReadAllAsync();
            Ensure(sampleContenidoReadAll.Count > 0, "Contenido read all returns generated data.");
            await contenidoService.ModifyAsync(sampleContenido.Id, "Sample Contenido Texto", new DateTime(2026, 1, 10), 10, sampleUsuario.Id);
            Ensure(await contenidoService.ReadOIDAsync(sampleContenido.Id) is not null, "Contenido modify keeps the entity readable.");

            var notificacionService = new NotificacionService(unitOfWorkFactory);
            var sampleNotificacion = await notificacionService.NewAsync("Sample Notificacion Texto", new DateTime(2026, 1, 11));
            var sampleNotificacionReadBack = await notificacionService.ReadOIDAsync(sampleNotificacion.Id);
            Ensure(sampleNotificacionReadBack is not null, "Notificacion read by OID returns the generated entity.");
            var sampleNotificacionReadAll = await notificacionService.ReadAllAsync();
            Ensure(sampleNotificacionReadAll.Count > 0, "Notificacion read all returns generated data.");
            await notificacionService.ModifyAsync(sampleNotificacion.Id, "Sample Notificacion Texto", new DateTime(2026, 1, 11));
            Ensure(await notificacionService.ReadOIDAsync(sampleNotificacion.Id) is not null, "Notificacion modify keeps the entity readable.");

            var accionSistemaService = new AccionSistemaService(unitOfWorkFactory);
            var sampleAccionSistema = await accionSistemaService.NewAsync(new DateTime(2026, 1, 8), "Sample AccionSistema Descripcion", sampleUsuario.Id, sampleContenido.Id);
            Ensure(sampleAccionSistema.Causante?.Equals(sampleUsuario) == true, "AccionSistema.new relates sampleAccionSistema and sampleUsuario.");
            Ensure(sampleAccionSistema.ContenidoAfectado?.Equals(sampleContenido) == true, "AccionSistema.new relates sampleAccionSistema and sampleContenido.");
            var sampleAccionSistemaReadBack = await accionSistemaService.ReadOIDAsync(sampleAccionSistema.Id);
            Ensure(sampleAccionSistemaReadBack is not null, "AccionSistema read by OID returns the generated entity.");
            var sampleAccionSistemaReadAll = await accionSistemaService.ReadAllAsync();
            Ensure(sampleAccionSistemaReadAll.Count > 0, "AccionSistema read all returns generated data.");
            await accionSistemaService.ModifyAsync(sampleAccionSistema.Id, new DateTime(2026, 1, 8), "Sample AccionSistema Descripcion", sampleUsuario.Id, sampleContenido.Id);
            Ensure(await accionSistemaService.ReadOIDAsync(sampleAccionSistema.Id) is not null, "AccionSistema modify keeps the entity readable.");

            await usuarioService.AddDenunciasRecibidaAsync(sampleUsuario.Id, new[] { sampleDenuncia.Id });
            Ensure(sampleUsuario.DenunciasRecibidas.Contains(sampleDenuncia), "Usuario.addDenunciasRecibida relates Usuario and Denuncia.");
            await categoriaService.AddHijoAsync(sampleCategoria.Id, new[] { sampleCategoria.Id });
            Ensure(sampleCategoria.Hijos.Contains(sampleCategoria), "Categoria.addHijo relates Categoria and Categoria.");
            await categoriaService.AddPadreAsync(sampleCategoria.Id, sampleCategoria.Id);
            Ensure(sampleCategoria.Padre?.Equals(sampleCategoria) == true, "Categoria.addPadre relates Categoria and Categoria.");
            await comentarioService.AddComentarioAsociadoAsync(sampleComentario.Id, sampleComentario.Id);
            Ensure(sampleComentario.ComentarioAsociado?.Equals(sampleComentario) == true, "Comentario.addComentarioAsociado relates Comentario and Comentario.");
            await comentarioService.AddRespuestaAsync(sampleComentario.Id, new[] { sampleComentario.Id });
            Ensure(sampleComentario.Respuestas.Contains(sampleComentario), "Comentario.addRespuesta relates Comentario and Comentario.");
            await denunciaService.AddDenunciadoAsync(sampleDenuncia.Id, sampleUsuario.Id);
            Ensure(sampleDenuncia.Denunciado?.Equals(sampleUsuario) == true, "Denuncia.addDenunciado relates Denuncia and Usuario.");
            await denunciaService.AddContenidoDenunciadoAsync(sampleDenuncia.Id, sampleContenido.Id);
            Ensure(sampleDenuncia.ContenidoDenunciado?.Equals(sampleContenido) == true, "Denuncia.addContenidoDenunciado relates Denuncia and Contenido.");
            await contenidoService.AddDenunciaAsync(sampleContenido.Id, new[] { sampleDenuncia.Id });
            Ensure(sampleContenido.Denuncias.Contains(sampleDenuncia), "Contenido.addDenuncia relates Contenido and Denuncia.");

            await categoriaService.DestroyAsync(sampleCategoria.Id);
            await comentarioService.DestroyAsync(sampleComentario.Id);
            await temaConversacionService.DestroyAsync(sampleTemaConversacion.Id);
            await denunciaService.DestroyAsync(sampleDenuncia.Id);
            await valoracionService.DestroyAsync(sampleValoracion.Id);
            await rechazoService.DestroyAsync(sampleRechazo.Id);
            await videojuegoService.DestroyAsync(sampleVideojuego.Id);
            await notificacionService.DestroyAsync(sampleNotificacion.Id);
            await accionSistemaService.DestroyAsync(sampleAccionSistema.Id);
            await contenidoService.DestroyAsync(sampleContenido.Id);
            await usuarioService.DestroyAsync(sampleUsuario.Id);

            Ensure((await usuarioService.ReadAllAsync()).Count == 0, "Usuario seed cleanup completed.");
            Ensure((await videojuegoService.ReadAllAsync()).Count == 0, "Videojuego seed cleanup completed.");
            Ensure((await categoriaService.ReadAllAsync()).Count == 0, "Categoria seed cleanup completed.");
            Ensure((await temaConversacionService.ReadAllAsync()).Count == 0, "TemaConversacion seed cleanup completed.");
            Ensure((await comentarioService.ReadAllAsync()).Count == 0, "Comentario seed cleanup completed.");
            Ensure((await denunciaService.ReadAllAsync()).Count == 0, "Denuncia seed cleanup completed.");
            Ensure((await valoracionService.ReadAllAsync()).Count == 0, "Valoracion seed cleanup completed.");
            Ensure((await rechazoService.ReadAllAsync()).Count == 0, "Rechazo seed cleanup completed.");
            Ensure((await contenidoService.ReadAllAsync()).Count == 0, "Contenido seed cleanup completed.");
            Ensure((await notificacionService.ReadAllAsync()).Count == 0, "Notificacion seed cleanup completed.");
            Ensure((await accionSistemaService.ReadAllAsync()).Count == 0, "AccionSistema seed cleanup completed.");
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
