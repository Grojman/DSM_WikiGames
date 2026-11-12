using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Tests.Manual;

/// <summary>
/// Pruebas de las CustomTransaction y customizedTransaction (UseCases/manual).
/// Estructura AAA y nombres Metodo_Escenario_ResultadoEsperado (Tema 7).
/// El estado final se comprueba siempre desde una sesión nueva.
/// </summary>
public sealed class UseCaseTransactionTests
{
    // ======================= Notificacion.new =======================

    [Fact]
    public async Task NotificacionNew_ConUsuario_PersisteRelacionaYEnviaCorreo()
    {
        // Arrange
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario ana = await s.CrearUsuarioAsync("Ana");

        // Act
        Notificacion n = await s.NotificacionUseCase.NewAsync("Hola Ana", DateTime.Now, new[] { ana.Id });

        // Assert
        var leidas = await s.LeerNotificacionesAsync();
        var leida = Assert.Single(leidas);
        Assert.Equal(n.Id, leida.Id);
        Assert.Equal("Hola Ana", leida.Texto);
        Assert.Contains(ana.Id, leida.Notificados);

        var correo = Assert.Single(s.Email.Enviados);
        Assert.Equal(ana.Email, correo.Para);
        Assert.Contains("Hola Ana", correo.Cuerpo);
    }

    [Fact]
    public async Task NotificacionNew_SinNotificados_LanzaExcepcionYNoPersiste()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() =>
            s.NotificacionUseCase.NewAsync("Texto", DateTime.Now, Array.Empty<int>()));

        Assert.Empty(await s.LeerNotificacionesAsync());
        Assert.Empty(s.Email.Enviados);
    }

    [Fact]
    public async Task NotificacionNew_SinTexto_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario ana = await s.CrearUsuarioAsync("Ana");

        await Assert.ThrowsAsync<ModelException>(() =>
            s.NotificacionUseCase.NewAsync("  ", DateTime.Now, new[] { ana.Id }));

        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task NotificacionNew_UsuarioInexistente_HaceRollbackYNoEnviaCorreo()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario ana = await s.CrearUsuarioAsync("Ana");

        await Assert.ThrowsAsync<ModelException>(() =>
            s.NotificacionUseCase.NewAsync("Texto", DateTime.Now, new[] { ana.Id, 999999 }));

        Assert.Empty(await s.LeerNotificacionesAsync());
        Assert.Empty(s.Email.Enviados);
    }

    // ======================= Usuario =======================

    [Fact]
    public async Task UsuarioNew_DatosValidos_CreaUsuarioYNotificaBienvenida()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        Usuario nuevo = await s.UsuarioUseCase.NewAsync("Luis", "luis@wikigames.test", "secreto", Rol.Registrado);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Usuario? leido = await uow.UsuarioRepository.ReadAsync(nuevo.Id);
            Assert.NotNull(leido);
            Assert.Equal("Luis", leido!.Name);
            Assert.NotEqual("secreto", leido.Password); // se guarda con hash
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("Bienvenido", notificacion.Texto);
        Assert.Contains(nuevo.Id, notificacion.Notificados);
        Assert.Equal("luis@wikigames.test", Assert.Single(s.Email.Enviados).Para);
    }

    [Fact]
    public async Task UsuarioNew_NombreVacio_LanzaExcepcionYNoPersiste()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() =>
            s.UsuarioUseCase.NewAsync("", "x@wikigames.test", "secreto", Rol.Registrado));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty(await uow.UsuarioRepository.ReadAllAsync());
        Assert.Empty(s.Email.Enviados);
    }

    [Fact]
    public async Task UsuarioDestroy_UsuarioExistente_EnviaAvisoYBorraUsuario()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario ana = await s.CrearUsuarioAsync("Ana");

        await s.UsuarioUseCase.DestroyAsync(ana.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Assert.Null(await uow.UsuarioRepository.ReadAsync(ana.Id));
        }

        // La notificación se crea antes del borrado y queda huérfana (aceptado); el correo sí se envía.
        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("eliminada", notificacion.Texto);
        var correo = Assert.Single(s.Email.Enviados);
        Assert.Equal(ana.Email, correo.Para);
    }

    [Fact]
    public async Task UsuarioDestroy_UsuarioInexistente_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => s.UsuarioUseCase.DestroyAsync(12345));

        Assert.Empty(s.Email.Enviados);
    }

    // ======================= Videojuego =======================

    [Fact]
    public async Task VideojuegoNewCustomized_DatosValidos_QuedaPendienteConMediaCeroYNotificaDueno()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");

        Videojuego v = await s.VideojuegoUseCase.NewCustomizedAsync("Texto", "Titulo", "https://juego", "img.png", dueno.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Videojuego leido = (await uow.VideojuegoRepository.ReadAsync(v.Id))!;
            Assert.Equal(EstadoContenido.Pendiente, leido.Estado);
            Assert.Equal(0, leido.NumeroMeGusta);
            Assert.Equal(0f, leido.PuntuacionMedia);
            Assert.Equal(DateTime.Today, leido.FechaCreacion.Date);
            Assert.Equal(dueno.Id, leido.UsuarioCreador.Id);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("esperar", notificacion.Texto);
        Assert.Contains(dueno.Id, notificacion.Notificados);
    }

    [Fact]
    public async Task VideojuegoNewCustomized_SinTitulo_LanzaExcepcionYNoPersiste()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");

        await Assert.ThrowsAsync<ModelException>(() =>
            s.VideojuegoUseCase.NewCustomizedAsync("Texto", " ", "https://juego", "img.png", dueno.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty(await uow.VideojuegoRepository.ReadAllAsync());
        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task AprobarVideojuego_Pendiente_PasaAAprobadoYNotificaDueno()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);

        await s.VideojuegoUseCase.AprobarVideojuegoAsync(v.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Assert.Equal(EstadoContenido.Aprobado, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.Estado);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("Enhorabuena", notificacion.Texto);
        Assert.Contains(dueno.Id, notificacion.Notificados);
    }

    [Theory]
    [InlineData(EstadoContenido.Aprobado)]
    [InlineData(EstadoContenido.Rechazado)]
    public async Task AprobarVideojuego_NoPendiente_LanzaExcepcionYNoCambiaEstado(EstadoContenido estadoInicial)
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Videojuego v = await s.CrearVideojuegoAsync(dueno, estado: estadoInicial);

        await Assert.ThrowsAsync<ModelException>(() => s.VideojuegoUseCase.AprobarVideojuegoAsync(v.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(estadoInicial, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.Estado);
        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task RechazarVideojuego_Pendiente_CreaRechazoPasaARechazadoYNotifica()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario moderador = await s.CrearUsuarioAsync("Moderador", Rol.Moderador);
        Videojuego v = await s.CrearVideojuegoAsync(dueno);

        await s.VideojuegoUseCase.RechazarVideojuegoAsync(v.Id, "Contenido inapropiado", moderador.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Videojuego leido = (await uow.VideojuegoRepository.ReadAsync(v.Id))!;
            Assert.Equal(EstadoContenido.Rechazado, leido.Estado);

            Rechazo rechazo = Assert.Single(await uow.RechazoRepository.ReadAllAsync());
            Assert.Equal("Contenido inapropiado", rechazo.Motivo);
            Assert.Equal(v.Id, rechazo.Videojuego.Id);
            Assert.Equal(moderador.Id, rechazo.Gestor.Id);
            Assert.Equal(rechazo.Id, leido.Rechazo!.Id);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("rechazado", notificacion.Texto);
        Assert.Contains("Contenido inapropiado", notificacion.Texto);
        Assert.Contains(dueno.Id, notificacion.Notificados);
    }

    [Fact]
    public async Task RechazarVideojuego_NoPendiente_LanzaExcepcionSinCrearRechazo()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario moderador = await s.CrearUsuarioAsync("Moderador", Rol.Moderador);
        Videojuego v = await s.CrearVideojuegoAsync(dueno, estado: EstadoContenido.Aprobado);

        await Assert.ThrowsAsync<ModelException>(() =>
            s.VideojuegoUseCase.RechazarVideojuegoAsync(v.Id, "Motivo", moderador.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(EstadoContenido.Aprobado, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.Estado);
        Assert.Empty(await uow.RechazoRepository.ReadAllAsync());
    }

    [Fact]
    public async Task RechazarVideojuego_GestorInexistente_HaceRollbackYSigueSiendoPendiente()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);

        await Assert.ThrowsAsync<ModelException>(() =>
            s.VideojuegoUseCase.RechazarVideojuegoAsync(v.Id, "Motivo", 999999));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(EstadoContenido.Pendiente, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.Estado);
        Assert.Empty(await uow.RechazoRepository.ReadAllAsync());
        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task RechazarVideojuego_SinMotivo_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario moderador = await s.CrearUsuarioAsync("Moderador", Rol.Moderador);
        Videojuego v = await s.CrearVideojuegoAsync(dueno);

        await Assert.ThrowsAsync<ModelException>(() =>
            s.VideojuegoUseCase.RechazarVideojuegoAsync(v.Id, "", moderador.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(EstadoContenido.Pendiente, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.Estado);
    }

    // ======================= Categoria.crearSubcategoria =======================

    [Fact]
    public async Task CrearSubcategoria_NombreNuevo_CreaCategoriaHijaDelPadre()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        var padre = await new Application.Services.CategoriaService(s.UnitOfWork).NewAsync("Acción");

        Categoria hija = await s.CategoriaUseCase.CrearSubcategoriaAsync(padre.Id, "Shooter");

        await using IUnitOfWork uow = s.NuevaSesion();
        Categoria hijaLeida = (await uow.CategoriaRepository.ReadAsync(hija.Id))!;
        Assert.Equal("Shooter", hijaLeida.Name);
        Assert.Equal(padre.Id, hijaLeida.Padre!.Id);
        Categoria padreLeido = (await uow.CategoriaRepository.ReadAsync(padre.Id))!;
        Assert.Contains(padreLeido.Hijos, h => h.Id == hija.Id);
    }

    [Theory]
    [InlineData("Shooter")]
    [InlineData("shooter ")]
    public async Task CrearSubcategoria_NombreRepetidoEnMismoPadre_LanzaExcepcionYNoCrea(string nombreRepetido)
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        var padre = await new Application.Services.CategoriaService(s.UnitOfWork).NewAsync("Acción");
        await s.CategoriaUseCase.CrearSubcategoriaAsync(padre.Id, "Shooter");

        await Assert.ThrowsAsync<ModelException>(() =>
            s.CategoriaUseCase.CrearSubcategoriaAsync(padre.Id, nombreRepetido));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(2, (await uow.CategoriaRepository.ReadAllAsync()).Count); // padre + una hija
    }

    [Fact]
    public async Task CrearSubcategoria_MismoNombreEnOtroPadre_SePermite()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        var service = new Application.Services.CategoriaService(s.UnitOfWork);
        var accion = await service.NewAsync("Acción");
        var deportes = await service.NewAsync("Deportes");
        await s.CategoriaUseCase.CrearSubcategoriaAsync(accion.Id, "Multijugador");

        Categoria otra = await s.CategoriaUseCase.CrearSubcategoriaAsync(deportes.Id, "Multijugador");

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(deportes.Id, (await uow.CategoriaRepository.ReadAsync(otra.Id))!.Padre!.Id);
        Assert.Equal(2, (await uow.CategoriaRepository.FindByNameLinQAsync("Multijugador")).Count);
    }

    [Fact]
    public async Task CrearSubcategoria_PadreInexistente_LanzaExcepcionYNoCrea()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => s.CategoriaUseCase.CrearSubcategoriaAsync(4242, "Huérfana"));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty(await uow.CategoriaRepository.ReadAllAsync());
    }

    // ======================= Comentario.addRespuestaTransaction =======================

    [Fact]
    public async Task AddRespuestaTransaction_Valida_RelacionaRespuestaYNotificaAutor()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario autor = await s.CrearUsuarioAsync("Autor");
        Usuario otro = await s.CrearUsuarioAsync("Otro");
        Videojuego v = await s.CrearVideojuegoAsync(autor);
        TemaConversacion tema = await s.CrearTemaAsync(v, autor);
        Comentario original = await s.CrearComentarioAsync(tema, autor, "Original");
        Comentario respuesta = await s.CrearComentarioAsync(tema, otro, "Respuesta");

        await s.ComentarioUseCase.AddRespuestaTransactionAsync(original.Id, new[] { respuesta.Id });

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Comentario leida = (await uow.ComentarioRepository.ReadAsync(respuesta.Id))!;
            Assert.Equal(original.Id, leida.ComentarioAsociado!.Id);
            Comentario originalLeido = (await uow.ComentarioRepository.ReadAsync(original.Id))!;
            Assert.Contains(originalLeido.Respuestas, r => r.Id == respuesta.Id);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("respondido", notificacion.Texto);
        Assert.Contains(autor.Id, notificacion.Notificados);
        Assert.Equal(autor.Email, Assert.Single(s.Email.Enviados).Para);
    }

    [Fact]
    public async Task AddRespuestaTransaction_SinRespuestas_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario autor = await s.CrearUsuarioAsync("Autor");
        Videojuego v = await s.CrearVideojuegoAsync(autor);
        Comentario original = await s.CrearComentarioAsync(await s.CrearTemaAsync(v, autor), autor);

        await Assert.ThrowsAsync<ModelException>(() =>
            s.ComentarioUseCase.AddRespuestaTransactionAsync(original.Id, Array.Empty<int>()));

        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task AddRespuestaTransaction_RespuestaInexistente_HaceRollback()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario autor = await s.CrearUsuarioAsync("Autor");
        Videojuego v = await s.CrearVideojuegoAsync(autor);
        Comentario original = await s.CrearComentarioAsync(await s.CrearTemaAsync(v, autor), autor);

        await Assert.ThrowsAsync<ModelException>(() =>
            s.ComentarioUseCase.AddRespuestaTransactionAsync(original.Id, new[] { 777777 }));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty((await uow.ComentarioRepository.ReadAsync(original.Id))!.Respuestas);
        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    // ======================= Denuncia =======================

    [Fact]
    public async Task DenunciaNewCustomized_Valida_QuedaPendienteYNotificaDenunciante()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario denunciante = await s.CrearUsuarioAsync("Denunciante");

        Denuncia d = await s.DenunciaUseCase.NewCustomizedAsync(denunciante.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Denuncia leida = (await uow.DenunciaRepository.ReadAsync(d.Id))!;
            Assert.Equal(EstadoContenido.Pendiente, leida.Estado);
            Assert.Equal(DateTime.Today, leida.Fecha.Date);
            Assert.Equal(denunciante.Id, leida.Denunciante.Id);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("recibido tu denuncia", notificacion.Texto);
        Assert.Contains(denunciante.Id, notificacion.Notificados);
    }

    [Fact]
    public async Task DenunciaNewCustomized_DenuncianteInexistente_LanzaExcepcionYNoPersiste()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => s.DenunciaUseCase.NewCustomizedAsync(31337));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty(await uow.DenunciaRepository.ReadAllAsync());
    }

    [Fact]
    public async Task AprobarDenuncia_Pendiente_PasaAAprobadaYNotificaDenunciante()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario denunciante = await s.CrearUsuarioAsync("Denunciante");
        Denuncia d = await s.CrearDenunciaAsync(denunciante, EstadoContenido.Pendiente);

        await s.DenunciaUseCase.AprobarDenunciaAsync(d.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Assert.Equal(EstadoContenido.Aprobado, (await uow.DenunciaRepository.ReadAsync(d.Id))!.Estado);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("aceptada", notificacion.Texto);
        Assert.Contains(denunciante.Id, notificacion.Notificados);
    }

    [Theory]
    [InlineData(EstadoContenido.Aprobado)]
    [InlineData(EstadoContenido.Rechazado)]
    public async Task AprobarDenuncia_NoPendiente_LanzaExcepcionYNoCambiaEstado(EstadoContenido estadoInicial)
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario denunciante = await s.CrearUsuarioAsync("Denunciante");
        Denuncia d = await s.CrearDenunciaAsync(denunciante, estadoInicial);

        await Assert.ThrowsAsync<ModelException>(() => s.DenunciaUseCase.AprobarDenunciaAsync(d.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(estadoInicial, (await uow.DenunciaRepository.ReadAsync(d.Id))!.Estado);
        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    // ======================= Valoracion =======================

    [Fact]
    public async Task ValoracionNew_DosValoraciones_ActualizaMediaYNotificaDueno()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario critico = await s.CrearUsuarioAsync("Critico");
        Videojuego v = await s.CrearVideojuegoAsync(dueno, estado: EstadoContenido.Aprobado);

        Valoracion primera = await s.ValoracionUseCase.NewAsync("Bien", 4f, critico.Id, v.Id);
        await s.ValoracionUseCase.NewAsync("Muy bien", 8f, critico.Id, v.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Videojuego leido = (await uow.VideojuegoRepository.ReadAsync(v.Id))!;
            Assert.Equal(6f, leido.PuntuacionMedia, 3);
            Assert.Equal(2, leido.Valoraciones.Count);

            Valoracion valoracion = (await uow.ValoracionRepository.ReadAsync(primera.Id))!;
            Assert.Equal(0, valoracion.NumeroMeGusta);
            Assert.Equal(DateTime.Today, valoracion.FechaCreacion.Date);
        }

        var notificaciones = await s.LeerNotificacionesAsync();
        Assert.Equal(2, notificaciones.Count);
        Assert.All(notificaciones, n => Assert.Contains("nueva valoración", n.Texto));
        Assert.All(s.Email.Enviados, c => Assert.Equal(dueno.Email, c.Para));
    }

    [Fact]
    public async Task ValoracionNew_VideojuegoInexistente_LanzaExcepcionYNoPersiste()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario critico = await s.CrearUsuarioAsync("Critico");

        await Assert.ThrowsAsync<ModelException>(() => s.ValoracionUseCase.NewAsync("x", 5f, critico.Id, 4040));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty(await uow.ValoracionRepository.ReadAllAsync());
        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task ActualizarValoracion_NuevoValor_CambiaPuntuacionYRecalculaMedia()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario critico = await s.CrearUsuarioAsync("Critico");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        Valoracion a = await s.ValoracionUseCase.NewAsync("A", 2f, critico.Id, v.Id);
        await s.ValoracionUseCase.NewAsync("B", 6f, critico.Id, v.Id);

        await s.ValoracionUseCase.ActualizarValoracionAsync(a.Id, 10f);

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(10f, (await uow.ValoracionRepository.ReadAsync(a.Id))!.Puntuacion);
        Assert.Equal(8f, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.PuntuacionMedia, 3);
    }

    [Fact]
    public async Task ActualizarValoracion_Inexistente_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => s.ValoracionUseCase.ActualizarValoracionAsync(5555, 3f));
    }

    [Fact]
    public async Task EliminarValoracion_UnaDeDos_BorraYRecalculaMedia()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario critico = await s.CrearUsuarioAsync("Critico");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        Valoracion a = await s.ValoracionUseCase.NewAsync("A", 2f, critico.Id, v.Id);
        await s.ValoracionUseCase.NewAsync("B", 6f, critico.Id, v.Id);

        await s.ValoracionUseCase.EliminarValoracionAsync(a.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Null(await uow.ValoracionRepository.ReadAsync(a.Id));
        Assert.Equal(6f, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.PuntuacionMedia, 3);
    }

    [Fact]
    public async Task EliminarValoracion_LaUnica_DejaMediaACero()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario critico = await s.CrearUsuarioAsync("Critico");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        Valoracion a = await s.ValoracionUseCase.NewAsync("A", 7f, critico.Id, v.Id);

        await s.ValoracionUseCase.EliminarValoracionAsync(a.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Empty(await uow.ValoracionRepository.ReadAllAsync());
        Assert.Equal(0f, (await uow.VideojuegoRepository.ReadAsync(v.Id))!.PuntuacionMedia);
    }

    // ======================= Contenido =======================

    [Fact]
    public async Task ContenidoDestroy_ContenidoExistente_LoBorraYNotificaDueno()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        var contenido = await new Application.Services.ContenidoService(s.UnitOfWork).NewCustomizedAsync("Spam", dueno.Id);

        await s.ContenidoUseCase.DestroyAsync(contenido.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Assert.Null(await uow.ContenidoRepository.ReadAsync(contenido.Id));
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("eliminado por un moderador", notificacion.Texto);
        Assert.Contains(dueno.Id, notificacion.Notificados);
    }

    [Fact]
    public async Task ContenidoDestroy_Inexistente_LanzaExcepcionSinNotificar()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => s.ContenidoUseCase.DestroyAsync(8080));

        Assert.Empty(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task DarMeGusta_PrimeraVez_RelacionaIncrementaYNotificaDueno()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);

        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id);

        await using (IUnitOfWork uow = s.NuevaSesion())
        {
            Contenido leido = (await uow.ContenidoRepository.ReadAsync(v.Id))!;
            Assert.Equal(1, leido.NumeroMeGusta);
            Assert.Contains(leido.UsuariosMeGusta, u => u.Id == fan.Id);
        }

        var notificacion = Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Contains("Fan", notificacion.Texto);
        Assert.Contains(dueno.Id, notificacion.Notificados);
    }

    [Fact]
    public async Task DarMeGusta_Repetido_LanzaExcepcionYNoIncrementa()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id);

        await Assert.ThrowsAsync<ModelException>(() => s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(1, (await uow.ContenidoRepository.ReadAsync(v.Id))!.NumeroMeGusta);
        Assert.Single(await s.LeerNotificacionesAsync());
    }

    [Fact]
    public async Task DarMeGusta_DosUsuariosDistintos_CuentaDosMeGusta()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan1 = await s.CrearUsuarioAsync("Fan1");
        Usuario fan2 = await s.CrearUsuarioAsync("Fan2");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);

        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan1.Id);
        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan2.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Contenido leido = (await uow.ContenidoRepository.ReadAsync(v.Id))!;
        Assert.Equal(2, leido.NumeroMeGusta);
        Assert.Equal(2, leido.UsuariosMeGusta.Count);
    }

    [Fact]
    public async Task DarMeGusta_UnUsuarioADosContenidos_ConservaAmbosMeGusta()
    {
        // Un usuario debe poder tener varios "me gusta" (asociación UsuarioMeGusta 0..* – 0..*).
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Videojuego v1 = await s.CrearVideojuegoAsync(dueno, "Juego1");
        Videojuego v2 = await s.CrearVideojuegoAsync(dueno, "Juego2");

        await s.ContenidoUseCase.DarMeGustaAsync(v1.Id, fan.Id);
        await s.ContenidoUseCase.DarMeGustaAsync(v2.Id, fan.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Contains((await uow.ContenidoRepository.ReadAsync(v1.Id))!.UsuariosMeGusta, u => u.Id == fan.Id);
        Assert.Contains((await uow.ContenidoRepository.ReadAsync(v2.Id))!.UsuariosMeGusta, u => u.Id == fan.Id);
    }
}
