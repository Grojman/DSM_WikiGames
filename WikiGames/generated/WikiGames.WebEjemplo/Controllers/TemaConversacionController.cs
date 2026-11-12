// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(TemaConversacionController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.WebEjemplo.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.WebEjemplo.Controllers;

/*PROTECTED REGION ID(TemaConversacionController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class TemaConversacionController : Controller
{
    /*PROTECTED REGION ID(TemaConversacionController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly TemaConversacionService _temaConversacionService;
    private readonly ContenidoService _contenidoService;
    private readonly ILogger<TemaConversacionController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public TemaConversacionController(TemaConversacionService temaConversacionService, ContenidoService contenidoService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<TemaConversacionController>? logger = null)
    {
        _temaConversacionService = temaConversacionService;
        _contenidoService = contenidoService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<TemaConversacionController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(TemaConversacionController_Index_GET_body) START*/

        var entities = await _temaConversacionService.ReadAllAsync();
        return View(entities.Select(TemaConversacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(TemaConversacionController_Details_GET_body) START*/

        var entity = await _temaConversacionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(TemaConversacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(TemaConversacionController_Create_GET_body) START*/

        var model = new WikiGames.WebEjemplo.ViewModels.TemaConversacionViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.WebEjemplo.ViewModels.TemaConversacionViewModel model)
    {
    /*PROTECTED REGION ID(TemaConversacionController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new TemaConversacionService(unitOfWork);



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

        await unitOfWork.BeginTransactionAsync("MVC.TemaConversacion.Create");
        try
        {

            var entity = await service.NewAsync(model.Texto, model.FechaCreacion, model.NumeroMeGusta, model.Titulo, model.VideojuegoId.GetValueOrDefault(), model.UsuarioCreadorId.GetValueOrDefault());
            entity.Videojuego = model.VideojuegoId is int referenceVideojuego
                ? await unitOfWork.VideojuegoRepository.ReadAsync(referenceVideojuego) ?? throw new InvalidOperationException("El registro seleccionado de Videojuego ya no existe.") : null!;
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierTemaConversacionDefaultAsync(entity);

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
    /*PROTECTED REGION ID(TemaConversacionController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(TemaConversacionController_Edit_GET_body) START*/

        var entity = await _temaConversacionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = TemaConversacionAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.WebEjemplo.ViewModels.TemaConversacionViewModel model)
    {
    /*PROTECTED REGION ID(TemaConversacionController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new TemaConversacionService(unitOfWork);
        var entity = await service.ReadOIDTemaConversacionDefaultAsync(id);
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

        await unitOfWork.BeginTransactionAsync("MVC.TemaConversacion.Edit");
        try
        {

            await service.ModifyAsync(id, model.Texto, model.FechaCreacion, model.NumeroMeGusta, model.Titulo, model.VideojuegoId.GetValueOrDefault(), model.UsuarioCreadorId.GetValueOrDefault());
            entity.Videojuego = model.VideojuegoId is int referenceVideojuego
                ? await unitOfWork.VideojuegoRepository.ReadAsync(referenceVideojuego) ?? throw new InvalidOperationException("El registro seleccionado de Videojuego ya no existe.") : null!;
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierTemaConversacionDefaultAsync(entity);

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
    /*PROTECTED REGION ID(TemaConversacionController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(TemaConversacionController_Delete_GET_body) START*/

        var entity = await _temaConversacionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(TemaConversacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(TemaConversacionController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.WebEjemplo.ViewModels.TemaConversacionViewModel());
        await _temaConversacionService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_FindByTitulo_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByTitulo(string p_texto)
    {
    /*PROTECTED REGION ID(TemaConversacionController_FindByTitulo_GET_body) START*/

        var result = await _temaConversacionService.FindByTituloAsync(p_texto);
        return Ok(result.Select(TemaConversacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_FindByRecientes_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByRecientes()
    {
    /*PROTECTED REGION ID(TemaConversacionController_FindByRecientes_GET_body) START*/

        var result = await _temaConversacionService.FindByRecientesAsync();
        return Ok(result.Select(TemaConversacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_AddDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenuncia(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
    /*PROTECTED REGION ID(TemaConversacionController_AddDenuncia_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddDenunciaAsync(p_oid, p_denuncia_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_AddUsuarioFavorito_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioFavorito(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(TemaConversacionController_AddUsuarioFavorito_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioFavoritoAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_AddUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(TemaConversacionController_AddUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_QuitarUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> QuitarUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(TemaConversacionController_QuitarUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.QuitarUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_FindByTextoLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByTextoLinQ(string p_texto)
    {
    /*PROTECTED REGION ID(TemaConversacionController_FindByTextoLinQ_GET_body) START*/

        var result = await _contenidoService.FindByTextoLinQAsync(p_texto);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_FindOrderByFecha_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderByFecha()
    {
    /*PROTECTED REGION ID(TemaConversacionController_FindOrderByFecha_GET_body) START*/

        var result = await _contenidoService.FindOrderByFechaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_FindOrderMeGusta_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderMeGusta()
    {
    /*PROTECTED REGION ID(TemaConversacionController_FindOrderMeGusta_GET_body) START*/

        var result = await _contenidoService.FindOrderMeGustaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TemaConversacionController_FindBYUserLiked_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindBYUserLiked(int p_user)
    {
    /*PROTECTED REGION ID(TemaConversacionController_FindBYUserLiked_GET_body) START*/

        var result = await _contenidoService.FindBYUserLikedAsync(p_user);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(TemaConversacionController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsTemaConversacionViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.TemaConversacionViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(TemaConversacionController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsTemaConversacionViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.TemaConversacionViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
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
