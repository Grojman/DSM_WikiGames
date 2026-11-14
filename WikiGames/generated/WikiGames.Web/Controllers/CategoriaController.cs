// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(CategoriaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

/*PROTECTED REGION ID(CategoriaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class CategoriaController : Controller
{
    /*PROTECTED REGION ID(CategoriaController_dependencies) START*/
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly CategoriaService _categoriaService;
    private readonly ILogger<CategoriaController> _logger;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public CategoriaController(CategoriaService categoriaService, WikiGames.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, WikiGames.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<CategoriaController>? logger = null)
    {
        _categoriaService = categoriaService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<CategoriaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(CategoriaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(CategoriaController_Index_GET_body) START*/

        var entities = await _categoriaService.ReadAllAsync();
        return View(entities.Select(CategoriaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CategoriaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(CategoriaController_Details_GET_body) START*/

        var entity = await _categoriaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CategoriaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CategoriaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(CategoriaController_Create_GET_body) START*/

        var model = new WikiGames.Web.ViewModels.CategoriaViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CategoriaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.CategoriaViewModel model)
    {
    /*PROTECTED REGION ID(CategoriaController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.CategoriaService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.PadreId is int selectedPadre &&
            !model.PadreOptions.Any(option => option.Value == Convert.ToString(selectedPadre, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("PadreId", "Selecciona un registro existente de Categoria.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Categoria.Create");
        try
        {

            var entity = await service.NewAsync(model.Name);
            entity.Padre = model.PadreId is int referencePadre
                ? await unitOfWork.CategoriaRepository.ReadAsync(referencePadre) ?? throw new InvalidOperationException("El registro seleccionado de Categoria ya no existe.") : null!;

            await service.ModifierCategoriaDefaultAsync(entity);

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
    /*PROTECTED REGION ID(CategoriaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(CategoriaController_Edit_GET_body) START*/

        var entity = await _categoriaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = CategoriaAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CategoriaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, WikiGames.Web.ViewModels.CategoriaViewModel model, int p_videojuego_oid)
    {
    /*PROTECTED REGION ID(CategoriaController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.CategoriaService(unitOfWork);
        var entity = await service.ReadOIDCategoriaDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.PadreId is int selectedPadre &&
            !model.PadreOptions.Any(option => option.Value == Convert.ToString(selectedPadre, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("PadreId", "Selecciona un registro existente de Categoria.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Categoria.Edit");
        try
        {

            await service.ModifyAsync(id, model.Name, p_videojuego_oid);
            entity.Padre = model.PadreId is int referencePadre
                ? await unitOfWork.CategoriaRepository.ReadAsync(referencePadre) ?? throw new InvalidOperationException("El registro seleccionado de Categoria ya no existe.") : null!;

            await service.ModifierCategoriaDefaultAsync(entity);

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
    /*PROTECTED REGION ID(CategoriaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(CategoriaController_Delete_GET_body) START*/

        var entity = await _categoriaService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CategoriaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CategoriaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(CategoriaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.Web.ViewModels.CategoriaViewModel());
        await _categoriaService.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CategoriaController_AddHijo_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddHijo(int p_oid, IReadOnlyList<int> p_categoria2_oids)
    {
    /*PROTECTED REGION ID(CategoriaController_AddHijo_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _categoriaService.AddHijoAsync(p_oid, p_categoria2_oids);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CategoriaController_AddPadre_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AddPadre(int p_oid, int p_categoria2_oid)
    {
    /*PROTECTED REGION ID(CategoriaController_AddPadre_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _categoriaService.AddPadreAsync(p_oid, p_categoria2_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CategoriaController_FindByNameLinQ_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> FindByNameLinQ(string p_name)
    {
    /*PROTECTED REGION ID(CategoriaController_FindByNameLinQ_GET_body) START*/

        var result = await _categoriaService.FindByNameLinQAsync(p_name);
        return Ok(result.Select(CategoriaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(CategoriaController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsCategoriaViewModel) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.CategoriaViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CategoriaController_method_LoadFormOptionsAsync_WikiGamesWebViewModelsCategoriaViewModel_WikiGamesApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(WikiGames.Web.ViewModels.CategoriaViewModel model, WikiGames.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.CategoriaRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.PadreOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
