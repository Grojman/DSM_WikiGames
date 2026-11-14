// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(UsuarioController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;
using WikiGames.Web.Utils.Security;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

/*PROTECTED REGION ID(UsuarioController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class UsuarioController : Controller
{
    /*PROTECTED REGION ID(UsuarioController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly UsuarioService _usuarioService;
    private readonly ILogger<UsuarioController> _logger;
    public UsuarioController(UsuarioService usuarioService, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<UsuarioController>? logger = null)
    {
        _usuarioService = usuarioService;


        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<UsuarioController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(UsuarioController_Index_GET_body) START*/

        var entities = await _usuarioService.ReadAllAsync();
        return View(entities.Select(UsuarioAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(UsuarioController_Details_GET_body) START*/

        var entity = await _usuarioService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(UsuarioAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_Register_GET_attributes) START*/
    [AllowAnonymous]
    /*PROTECTED REGION END*/
    public IActionResult Register()
    {
    /*PROTECTED REGION ID(UsuarioController_Register_GET_body) START*/

        return View(new WikiGames.Web.ViewModels.UsuarioViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_Register_POST_attributes) START*/
    [AllowAnonymous]
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Register(WikiGames.Web.ViewModels.UsuarioViewModel model)
    {
    /*PROTECTED REGION ID(UsuarioController_Register_POST_body) START*/


        if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(model.Password), "Debes indicar una contraseña.");
        if (!ModelState.IsValid) return View(model);
        Usuario createdUser;
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new global::WikiGames.Application.UseCases.Usuario.UsuarioUseCase(unitOfWork);
        try
        {
            createdUser = await useCase.NewAsync(model.Name, model.Email, model.Password!, model.RolUsuario);
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not create the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }
        try
        {
            await HariaWebAuthentication.SignInAsync(HttpContext, "Usuario", createdUser.Id, createdUser);
            HttpContext.Session.SetUser(UsuarioAssembler.ToViewModel(createdUser));
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "La cuenta se ha creado, pero no se ha podido iniciar la sesión.");
            ModelState.AddModelError(string.Empty, "Tu cuenta ya está creada. Inicia sesión desde la página de acceso.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(UsuarioController_Edit_GET_body) START*/

        var entity = await _usuarioService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(UsuarioAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.Web.ViewModels.UsuarioViewModel model)
    {
    /*PROTECTED REGION ID(UsuarioController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);
        var stored = await _usuarioService.ReadOIDAsync(id);
        if (stored is null) return NotFound();
        try
        {
            await _usuarioService.ModifyAsync(id, model.Name, model.Email, (string.IsNullOrEmpty(model.Password) ? stored.Password : model.Password), model.RolUsuario);
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not update the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_AddDenunciasRecibida_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenunciasRecibida(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
    /*PROTECTED REGION ID(UsuarioController_AddDenunciasRecibida_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _usuarioService.AddDenunciasRecibidaAsync(p_oid, p_denuncia_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_AddContenidoFavorito_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddContenidoFavorito(int p_oid, IReadOnlyList<int> p_contenido_oids)
    {
    /*PROTECTED REGION ID(UsuarioController_AddContenidoFavorito_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _usuarioService.AddContenidoFavoritoAsync(p_oid, p_contenido_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_AddNotificacione_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddNotificacione(int p_oid, IReadOnlyList<int> p_notificacion_oids)
    {
    /*PROTECTED REGION ID(UsuarioController_AddNotificacione_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _usuarioService.AddNotificacioneAsync(p_oid, p_notificacion_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_FindByNameLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByNameLinQ(string p_name)
    {
    /*PROTECTED REGION ID(UsuarioController_FindByNameLinQ_GET_body) START*/

        var result = await _usuarioService.FindByNameLinQAsync(p_name);
        return Ok(result.Select(UsuarioAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_FindByTipoUsuario_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByTipoUsuario(Rol p_rol)
    {
    /*PROTECTED REGION ID(UsuarioController_FindByTipoUsuario_GET_body) START*/

        var result = await _usuarioService.FindByTipoUsuarioAsync(p_rol);
        return Ok(result.Select(UsuarioAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
