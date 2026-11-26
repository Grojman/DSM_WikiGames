// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ContenidoController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(ContenidoController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class ContenidoController
{
    [HttpGet]
    /*PROTECTED REGION ID(ContenidoController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(ContenidoController_Create_GET_body) ENABLED START*/

        var model = new WikiGames.Web.ViewModels.ContenidoViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.ContenidoViewModel model)
    {
    /*PROTECTED REGION ID(ContenidoController_Create_POST_body) ENABLED START*/

        ModelState.Remove(nameof(model.FechaCreacion)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.NumeroMeGusta)); // Campo fuera del contrato Create de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.Services.ContenidoService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.UsuarioCreadorId is int selectedUsuarioCreador &&
            !model.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (model.UsuarioCreadorId is null) ModelState.AddModelError("UsuarioCreadorId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Contenido.Create");
        try
        {

            var entity = await service.NewCustomizedAsync(model.Texto, model.UsuarioCreadorId.GetValueOrDefault());
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;

            await service.ModifierContenidoDefaultAsync(entity);

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
    /*PROTECTED REGION ID(ContenidoController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ContenidoController_Delete_GET_body) ENABLED START*/

        var entity = await _contenidoService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ContenidoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ContenidoController_DeleteConfirmed_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.Web.ViewModels.ContenidoViewModel());
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new global::WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
        await useCase.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_DarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(ContenidoController_DarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
        await useCase.DarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ContenidoController_RetirarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RetirarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(ContenidoController_RetirarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.RetirarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    // =====================================================================
    // Página WikiGames "Me gusta" (me-gusta.html). Usuario autenticado.
    // Acciones enteramente manuales: HARIA las conserva al regenerar.
    // =====================================================================

    [HttpGet]
    public async Task<IActionResult> MeGusta(string? tipo)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        var filtro = tipo is ContenidoMeGustaViewModel.TipoVideojuegos
            or ContenidoMeGustaViewModel.TipoConversaciones
            or ContenidoMeGustaViewModel.TipoComentarios
            ? tipo
            : ContenidoMeGustaViewModel.TipoTodos;

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var contenidos = await unitOfWork.ContenidoRepository.FindBYUserLikedAsync(usuarioId.Value);

        // Contenido es una jerarquía joined-subclass: se clasifica por Id en cada subclase.
        var videojuegos = (await unitOfWork.VideojuegoRepository.ReadAllAsync()).ToDictionary(x => x.Id);
        var temas = (await unitOfWork.TemaConversacionRepository.ReadAllAsync()).ToDictionary(x => x.Id);
        var comentarios = (await unitOfWork.ComentarioRepository.ReadAllAsync()).ToDictionary(x => x.Id);
        var valoraciones = (await unitOfWork.ValoracionRepository.ReadAllAsync()).ToDictionary(x => x.Id);
        var esModerador = User.IsUsuario<ModeradorModel>();

        var elementos = new List<ContenidoMeGustaItemViewModel>();
        foreach (var c in contenidos.OrderByDescending(x => x.FechaCreacion).ThenByDescending(x => x.Id))
        {
            ContenidoMeGustaItemViewModel item;
            if (videojuegos.TryGetValue(c.Id, out var v))
            {
                if (v.Estado != EstadoContenido.Aprobado && !esModerador && v.UsuarioCreador?.Id != usuarioId.Value)
                    continue;
                item = new ContenidoMeGustaItemViewModel
                {
                    ContenidoId = c.Id,
                    Tipo = "Videojuego",
                    TipoFiltro = ContenidoMeGustaViewModel.TipoVideojuegos,
                    Titulo = v.Titulo,
                    Descripcion = v.Texto,
                    AutorNombre = v.UsuarioCreador?.Name,
                    AutorEmail = v.UsuarioCreador?.Email,
                    FechaCreacion = v.FechaCreacion,
                    Enlace = Url.Action("Ficha", "Videojuego", new { id = v.Id }),
                    EsVideojuego = true,
                };
            }
            else if (temas.TryGetValue(c.Id, out var t))
            {
                item = new ContenidoMeGustaItemViewModel
                {
                    ContenidoId = c.Id,
                    Tipo = "Conversación",
                    TipoFiltro = ContenidoMeGustaViewModel.TipoConversaciones,
                    Titulo = t.Titulo,
                    Descripcion = t.Texto,
                    AutorNombre = t.UsuarioCreador?.Name,
                    AutorEmail = t.UsuarioCreador?.Email,
                    FechaCreacion = t.FechaCreacion,
                    Enlace = t.Videojuego is not null
                        ? Url.Action("Conversaciones", "Videojuego", new { id = t.Videojuego.Id }, null, null, $"tema-{t.Id}")
                        : null,
                };
            }
            else if (comentarios.TryGetValue(c.Id, out var com))
            {
                item = new ContenidoMeGustaItemViewModel
                {
                    ContenidoId = c.Id,
                    Tipo = "Comentario",
                    TipoFiltro = ContenidoMeGustaViewModel.TipoComentarios,
                    Titulo = com.Tema is not null ? $"En «{com.Tema.Titulo}»" : $"Comentario #{com.Id}",
                    Descripcion = com.Texto,
                    AutorNombre = com.UsuarioCreador?.Name,
                    AutorEmail = com.UsuarioCreador?.Email,
                    FechaCreacion = com.FechaCreacion,
                    Enlace = com.Tema?.Videojuego is not null
                        ? Url.Action("Conversaciones", "Videojuego", new { id = com.Tema.Videojuego.Id }, null, null, $"tema-{com.Tema.Id}")
                        : null,
                };
            }
            else if (valoraciones.TryGetValue(c.Id, out var val))
            {
                item = new ContenidoMeGustaItemViewModel
                {
                    ContenidoId = c.Id,
                    Tipo = "Valoración",
                    TipoFiltro = ContenidoMeGustaViewModel.TipoComentarios,
                    Titulo = val.Videojuego is not null ? $"Valoración en {val.Videojuego.Titulo}" : $"Valoración #{val.Id}",
                    Descripcion = val.Texto,
                    AutorNombre = val.UsuarioCreador?.Name,
                    AutorEmail = val.UsuarioCreador?.Email,
                    FechaCreacion = val.FechaCreacion,
                    Enlace = val.Videojuego is not null
                        ? Url.Action("Valoraciones", "Videojuego", new { id = val.Videojuego.Id })
                        : null,
                };
            }
            else
            {
                item = new ContenidoMeGustaItemViewModel
                {
                    ContenidoId = c.Id,
                    Tipo = "Publicación",
                    TipoFiltro = "otros",
                    Titulo = $"Publicación #{c.Id}",
                    Descripcion = c.Texto,
                    AutorNombre = c.UsuarioCreador?.Name,
                    AutorEmail = c.UsuarioCreador?.Email,
                    FechaCreacion = c.FechaCreacion,
                };
            }

            if (filtro == ContenidoMeGustaViewModel.TipoTodos || item.TipoFiltro == filtro)
                elementos.Add(item);
        }

        return View(new ContenidoMeGustaViewModel
        {
            Tipo = filtro,
            Elementos = elementos,
        });
    }

    /// <summary>Retira el Me gusta del usuario actual sobre el contenido indicado.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarMeGusta(int id, string? tipo)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        try
        {
            await unitOfWork.BeginTransactionAsync("MVC.Contenido.QuitarMeGusta");
            await new ContenidoService(unitOfWork).RetirarMeGustaAsync(id, usuarioId.Value);
            await unitOfWork.CommitAsync();
            TempData["Toast"] = "Me gusta retirado";
        }
        catch (ModelException error)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogWarning(error, "Could not remove like from Contenido {Id}.", id);
            TempData["Toast"] = "No se ha podido quitar el Me gusta";
        }

        return RedirectToAction(nameof(MeGusta), new { tipo });
    }
}
