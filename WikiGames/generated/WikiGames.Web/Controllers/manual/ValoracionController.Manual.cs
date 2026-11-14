// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ValoracionController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(ValoracionController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class ValoracionController
{
    [HttpGet]
    /*PROTECTED REGION ID(ValoracionController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(ValoracionController_Create_GET_body) ENABLED START*/

        var model = new WikiGames.Web.ViewModels.ValoracionViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.ValoracionViewModel model)
    {
    /*PROTECTED REGION ID(ValoracionController_Create_POST_body) ENABLED START*/

        ModelState.Remove(nameof(model.FechaCreacion)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.NumeroMeGusta)); // Campo fuera del contrato Create de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.UseCases.Valoracion.ValoracionUseCase(unitOfWork);



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

        await unitOfWork.BeginTransactionAsync("MVC.Valoracion.Create");
        try
        {

            var entity = await service.NewAsync(model.Texto, model.Puntuacion, model.UsuarioCreadorId.GetValueOrDefault(), model.VideojuegoId.GetValueOrDefault());
            entity.Videojuego = model.VideojuegoId is int referenceVideojuego
                ? await unitOfWork.VideojuegoRepository.ReadAsync(referenceVideojuego) ?? throw new InvalidOperationException("El registro seleccionado de Videojuego ya no existe.") : null!;
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await unitOfWork.ValoracionRepository.SaveAsync(entity);

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
    /*PROTECTED REGION ID(ValoracionController_ActualizarValoracion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ActualizarValoracion(int p_oid, float nuevo_valor)
    {
    /*PROTECTED REGION ID(ValoracionController_ActualizarValoracion_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Valoracion.ValoracionUseCase(unitOfWork);
        await useCase.ActualizarValoracionAsync(p_oid, nuevo_valor);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_EliminarValoracion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> EliminarValoracion(int p_oid)
    {
    /*PROTECTED REGION ID(ValoracionController_EliminarValoracion_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Valoracion.ValoracionUseCase(unitOfWork);
        await useCase.EliminarValoracionAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_DarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(ValoracionController_DarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
        await useCase.DarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ValoracionController_RetirarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RetirarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(ValoracionController_RetirarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.RetirarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
