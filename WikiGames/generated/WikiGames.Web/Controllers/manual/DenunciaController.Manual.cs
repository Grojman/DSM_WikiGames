// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(DenunciaController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(DenunciaController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class DenunciaController
{
    [HttpGet]
    /*PROTECTED REGION ID(DenunciaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(DenunciaController_Create_GET_body) ENABLED START*/

        var model = new WikiGames.Web.ViewModels.DenunciaViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DenunciaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.DenunciaViewModel model)
    {
    /*PROTECTED REGION ID(DenunciaController_Create_POST_body) ENABLED START*/

        ModelState.Remove(nameof(model.Fecha)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.Estado)); // Campo fuera del contrato Create de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.UseCases.Denuncia.DenunciaUseCase(unitOfWork);



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

        await unitOfWork.BeginTransactionAsync("MVC.Denuncia.Create");
        try
        {

            var entity = await service.NewCustomizedAsync(model.DenuncianteId.GetValueOrDefault());
            entity.Notificacion = model.NotificacionId is int referenceNotificacion
                ? await unitOfWork.NotificacionRepository.ReadAsync(referenceNotificacion) ?? throw new InvalidOperationException("El registro seleccionado de Notificacion ya no existe.") : null!;
            entity.Denunciante = model.DenuncianteId is int referenceDenunciante
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceDenunciante) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            entity.Denunciado = model.DenunciadoId is int referenceDenunciado
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceDenunciado) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            entity.ContenidoDenunciado = model.ContenidoDenunciadoId is int referenceContenidoDenunciado
                ? await unitOfWork.ContenidoRepository.ReadAsync(referenceContenidoDenunciado) ?? throw new InvalidOperationException("El registro seleccionado de Contenido ya no existe.") : null!;

            await unitOfWork.DenunciaRepository.SaveAsync(entity);

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
    /*PROTECTED REGION ID(DenunciaController_AprobarDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AprobarDenuncia(int p_oid)
    {
    /*PROTECTED REGION ID(DenunciaController_AprobarDenuncia_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Denuncia.DenunciaUseCase(unitOfWork);
        await useCase.AprobarDenunciaAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DenunciaController_RechazarDenuncia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RechazarDenuncia(int p_oid)
    {
    /*PROTECTED REGION ID(DenunciaController_RechazarDenuncia_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _denunciaService.RechazarDenunciaAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
