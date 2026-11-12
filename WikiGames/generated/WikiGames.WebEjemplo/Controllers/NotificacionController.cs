// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(NotificacionController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.WebEjemplo.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.WebEjemplo.Controllers;

/*PROTECTED REGION ID(NotificacionController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class NotificacionController : Controller
{
    /*PROTECTED REGION ID(NotificacionController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly NotificacionService _notificacionService;
    private readonly ILogger<NotificacionController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public NotificacionController(NotificacionService notificacionService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<NotificacionController>? logger = null)
    {
        _notificacionService = notificacionService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<NotificacionController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(NotificacionController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(NotificacionController_Index_GET_body) START*/

        var entities = await _notificacionService.ReadAllAsync();
        return View(entities.Select(NotificacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(NotificacionController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(NotificacionController_Details_GET_body) START*/

        var entity = await _notificacionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(NotificacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(NotificacionController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(NotificacionController_Edit_GET_body) START*/

        var entity = await _notificacionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = NotificacionAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(NotificacionController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.WebEjemplo.ViewModels.NotificacionViewModel model)
    {
    /*PROTECTED REGION ID(NotificacionController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new NotificacionService(unitOfWork);
        var entity = await service.ReadOIDNotificacionDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.DenunciaId is int selectedDenuncia &&
            !model.DenunciaOptions.Any(option => option.Value == Convert.ToString(selectedDenuncia, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("DenunciaId", "Selecciona un registro existente de Denuncia.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Notificacion.Edit");
        try
        {

            await service.ModifyAsync(id, model.Texto, model.Fecha);
            entity.Denuncia = model.DenunciaId is int referenceDenuncia
                ? await unitOfWork.DenunciaRepository.ReadAsync(referenceDenuncia) ?? throw new InvalidOperationException("El registro seleccionado de Denuncia ya no existe.") : null!;

            await service.ModifierNotificacionDefaultAsync(entity);

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
    /*PROTECTED REGION ID(NotificacionController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(NotificacionController_Delete_GET_body) START*/

        var entity = await _notificacionService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(NotificacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(NotificacionController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(NotificacionController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.WebEjemplo.ViewModels.NotificacionViewModel());
        await _notificacionService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(NotificacionController_AddDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenuncia(int p_oid, int p_denuncia_oid)
    {
    /*PROTECTED REGION ID(NotificacionController_AddDenuncia_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _notificacionService.AddDenunciaAsync(p_oid, p_denuncia_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(NotificacionController_FindByFecha_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByFecha(DateTime p_fecha)
    {
    /*PROTECTED REGION ID(NotificacionController_FindByFecha_GET_body) START*/

        var result = await _notificacionService.FindByFechaAsync(p_fecha);
        return Ok(result.Select(NotificacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(NotificacionController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsNotificacionViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.NotificacionViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(NotificacionController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsNotificacionViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.NotificacionViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.DenunciaRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.DenunciaOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
