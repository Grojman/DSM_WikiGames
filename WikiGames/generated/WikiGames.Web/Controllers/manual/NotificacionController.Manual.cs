// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(NotificacionController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
using WikiGames.Application.Exceptions;
using WikiGames.Web.Models.Usuarios;
using WikiGames.Web.Utils.Paginas;
using WikiGames.Web.ViewModels.Paginas;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

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

        var model = new WikiGames.Web.ViewModels.NotificacionViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(NotificacionController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.NotificacionViewModel model, IReadOnlyList<int> p_notificados_oids)
    {
    /*PROTECTED REGION ID(NotificacionController_Create_POST_body) ENABLED START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(unitOfWork);



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

    // =====================================================================
    // Páginas WikiGames "Notificaciones" y "Detalle de notificación"
    // (notificaciones.html y notificacion.html). Usuario autenticado.
    // Acciones enteramente manuales: HARIA las conserva al regenerar.
    // =====================================================================

    [HttpGet]
    public async Task<IActionResult> Lista(string? q)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var filtro = q?.Trim();
        var notificaciones = (await unitOfWork.NotificacionRepository.ReadAllAsync())
            .Where(n => n.Notificados.Any(u => u.Id == usuarioId.Value));

        if (!string.IsNullOrEmpty(filtro))
            notificaciones = notificaciones.Where(n => n.Texto.Contains(filtro, StringComparison.CurrentCultureIgnoreCase));

        var lista = notificaciones
            .OrderByDescending(n => n.Fecha)
            .ThenByDescending(n => n.Id)
            .ToList();

        var misVideojuegos = (await unitOfWork.VideojuegoRepository.ReadAllAsync())
            .Where(v => v.UsuarioCreador?.Id == usuarioId.Value)
            .OrderByDescending(v => v.Titulo.Length)
            .ToList();

        var esModerador = User.IsUsuario<ModeradorModel>();
        var items = lista.Select(n => BuildNotificacionItem(n, misVideojuegos, esModerador)).ToList();

        return View(new ListaNotificacionesViewModel
        {
            Q = filtro,
            Notificaciones = items,
        });
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.NotificacionRepository.ReadAsync(id);
        if (entity is null || !entity.Notificados.Any(u => u.Id == usuarioId.Value)) return NotFound();

        var misVideojuegos = (await unitOfWork.VideojuegoRepository.ReadAllAsync())
            .Where(v => v.UsuarioCreador?.Id == usuarioId.Value)
            .OrderByDescending(v => v.Titulo.Length)
            .ToList();

        var item = BuildNotificacionItem(entity, misVideojuegos, User.IsUsuario<ModeradorModel>());
        return View(item);
    }

    /// <summary>Elimina la notificación para el usuario actual (y la borra si ya no tiene destinatarios ni denuncia).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, string? q)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.NotificacionRepository.ReadAsync(id);
        if (entity is null || !entity.Notificados.Any(u => u.Id == usuarioId.Value)) return NotFound();

        var usuario = await unitOfWork.UsuarioRepository.ReadAsync(usuarioId.Value);
        if (usuario is null) return Challenge();

        try
        {
            await unitOfWork.BeginTransactionAsync("MVC.Notificacion.Eliminar");
            var destinatario = entity.Notificados.FirstOrDefault(u => u.Id == usuarioId.Value);
            if (destinatario is not null) entity.Notificados.Remove(destinatario);
            usuario.Notificaciones.Remove(entity);

            await unitOfWork.NotificacionRepository.SaveAsync(entity);
            if (entity.Notificados.Count == 0 && entity.Denuncia is null)
                await unitOfWork.NotificacionRepository.DeleteAsync(entity);

            await unitOfWork.CommitAsync();
            TempData["Toast"] = "Notificación eliminada";
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogError(error, "Could not delete Notificacion {Id} for Usuario {UsuarioId}.", id, usuarioId.Value);
            TempData["Toast"] = "No se ha podido eliminar la notificación";
        }

        return RedirectToAction(nameof(Lista), new { q });
    }

    private NotificacionItemViewModel BuildNotificacionItem(Notificacion entity, IReadOnlyList<Videojuego> misVideojuegos, bool esModerador)
    {
        var item = new NotificacionItemViewModel
        {
            Id = entity.Id,
            Texto = entity.Texto,
            Fecha = entity.Fecha,
            DenunciaId = entity.Denuncia?.Id,
        };

        // Intentar asociar la notificación con uno de los videojuegos del usuario (aprobado, pendiente o rechazado).
        var videojuego = misVideojuegos.FirstOrDefault(v =>
            !string.IsNullOrWhiteSpace(v.Titulo)
            && entity.Texto.Contains(v.Titulo, StringComparison.OrdinalIgnoreCase));

        if (videojuego is not null)
        {
            item.ElementoRelacionadoTipo = "Videojuego";
            item.ElementoRelacionadoTitulo = videojuego.Titulo;
            item.ElementoRelacionadoEnlace = Url.Action("Ficha", "Videojuego", new { id = videojuego.Id });
            item.ElementoRelacionadoIcono = "gamepad";

            if (videojuego.Rechazo is not null
                && entity.Texto.Contains("rechazad", StringComparison.OrdinalIgnoreCase))
            {
                item.RechazoId = videojuego.Rechazo.Id;
            }
        }
        else if (entity.Denuncia is not null)
        {
            item.ElementoRelacionadoTipo = "Denuncia";
            item.ElementoRelacionadoTitulo = $"Denuncia #{entity.Denuncia.Id} ({entity.Denuncia.Estado})";
            item.ElementoRelacionadoIcono = "flag";
            if (esModerador)
                item.ElementoRelacionadoEnlace = Url.Action("Lista", "Denuncia");
        }

        return item;
    }
}
