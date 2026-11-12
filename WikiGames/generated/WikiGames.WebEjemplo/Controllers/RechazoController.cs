// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(RechazoController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.WebEjemplo.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.WebEjemplo.Controllers;

/*PROTECTED REGION ID(RechazoController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class RechazoController : Controller
{
    /*PROTECTED REGION ID(RechazoController_dependencies) START*/
    private readonly RechazoService _rechazoService;
    private readonly ILogger<RechazoController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public RechazoController(RechazoService rechazoService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, ILogger<RechazoController>? logger = null)
    {
        _rechazoService = rechazoService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;

        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<RechazoController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(RechazoController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(RechazoController_Index_GET_body) START*/

        var entities = await _rechazoService.ReadAllAsync();
        return View(entities.Select(RechazoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(RechazoController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(RechazoController_Details_GET_body) START*/

        var entity = await _rechazoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(RechazoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(RechazoController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(RechazoController_Create_GET_body) START*/

        var model = new WikiGames.WebEjemplo.ViewModels.RechazoViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RechazoController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.WebEjemplo.ViewModels.RechazoViewModel model)
    {
    /*PROTECTED REGION ID(RechazoController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new RechazoService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.VideojuegoId is int selectedVideojuego &&
            !model.VideojuegoOptions.Any(option => option.Value == Convert.ToString(selectedVideojuego, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("VideojuegoId", "Selecciona un registro existente de Videojuego.");
        if (model.VideojuegoId is null) ModelState.AddModelError("VideojuegoId", "Debes seleccionar una opción.");
        if (model.GestorId is int selectedGestor &&
            !model.GestorOptions.Any(option => option.Value == Convert.ToString(selectedGestor, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("GestorId", "Selecciona un registro existente de Usuario.");
        if (model.GestorId is null) ModelState.AddModelError("GestorId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Rechazo.Create");
        try
        {

            var entity = await service.NewAsync(model.Fecha, model.Motivo, model.VideojuegoId.GetValueOrDefault(), model.GestorId.GetValueOrDefault());
            entity.Videojuego = model.VideojuegoId is int referenceVideojuego
                ? await unitOfWork.VideojuegoRepository.ReadAsync(referenceVideojuego) ?? throw new InvalidOperationException("El registro seleccionado de Videojuego ya no existe.") : null!;
            entity.Gestor = model.GestorId is int referenceGestor
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceGestor) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierRechazoDefaultAsync(entity);

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
    /*PROTECTED REGION ID(RechazoController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(RechazoController_Edit_GET_body) START*/

        var entity = await _rechazoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = RechazoAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RechazoController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.WebEjemplo.ViewModels.RechazoViewModel model)
    {
    /*PROTECTED REGION ID(RechazoController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new RechazoService(unitOfWork);
        var entity = await service.ReadOIDRechazoDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.VideojuegoId is int selectedVideojuego &&
            !model.VideojuegoOptions.Any(option => option.Value == Convert.ToString(selectedVideojuego, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("VideojuegoId", "Selecciona un registro existente de Videojuego.");
        if (model.VideojuegoId is null) ModelState.AddModelError("VideojuegoId", "Debes seleccionar una opción.");
        if (model.GestorId is int selectedGestor &&
            !model.GestorOptions.Any(option => option.Value == Convert.ToString(selectedGestor, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("GestorId", "Selecciona un registro existente de Usuario.");
        if (model.GestorId is null) ModelState.AddModelError("GestorId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Rechazo.Edit");
        try
        {

            await service.ModifyAsync(id, model.Fecha, model.Motivo, model.VideojuegoId.GetValueOrDefault(), model.GestorId.GetValueOrDefault());
            entity.Videojuego = model.VideojuegoId is int referenceVideojuego
                ? await unitOfWork.VideojuegoRepository.ReadAsync(referenceVideojuego) ?? throw new InvalidOperationException("El registro seleccionado de Videojuego ya no existe.") : null!;
            entity.Gestor = model.GestorId is int referenceGestor
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceGestor) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierRechazoDefaultAsync(entity);

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
    /*PROTECTED REGION ID(RechazoController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(RechazoController_Delete_GET_body) START*/

        var entity = await _rechazoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(RechazoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RechazoController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(RechazoController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.WebEjemplo.ViewModels.RechazoViewModel());
        await _rechazoService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(RechazoController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsRechazoViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.RechazoViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RechazoController_method_LoadFormOptionsAsync_WikiGamesWebEjemploViewModelsRechazoViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.WebEjemplo.ViewModels.RechazoViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.VideojuegoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.UsuarioRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.VideojuegoOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.GestorOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
