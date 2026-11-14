// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ValoracionController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

/*PROTECTED REGION ID(ValoracionController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ValoracionController : Controller
{
    /*PROTECTED REGION ID(ValoracionController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly ValoracionService _valoracionService;
    private readonly ContenidoService _contenidoService;
    private readonly ILogger<ValoracionController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public ValoracionController(ValoracionService valoracionService, ContenidoService contenidoService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<ValoracionController>? logger = null)
    {
        _valoracionService = valoracionService;
        _contenidoService = contenidoService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ValoracionController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ValoracionController_Index_GET_body) START*/

        var entities = await _valoracionService.ReadAllAsync();
        return View(entities.Select(ValoracionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ValoracionController_Details_GET_body) START*/

        var entity = await _valoracionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ValoracionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ValoracionController_Edit_GET_body) START*/

        var entity = await _valoracionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = ValoracionAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.Web.ViewModels.ValoracionViewModel model)
    {
    /*PROTECTED REGION ID(ValoracionController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.ValoracionService(unitOfWork);
        var entity = await service.ReadOIDValoracionDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.VideojuegoId is int selectedVideojuego &&
            !model.VideojuegoOptions.Any(option => option.Value == Convert.ToString(selectedVideojuego, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("VideojuegoId", "Selecciona un registro existente de Videojuego.");
        if (model.VideojuegoId is null) ModelState.AddModelError("VideojuegoId", "Debes seleccionar una opción.");
        if (model.UsuarioCreadorId is int selectedUsuarioCreador &&
            !model.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (model.UsuarioCreadorId is null) ModelState.AddModelError("UsuarioCreadorId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Valoracion.Edit");
        try
        {

            await service.ModifyAsync(id, model.Texto, model.FechaCreacion, model.NumeroMeGusta, model.Puntuacion, model.UsuarioCreadorId.GetValueOrDefault(), model.VideojuegoId.GetValueOrDefault());
            entity.Videojuego = model.VideojuegoId is int referenceVideojuego
                ? await unitOfWork.VideojuegoRepository.ReadAsync(referenceVideojuego) ?? throw new InvalidOperationException("El registro seleccionado de Videojuego ya no existe.") : null!;
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierValoracionDefaultAsync(entity);

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

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ValoracionController_Delete_GET_body) START*/

        var entity = await _valoracionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ValoracionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ValoracionController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.Web.ViewModels.ValoracionViewModel());
        await _valoracionService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindByPuntuacion_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByPuntuacion(float puntuacion)
    {
    /*PROTECTED REGION ID(ValoracionController_FindByPuntuacion_GET_body) START*/

        var result = await _valoracionService.FindByPuntuacionAsync(puntuacion);
        return Ok(result.Select(ValoracionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindPeores_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindPeores()
    {
    /*PROTECTED REGION ID(ValoracionController_FindPeores_GET_body) START*/

        var result = await _valoracionService.FindPeoresAsync();
        return Ok(result.Select(ValoracionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindMejores_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindMejores()
    {
    /*PROTECTED REGION ID(ValoracionController_FindMejores_GET_body) START*/

        var result = await _valoracionService.FindMejoresAsync();
        return Ok(result.Select(ValoracionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_AddDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenuncia(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
    /*PROTECTED REGION ID(ValoracionController_AddDenuncia_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddDenunciaAsync(p_oid, p_denuncia_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_AddUsuarioFavorito_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioFavorito(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(ValoracionController_AddUsuarioFavorito_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioFavoritoAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_AddUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(ValoracionController_AddUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_QuitarUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> QuitarUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(ValoracionController_QuitarUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.QuitarUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindByTextoLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByTextoLinQ(string p_texto)
    {
    /*PROTECTED REGION ID(ValoracionController_FindByTextoLinQ_GET_body) START*/

        var result = await _contenidoService.FindByTextoLinQAsync(p_texto);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindOrderByFecha_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderByFecha()
    {
    /*PROTECTED REGION ID(ValoracionController_FindOrderByFecha_GET_body) START*/

        var result = await _contenidoService.FindOrderByFechaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindOrderMeGusta_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderMeGusta()
    {
    /*PROTECTED REGION ID(ValoracionController_FindOrderMeGusta_GET_body) START*/

        var result = await _contenidoService.FindOrderMeGustaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_FindBYUserLiked_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindBYUserLiked(int p_user)
    {
    /*PROTECTED REGION ID(ValoracionController_FindBYUserLiked_GET_body) START*/

        var result = await _contenidoService.FindBYUserLikedAsync(p_user);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(ValoracionController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsValoracionViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.ValoracionViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ValoracionController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsValoracionViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.ValoracionViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.VideojuegoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.UsuarioRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.VideojuegoOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.UsuarioCreadorOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
