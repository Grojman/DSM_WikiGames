using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;
using WikiGames.Infrastructure.Utils.Manual;

namespace WikiGames.Tests.Manual;

/// <summary>
/// Pruebas de las operaciones Custom / customized de Services/manual, de los ReadFilter
/// modificados y del servicio externo de correo.
/// </summary>
public sealed class CustomOperationTests
{
    // ======================= Contenido.newCustomized =======================

    [Fact]
    public async Task ContenidoNewCustomized_DatosValidos_MeGustaACeroYFechaActual()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario autor = await s.CrearUsuarioAsync("Autor");

        Contenido c = await new ContenidoService(s.UnitOfWork).NewCustomizedAsync("Hola mundo", autor.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Contenido leido = (await uow.ContenidoRepository.ReadAsync(c.Id))!;
        Assert.Equal("Hola mundo", leido.Texto);
        Assert.Equal(0, leido.NumeroMeGusta);
        Assert.Equal(DateTime.Today, leido.FechaCreacion.Date);
        Assert.Equal(autor.Id, leido.UsuarioCreador.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ContenidoNewCustomized_SinTexto_LanzaExcepcion(string texto)
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario autor = await s.CrearUsuarioAsync("Autor");

        await Assert.ThrowsAsync<ModelException>(() => new ContenidoService(s.UnitOfWork).NewCustomizedAsync(texto, autor.Id));
    }

    [Fact]
    public async Task ContenidoNewCustomized_AutorInexistente_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => new ContenidoService(s.UnitOfWork).NewCustomizedAsync("Texto", 999));
    }

    // ======================= Contenido.retirarMeGusta =======================

    [Fact]
    public async Task RetirarMeGusta_ConMeGustaPrevio_QuitaRelacionYDecrementa()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id);

        await new ContenidoService(s.UnitOfWork).RetirarMeGustaAsync(v.Id, fan.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Contenido leido = (await uow.ContenidoRepository.ReadAsync(v.Id))!;
        Assert.Equal(0, leido.NumeroMeGusta);
        Assert.DoesNotContain(leido.UsuariosMeGusta, u => u.Id == fan.Id);
    }

    [Fact]
    public async Task RetirarMeGusta_SinMeGustaPrevio_LanzaExcepcionYNoCambiaContador()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Usuario otro = await s.CrearUsuarioAsync("Otro");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id);

        await Assert.ThrowsAsync<ModelException>(() => new ContenidoService(s.UnitOfWork).RetirarMeGustaAsync(v.Id, otro.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(1, (await uow.ContenidoRepository.ReadAsync(v.Id))!.NumeroMeGusta);
    }

    [Fact]
    public async Task RetirarMeGusta_DespuesSePuedeVolverADarMeGusta()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Videojuego v = await s.CrearVideojuegoAsync(dueno);
        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id);
        await new ContenidoService(s.UnitOfWork).RetirarMeGustaAsync(v.Id, fan.Id);

        await s.ContenidoUseCase.DarMeGustaAsync(v.Id, fan.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(1, (await uow.ContenidoRepository.ReadAsync(v.Id))!.NumeroMeGusta);
    }

    // ======================= Denuncia.rechazarDenuncia =======================

    [Fact]
    public async Task RechazarDenuncia_Pendiente_PasaARechazadaSinNotificar()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario denunciante = await s.CrearUsuarioAsync("Denunciante");
        Denuncia d = await s.CrearDenunciaAsync(denunciante, EstadoContenido.Pendiente);

        await new DenunciaService(s.UnitOfWork).RechazarDenunciaAsync(d.Id);

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(EstadoContenido.Rechazado, (await uow.DenunciaRepository.ReadAsync(d.Id))!.Estado);
        Assert.Empty(await s.LeerNotificacionesAsync());
        Assert.Empty(s.Email.Enviados);
    }

    [Theory]
    [InlineData(EstadoContenido.Aprobado)]
    [InlineData(EstadoContenido.Rechazado)]
    public async Task RechazarDenuncia_NoPendiente_LanzaExcepcionYMantieneEstado(EstadoContenido estadoInicial)
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario denunciante = await s.CrearUsuarioAsync("Denunciante");
        Denuncia d = await s.CrearDenunciaAsync(denunciante, estadoInicial);

        await Assert.ThrowsAsync<ModelException>(() => new DenunciaService(s.UnitOfWork).RechazarDenunciaAsync(d.Id));

        await using IUnitOfWork uow = s.NuevaSesion();
        Assert.Equal(estadoInicial, (await uow.DenunciaRepository.ReadAsync(d.Id))!.Estado);
    }

    [Fact]
    public async Task RechazarDenuncia_Inexistente_LanzaExcepcion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();

        await Assert.ThrowsAsync<ModelException>(() => new DenunciaService(s.UnitOfWork).RechazarDenunciaAsync(1234));
    }

    // ======================= ReadFilter modificados =======================

    [Fact]
    public async Task FindBYUserLiked_DevuelveSoloLosContenidosQueLeGustanAlUsuario()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        Usuario otro = await s.CrearUsuarioAsync("Otro");
        Videojuego gustado = await s.CrearVideojuegoAsync(dueno, "Gustado");
        Videojuego ajeno = await s.CrearVideojuegoAsync(dueno, "Ajeno");
        await s.ContenidoUseCase.DarMeGustaAsync(gustado.Id, fan.Id);
        await s.ContenidoUseCase.DarMeGustaAsync(ajeno.Id, otro.Id);

        IReadOnlyList<Contenido> resultado = await new ContenidoService(s.UnitOfWork).FindBYUserLikedAsync(fan.Id);

        Assert.Equal(new[] { gustado.Id }, resultado.Select(c => c.Id).ToArray());
    }

    [Fact]
    public async Task FindBYUserLiked_UsuarioSinMeGusta_DevuelveVacio()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario dueno = await s.CrearUsuarioAsync("Dueno");
        Usuario fan = await s.CrearUsuarioAsync("Fan");
        await s.CrearVideojuegoAsync(dueno);

        Assert.Empty(await new ContenidoService(s.UnitOfWork).FindBYUserLikedAsync(fan.Id));
    }

    [Fact]
    public async Task FindPopulares_OrdenaPorTemasYValoracionesDeLaUltimaSemana()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario u = await s.CrearUsuarioAsync("Usuario");
        Videojuego muyActivo = await s.CrearVideojuegoAsync(u, "MuyActivo");
        Videojuego activo = await s.CrearVideojuegoAsync(u, "Activo");
        Videojuego antiguo = await s.CrearVideojuegoAsync(u, "Antiguo");

        await s.CrearTemaAsync(muyActivo, u);
        await s.CrearTemaAsync(muyActivo, u);
        await s.ValoracionUseCase.NewAsync("ok", 5f, u.Id, muyActivo.Id);   // 3 recientes
        await s.CrearTemaAsync(activo, u);                                   // 1 reciente
        await s.CrearTemaAsync(antiguo, u, DateTime.Now.AddDays(-30));
        await s.CrearTemaAsync(antiguo, u, DateTime.Now.AddDays(-20));       // 0 recientes

        IReadOnlyList<Videojuego> resultado = await new VideojuegoService(s.UnitOfWork).FindPopularesAsync();

        Assert.Equal(new[] { muyActivo.Id, activo.Id, antiguo.Id }, resultado.Select(v => v.Id).ToArray());
    }

    [Fact]
    public async Task FindByTipoUsuario_DevuelveSoloUsuariosDeEseRol()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario mod = await s.CrearUsuarioAsync("Mod", Rol.Moderador);
        await s.CrearUsuarioAsync("Normal", Rol.Registrado);
        await s.CrearUsuarioAsync("Admin", Rol.Administrador);

        IReadOnlyList<Usuario> resultado = await new UsuarioService(s.UnitOfWork).FindByTipoUsuarioAsync(Rol.Moderador);

        Assert.Equal(new[] { mod.Id }, resultado.Select(x => x.Id).ToArray());
    }

    // ======================= Servicio externo de correo (SMTP) =======================

    [Fact]
    public void ExternalUtilRegistration_RegistraEmailServiceConConfiguracionSmtp()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Smtp:Host"] = "smtp.prueba.local",
                ["Smtp:Port"] = "2525",
                ["Smtp:EnableSsl"] = "false",
            })
            .Build();
        var services = new ServiceCollection();

        ExternalUtilRegistration.Register(services, configuration);
        using ServiceProvider provider = services.BuildServiceProvider();

        Assert.IsType<EmailService>(provider.GetRequiredService<IEmailService>());
        SmtpSettings settings = provider.GetRequiredService<SmtpSettings>();
        Assert.Equal("smtp.prueba.local", settings.Host);
        Assert.Equal(2525, settings.Port);
        Assert.False(settings.EnableSsl);
        Assert.Equal("wikigames@example.com", settings.FromAddress); // valor de ejemplo por defecto
    }

    [Fact]
    public void SmtpSettings_SinConfiguracion_UsaValoresDeEjemplo()
    {
        SmtpSettings settings = SmtpSettings.FromConfiguration(null);

        Assert.Equal("smtp.example.com", settings.Host);
        Assert.Equal(587, settings.Port);
        Assert.True(settings.EnableSsl);
    }

    [Fact]
    public async Task EmailService_DestinatarioVacio_DevuelveFalse()
    {
        var service = new EmailService(new SmtpSettings());

        Assert.False(await service.SendEmailAsync("", "Asunto", "Cuerpo"));
    }

    [Fact]
    public async Task EmailService_ServidorInaccesible_DevuelveFalseSinLanzarExcepcion()
    {
        var service = new EmailService(new SmtpSettings { Host = "127.0.0.1", Port = 1, EnableSsl = false });

        bool enviado = await service.SendEmailAsync("alguien@wikigames.test", "Asunto", "Cuerpo");

        Assert.False(enviado);
    }

    [Fact]
    public async Task NotificacionNew_SinUtils_NoEnviaCorreoPeroCreaLaNotificacion()
    {
        await using var s = await WikiGamesScenario.CreateAsync();
        Usuario ana = await s.CrearUsuarioAsync("Ana");
        var useCase = new Application.UseCases.Notificacion.NotificacionUseCase(s.UnitOfWork);

        await useCase.NewAsync("Sin correo", DateTime.Now, new[] { ana.Id });

        Assert.Single(await s.LeerNotificacionesAsync());
        Assert.Empty(s.Email.Enviados);
    }
}
