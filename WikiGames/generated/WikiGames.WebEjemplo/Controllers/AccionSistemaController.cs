// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(AccionSistemaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.WebEjemplo.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.WebEjemplo.Controllers;

/*PROTECTED REGION ID(AccionSistemaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class AccionSistemaController : Controller
{
    /*PROTECTED REGION ID(AccionSistemaController_dependencies) START*/
    private readonly AccionSistemaService _accionSistemaService;
    private readonly ILogger<AccionSistemaController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public AccionSistemaController(AccionSistemaService accionSistemaService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, ILogger<AccionSistemaController>? logger = null)
    {
        _accionSistemaService = accionSistemaService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;

        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<AccionSistemaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(AccionSistemaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(AccionSistemaController_Index_GET_body) START*/

        var entities = await _accionSistemaService.ReadAllAsync();
        return View(entities.Select(AccionSistemaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(AccionSistemaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(AccionSistemaController_Details_GET_body) START*/

        var entity = await _accionSistemaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(AccionSistemaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(AccionSistemaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(AccionSistemaController_Create_GET_body) START*/

        var model = new WikiGames.WebEjemplo.ViewModels.AccionSistemaViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(AccionSistemaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.WebEjemplo.ViewModels.AccionSistemaViewModel model)
    {
    /*PROTECTED REGION ID(AccionSistemaController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new AccionSistemaService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.CausanteId is int selectedCausante &&
            !model.CausanteOptions.Any(option => option.Value == Convert.ToString(selectedCausante, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("CausanteId", "Selecciona un registro existente de Usuario.");
        if (model.CausanteId is null) ModelState.AddModelError("CausanteId", "Debes seleccionar una opción.");
        if (model.ContenidoAfectadoId is int selectedContenidoAfectado &&
            !model.ContenidoAfectadoOptions.Any(option => option.Value == Convert.ToString(selectedContenidoAfectado, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ContenidoAfectadoId", "Selecciona un registro existente de Contenido.");
        if (model.ContenidoAfectadoId is null) ModelState.AddModelError("ContenidoAfectadoId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.AccionSistema.Create");
        try
        {

            var entity = await service.NewAsync(model.Fecha, model.Descripcion, model.CausanteId.GetValueOrDefault(), model.ContenidoAfectadoId.GetValueOrDefault());
            entity.Causante = model.CausanteId is int referenceCausante
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceCausante) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            entity.ContenidoAfectado = model.ContenidoAfectadoId is int referenceContenidoAfectado
                ? await unitOfWork.ContenidoRepository.ReadAsync(referenceContenidoAfectado) ?? throw new InvalidOperationException("El registro seleccionado de Contenido ya no existe.") : null!;

            await service.ModifierAccionSistemaDefaultAsync(entity);

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
    /*PROTECTED REGION ID(AccionSistemaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(AccionSistemaController_Edit_GET_body) START*/

        var entity = await _accionSistemaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = AccionSistemaAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(AccionSistemaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.WebEjemplo.ViewModels.AccionSistemaViewModel model)
    {
    /*PROTECTED REGION ID(AccionSistemaController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new AccionSistemaService(unitOfWork);
        var entity = await service.ReadOIDAccionSistemaDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.CausanteId is int selectedCausante &&
            !model.CausanteOptions.Any(option => option.Value == Convert.ToString(selectedCausante, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("CausanteId", "Selecciona un registro existente de Usuario.");
        if (model.CausanteId is null) ModelState.AddModelError("CausanteId", "Debes seleccionar una opción.");
        if (model.ContenidoAfectadoId is int selectedContenidoAfectado &&
            !model.ContenidoAfectadoOptions.Any(option => option.Value == Convert.ToString(selectedContenidoAfectado, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ContenidoAfectadoId", "Selecciona un registro existente de Contenido.");
        if (model.ContenidoAfectadoId is null) ModelState.AddModelError("ContenidoAfectadoId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.AccionSistema.Edit");
        try
        {

            await service.ModifyAsync(id, model.Fecha, model.Descripcion, model.CausanteId.GetValueOrDefault(), model.ContenidoAfectadoId.GetValueOrDefault());
            entity.Causante = model.CausanteId is int referenceCausante
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceCausante) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            entity.ContenidoAfectado = model.ContenidoAfectadoId is int referenceContenidoAfectado
                ? await unitOfWork.ContenidoRepository.ReadAsync(referenceContenidoAfectado) ?? throw new InvalidOperationException("El registro seleccionado de Contenido ya no existe.") : null!;

            await service.ModifierAccionSistemaDefaultAsync(entity);

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
    /*PROTECTED REGION ID(AccionSistemaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(AccionSistemaController_Delete_GET_body) START*/

        var entity = await _accionSistemaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(AccionSistemaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(AccionSistemaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(AccionSistemaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.WebEjemplo.ViewModels.AccionSistemaViewModel());
        await _accionSistemaService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(AccionSistemaController_FindByFechaLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByFechaLinQ(DateTime p_fecha)
    {
    /*PROTECTED REGION ID(AccionSistemaController_FindByFechaLinQ_GET_body) START*/

        var result = await _accionSistemaService.FindByFechaLinQAsync(p_fecha);
        return Ok(result.Select(AccionSistemaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(AccionSistemaController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsAccionSistemaViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.AccionSistemaViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(AccionSistemaController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsAccionSistemaViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.AccionSistemaViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.UsuarioRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.ContenidoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Texto, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.CausanteOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.ContenidoAfectadoOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
