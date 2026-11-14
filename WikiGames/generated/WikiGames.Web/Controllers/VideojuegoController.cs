// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(VideojuegoController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

/*PROTECTED REGION ID(VideojuegoController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class VideojuegoController : Controller
{
    /*PROTECTED REGION ID(VideojuegoController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly VideojuegoService _videojuegoService;
    private readonly ContenidoService _contenidoService;
    private readonly ILogger<VideojuegoController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public VideojuegoController(VideojuegoService videojuegoService, ContenidoService contenidoService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<VideojuegoController>? logger = null)
    {
        _videojuegoService = videojuegoService;
        _contenidoService = contenidoService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<VideojuegoController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(VideojuegoController_Index_GET_body) START*/

        var entities = await _videojuegoService.ReadAllAsync();
        return View(entities.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(VideojuegoController_Details_GET_body) START*/

        var entity = await _videojuegoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(VideojuegoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(VideojuegoController_Edit_GET_body) START*/

        var entity = await _videojuegoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = VideojuegoAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.Web.ViewModels.VideojuegoViewModel model)
    {
    /*PROTECTED REGION ID(VideojuegoController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.VideojuegoService(unitOfWork);
        var entity = await service.ReadOIDVideojuegoDefaultAsync(id);
        if (entity is null) return NotFound();
        model.Valoraciones ??= new();
        if (model.Valoraciones.Count > 500) return BadRequest("Número de líneas no válido (máximo 500).");
        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.RechazoId is int selectedRechazo &&
            !model.RechazoOptions.Any(option => option.Value == Convert.ToString(selectedRechazo, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("RechazoId", "Selecciona un registro existente de Rechazo.");

        if (model.UsuarioCreadorId is int selectedUsuarioCreador &&
            !model.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (model.UsuarioCreadorId is null) ModelState.AddModelError("UsuarioCreadorId", "Debes seleccionar una opción.");
        if (model.Valoraciones is null) { model.Valoraciones = new(); ModelState.AddModelError(nameof(model.Valoraciones), "Debes indicar las líneas."); }
        if (model.Valoraciones.Count < 0 || model.Valoraciones.Count > 500)
            ModelState.AddModelError(nameof(model.Valoraciones), "Número de líneas no válido (máximo 500).");
        var seenValoraciones = new HashSet<int>();
        for (var index = 0; index < model.Valoraciones.Count; index++)
        {
            var row = model.Valoraciones[index];
            if (!EqualityComparer<int>.Default.Equals(row.Id, default!) &&
                (!entity.Valoraciones.Any(child => child.Id.Equals(row.Id)) || !seenValoraciones.Add(row.Id)))
                ModelState.AddModelError($"Valoraciones[{index}].Id", "La línea no pertenece a este registro o está repetida.");
        if (row.UsuarioCreadorId is int selectedRowValoracionesUsuarioCreador &&
            !row.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedRowValoracionesUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError($"Valoraciones[{index}].UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (row.UsuarioCreadorId is null) ModelState.AddModelError($"Valoraciones[{index}].UsuarioCreadorId", "Debes seleccionar una opción.");
        }

        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Videojuego.Edit");
        try
        {
            var submittedValoraciones = new List<WikiGames.Domain.Entities.Valoracion>();
            foreach (var row in model.Valoraciones)
            {
                var child = entity.Valoraciones.SingleOrDefault(item => item.Id.Equals(row.Id)) ?? ValoracionAssembler.Create(row);
                ValoracionAssembler.ApplyToEntity(row, child);
            child.UsuarioCreador = row.UsuarioCreadorId is int referenceRowValoracionesUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceRowValoracionesUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
                submittedValoraciones.Add(child);
            }
            await service.ModifyAsync(id, model.Texto, model.FechaCreacion, model.NumeroMeGusta, model.Titulo, model.Enlace, model.Imagen, model.Estado, model.PuntuacionMedia, model.UsuarioCreadorId.GetValueOrDefault());
            entity.Rechazo = model.RechazoId is int referenceRechazo
                ? await unitOfWork.RechazoRepository.ReadAsync(referenceRechazo) ?? throw new InvalidOperationException("El registro seleccionado de Rechazo ya no existe.") : null!;
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            foreach (var removed in entity.Valoraciones.Where(child => !submittedValoraciones.Contains(child)).ToList()) entity.Valoraciones.Remove(removed);
            for (var index = 0; index < submittedValoraciones.Count; index++)
            {
                var child = submittedValoraciones[index];
                child.Videojuego = entity;
                if (!entity.Valoraciones.Contains(child)) entity.Valoraciones.Add(child);
            }
            await service.ModifierVideojuegoDefaultAsync(entity);

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
    /*PROTECTED REGION ID(VideojuegoController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(VideojuegoController_Delete_GET_body) START*/

        var entity = await _videojuegoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(VideojuegoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(VideojuegoController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.Web.ViewModels.VideojuegoViewModel());
        await _videojuegoService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindByNombre_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByNombre(string p_texto)
    {
    /*PROTECTED REGION ID(VideojuegoController_FindByNombre_GET_body) START*/

        var result = await _videojuegoService.FindByNombreAsync(p_texto);
        return Ok(result.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindByCategoria_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByCategoria(string p_texto)
    {
    /*PROTECTED REGION ID(VideojuegoController_FindByCategoria_GET_body) START*/

        var result = await _videojuegoService.FindByCategoriaAsync(p_texto);
        return Ok(result.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindRecientes_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindRecientes()
    {
    /*PROTECTED REGION ID(VideojuegoController_FindRecientes_GET_body) START*/

        var result = await _videojuegoService.FindRecientesAsync();
        return Ok(result.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindAntiguos_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindAntiguos()
    {
    /*PROTECTED REGION ID(VideojuegoController_FindAntiguos_GET_body) START*/

        var result = await _videojuegoService.FindAntiguosAsync();
        return Ok(result.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindByMeGusta_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByMeGusta()
    {
    /*PROTECTED REGION ID(VideojuegoController_FindByMeGusta_GET_body) START*/

        var result = await _videojuegoService.FindByMeGustaAsync();
        return Ok(result.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindPopulares_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindPopulares()
    {
    /*PROTECTED REGION ID(VideojuegoController_FindPopulares_GET_body) START*/

        var result = await _videojuegoService.FindPopularesAsync();
        return Ok(result.Select(VideojuegoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_AddDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenuncia(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
    /*PROTECTED REGION ID(VideojuegoController_AddDenuncia_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddDenunciaAsync(p_oid, p_denuncia_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_AddUsuarioFavorito_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioFavorito(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(VideojuegoController_AddUsuarioFavorito_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioFavoritoAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_AddUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(VideojuegoController_AddUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.AddUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_QuitarUsuarioMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> QuitarUsuarioMeGusta(int p_oid, IReadOnlyList<int> p_usuario_oids)
    {
    /*PROTECTED REGION ID(VideojuegoController_QuitarUsuarioMeGusta_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.QuitarUsuarioMeGustaAsync(p_oid, p_usuario_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindByTextoLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByTextoLinQ(string p_texto)
    {
    /*PROTECTED REGION ID(VideojuegoController_FindByTextoLinQ_GET_body) START*/

        var result = await _contenidoService.FindByTextoLinQAsync(p_texto);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindOrderByFecha_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderByFecha()
    {
    /*PROTECTED REGION ID(VideojuegoController_FindOrderByFecha_GET_body) START*/

        var result = await _contenidoService.FindOrderByFechaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindOrderMeGusta_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindOrderMeGusta()
    {
    /*PROTECTED REGION ID(VideojuegoController_FindOrderMeGusta_GET_body) START*/

        var result = await _contenidoService.FindOrderMeGustaAsync();
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_FindBYUserLiked_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindBYUserLiked(int p_user)
    {
    /*PROTECTED REGION ID(VideojuegoController_FindBYUserLiked_GET_body) START*/

        var result = await _contenidoService.FindBYUserLikedAsync(p_user);
        return Ok(result.Select(ContenidoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(VideojuegoController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsVideojuegoViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.VideojuegoViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(VideojuegoController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsVideojuegoViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.VideojuegoViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {
        if (model.Valoraciones.Count > 500) { ModelState.AddModelError(nameof(model.Valoraciones), "Máximo 500 líneas."); return; }
        var options0 = (await unitOfWork.RechazoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Motivo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.UsuarioRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.RechazoOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.UsuarioCreadorOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        foreach (var row in model.Valoraciones)
        {
        row.UsuarioCreadorOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        }
        model.ValoracionesTemplate.UsuarioCreadorOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
    }
    /*PROTECTED REGION END*/

}
