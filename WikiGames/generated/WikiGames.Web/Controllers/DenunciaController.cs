// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(DenunciaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

/*PROTECTED REGION ID(DenunciaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class DenunciaController : Controller
{
    /*PROTECTED REGION ID(DenunciaController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly DenunciaService _denunciaService;
    private readonly ILogger<DenunciaController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public DenunciaController(DenunciaService denunciaService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<DenunciaController>? logger = null)
    {
        _denunciaService = denunciaService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<DenunciaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(DenunciaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(DenunciaController_Index_GET_body) START*/

        var entities = await _denunciaService.ReadAllAsync();
        return View(entities.Select(DenunciaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(DenunciaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(DenunciaController_Details_GET_body) START*/

        var entity = await _denunciaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(DenunciaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(DenunciaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(DenunciaController_Edit_GET_body) START*/

        var entity = await _denunciaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = DenunciaAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DenunciaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.Web.ViewModels.DenunciaViewModel model)
    {
    /*PROTECTED REGION ID(DenunciaController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.DenunciaService(unitOfWork);
        var entity = await service.ReadOIDDenunciaDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.NotificacionId is int selectedNotificacion &&
            !model.NotificacionOptions.Any(option => option.Value == Convert.ToString(selectedNotificacion, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("NotificacionId", "Selecciona un registro existente de Notificacion.");

        if (model.DenuncianteId is int selectedDenunciante &&
            !model.DenuncianteOptions.Any(option => option.Value == Convert.ToString(selectedDenunciante, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("DenuncianteId", "Selecciona un registro existente de Usuario.");
        if (model.DenuncianteId is null) ModelState.AddModelError("DenuncianteId", "Debes seleccionar una opción.");
        if (model.DenunciadoId is int selectedDenunciado &&
            !model.DenunciadoOptions.Any(option => option.Value == Convert.ToString(selectedDenunciado, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("DenunciadoId", "Selecciona un registro existente de Usuario.");

        if (model.ContenidoDenunciadoId is int selectedContenidoDenunciado &&
            !model.ContenidoDenunciadoOptions.Any(option => option.Value == Convert.ToString(selectedContenidoDenunciado, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ContenidoDenunciadoId", "Selecciona un registro existente de Contenido.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Denuncia.Edit");
        try
        {

            await service.ModifyAsync(id, model.Fecha, model.Estado, model.DenuncianteId.GetValueOrDefault());
            entity.Notificacion = model.NotificacionId is int referenceNotificacion
                ? await unitOfWork.NotificacionRepository.ReadAsync(referenceNotificacion) ?? throw new InvalidOperationException("El registro seleccionado de Notificacion ya no existe.") : null!;
            entity.Denunciante = model.DenuncianteId is int referenceDenunciante
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceDenunciante) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            entity.Denunciado = model.DenunciadoId is int referenceDenunciado
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceDenunciado) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            entity.ContenidoDenunciado = model.ContenidoDenunciadoId is int referenceContenidoDenunciado
                ? await unitOfWork.ContenidoRepository.ReadAsync(referenceContenidoDenunciado) ?? throw new InvalidOperationException("El registro seleccionado de Contenido ya no existe.") : null!;

            await service.ModifierDenunciaDefaultAsync(entity);

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
    /*PROTECTED REGION ID(DenunciaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(DenunciaController_Delete_GET_body) START*/

        var entity = await _denunciaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(DenunciaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DenunciaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(DenunciaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.Web.ViewModels.DenunciaViewModel());
        await _denunciaService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DenunciaController_AddDenunciado_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddDenunciado(int p_oid, int p_usuario_oid)
    {
    /*PROTECTED REGION ID(DenunciaController_AddDenunciado_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _denunciaService.AddDenunciadoAsync(p_oid, p_usuario_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DenunciaController_AddContenidoDenunciado_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddContenidoDenunciado(int p_oid, int p_contenido_oid)
    {
    /*PROTECTED REGION ID(DenunciaController_AddContenidoDenunciado_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _denunciaService.AddContenidoDenunciadoAsync(p_oid, p_contenido_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(DenunciaController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsDenunciaViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.DenunciaViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(DenunciaController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsDenunciaViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.DenunciaViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.NotificacionRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Texto, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.UsuarioRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options2 = (await unitOfWork.ContenidoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Texto, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.NotificacionOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.DenuncianteOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.DenunciadoOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.ContenidoDenunciadoOptions = options2.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
