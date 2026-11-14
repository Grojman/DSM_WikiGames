// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ContenidoController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

/*PROTECTED REGION ID(ContenidoController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ContenidoController : Controller
{
    /*PROTECTED REGION ID(ContenidoController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly ContenidoService _contenidoService;
    private readonly ILogger<ContenidoController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public ContenidoController(ContenidoService contenidoService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<ContenidoController>? logger = null)
    {
        _contenidoService = contenidoService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ContenidoController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ContenidoController_Index_GET_body) START*/

        var entities = await _contenidoService.ReadAllAsync();
        return View(entities.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ContenidoController_Details_GET_body) START*/

        var entity = await _contenidoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ContenidoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ContenidoController_Edit_GET_body) START*/

        var entity = await _contenidoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = ContenidoAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.Web.ViewModels.ContenidoViewModel model)
    {
    /*PROTECTED REGION ID(ContenidoController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.ContenidoService(unitOfWork);
        var entity = await service.ReadOIDContenidoDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.UsuarioCreadorId is int selectedUsuarioCreador &&
            !model.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (model.UsuarioCreadorId is null) ModelState.AddModelError("UsuarioCreadorId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Contenido.Edit");
        try
        {

            await service.ModifyAsync(id, model.Texto, model.FechaCreacion, model.NumeroMeGusta, model.UsuarioCreadorId.GetValueOrDefault());
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierContenidoDefaultAsync(entity);

            await unitOfWork.CommitAsync();
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "Could not save the record and its lines.");
            ModelState.AddModelError(string.Empty, "No se han podido guardar el registro y sus líneas.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_AddDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenuncia(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
    /*PROTECTED REGION ID(ContenidoController_AddDenuncia_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddDenunciaAsync(p_oid, p_denuncia_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_AddUsuarioFavorito_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioFavorito(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(ContenidoController_AddUsuarioFavorito_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioFavoritoAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_AddUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(ContenidoController_AddUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_QuitarUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> QuitarUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(ContenidoController_QuitarUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.QuitarUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_FindByTextoLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByTextoLinQ(string p_texto)
    {
    /*PROTECTED REGION ID(ContenidoController_FindByTextoLinQ_GET_body) START*/

        var result = await _contenidoService.FindByTextoLinQAsync(p_texto);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_FindOrderByFecha_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderByFecha()
    {
    /*PROTECTED REGION ID(ContenidoController_FindOrderByFecha_GET_body) START*/

        var result = await _contenidoService.FindOrderByFechaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_FindOrderMeGusta_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderMeGusta()
    {
    /*PROTECTED REGION ID(ContenidoController_FindOrderMeGusta_GET_body) START*/

        var result = await _contenidoService.FindOrderMeGustaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_FindBYUserLiked_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindBYUserLiked(int p_user)
    {
    /*PROTECTED REGION ID(ContenidoController_FindBYUserLiked_GET_body) START*/

        var result = await _contenidoService.FindBYUserLikedAsync(p_user);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(ContenidoController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsContenidoViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.ContenidoViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ContenidoController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsContenidoViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.ContenidoViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.UsuarioRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.UsuarioCreadorOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
