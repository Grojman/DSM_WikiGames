// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(NotificacionController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.WebEjemplo.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.WebEjemplo.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(NotificacionController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class NotificacionController
{
    [HttpGet]
    /*PROTECTED REGION ID(NotificacionController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(NotificacionController_Create_GET_body) ENABLED START*/

        var model = new WikiGames.WebEjemplo.ViewModels.NotificacionViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(NotificacionController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.WebEjemplo.ViewModels.NotificacionViewModel model, IReadOnlyList<int> p_notificados_oids)
    {
    /*PROTECTED REGION ID(NotificacionController_Create_POST_body) ENABLED START*/

        ModelState.Remove(nameof(model.Texto)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.Fecha)); // Campo fuera del contrato Create de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new NotificacionUseCase(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.DenunciaId is int selectedDenuncia &&
            !model.DenunciaOptions.Any(option => option.Value == Convert.ToString(selectedDenuncia, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("DenunciaId", "Selecciona un registro existente de Denuncia.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Notificacion.Create");
        try
        {

            var entity = await service.NewAsync(model.Texto, model.Fecha, p_notificados_oids);
            entity.Denuncia = model.DenunciaId is int referenceDenuncia
                ? await unitOfWork.DenunciaRepository.ReadAsync(referenceDenuncia) ?? throw new InvalidOperationException("El registro seleccionado de Denuncia ya no existe.") : null!;

            await unitOfWork.NotificacionRepository.SaveAsync(entity);

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
}
