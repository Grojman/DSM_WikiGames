// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(VideojuegoController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(VideojuegoController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class VideojuegoController
{
    [HttpGet]
    /*PROTECTED REGION ID(VideojuegoController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(VideojuegoController_Create_GET_body) ENABLED START*/

        var model = new WikiGames.Web.ViewModels.VideojuegoViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.VideojuegoViewModel model)
    {
    /*PROTECTED REGION ID(VideojuegoController_Create_POST_body) ENABLED START*/

        ModelState.Remove(nameof(model.Estado)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.PuntuacionMedia)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.FechaCreacion)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.NumeroMeGusta)); // Campo fuera del contrato Create de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new global::WikiGames.Application.UseCases.Videojuego.VideojuegoUseCase(unitOfWork);

        model.Valoraciones ??= new();
        if (model.Valoraciones.Count > 500) return BadRequest("Número de líneas no válido (máximo 500).");
        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.RechazoId is int selectedRechazo &&
            !model.RechazoOptions.Any(option => option.Value == Convert.ToString(selectedRechazo, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("RechazoId", "Selecciona un registro existente de Rechazo.");

        if (model.UsuarioCreadorId is int selectedUsuarioCreador &&
            !model.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (model.UsuarioCreadorId is null) ModelState.AddModelError("UsuarioCreadorId", "Debes seleccionar una opción.");
        if (model.Valoraciones is null) { model.Valoraciones = new(); ModelState.AddModelError(nameof(model.Valoraciones), "Debes indicar las líneas."); }
        if (model.Valoraciones.Count < 0 || model.Valoraciones.Count > 500)
            ModelState.AddModelError(nameof(model.Valoraciones), "Número de líneas no válido (máximo 500).");

        for (var index = 0; index < model.Valoraciones.Count; index++)
        {
            var row = model.Valoraciones[index];
            if (!EqualityComparer<int>.Default.Equals(row.Id, default!))
                ModelState.AddModelError($"Valoraciones[{index}].Id", "La línea no pertenece a este registro o está repetida.");
        if (row.UsuarioCreadorId is int selectedRowValoracionesUsuarioCreador &&
            !row.UsuarioCreadorOptions.Any(option => option.Value == Convert.ToString(selectedRowValoracionesUsuarioCreador, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError($"Valoraciones[{index}].UsuarioCreadorId", "Selecciona un registro existente de Usuario.");
        if (row.UsuarioCreadorId is null) ModelState.AddModelError($"Valoraciones[{index}].UsuarioCreadorId", "Debes seleccionar una opción.");
        }

        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Videojuego.Create");
        try
        {
            var submittedValoraciones = new List<WikiGames.Domain.Entities.Valoracion>();
            foreach (var row in model.Valoraciones)
            {
                var child = ValoracionAssembler.Create(row);
                ValoracionAssembler.ApplyToEntity(row, child);
            child.UsuarioCreador = row.UsuarioCreadorId is int referenceRowValoracionesUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceRowValoracionesUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
                submittedValoraciones.Add(child);
            }
            var entity = await service.NewCustomizedAsync(model.Texto, model.Titulo, model.Enlace, model.Imagen, model.UsuarioCreadorId.GetValueOrDefault());
            entity.Rechazo = model.RechazoId is int referenceRechazo
                ? await unitOfWork.RechazoRepository.ReadAsync(referenceRechazo) ?? throw new InvalidOperationException("El registro seleccionado de Rechazo ya no existe.") : null!;
            entity.UsuarioCreador = model.UsuarioCreadorId is int referenceUsuarioCreador
                ? await unitOfWork.UsuarioRepository.ReadAsync(referenceUsuarioCreador) ?? throw new InvalidOperationException("El registro seleccionado de Usuario ya no existe.") : null!;
            
            for (var index = 0; index < submittedValoraciones.Count; index++)
            {
                var child = submittedValoraciones[index];
                child.Videojuego = entity;
                if (!entity.Valoraciones.Contains(child)) entity.Valoraciones.Add(child);
            }
            await unitOfWork.VideojuegoRepository.SaveAsync(entity);

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
    /*PROTECTED REGION ID(VideojuegoController_AprobarVideojuego_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AprobarVideojuego(int p_oid)
    {
    /*PROTECTED REGION ID(VideojuegoController_AprobarVideojuego_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Videojuego.VideojuegoUseCase(unitOfWork);
        await useCase.AprobarVideojuegoAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_RechazarVideojuego_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RechazarVideojuego(int p_oid, string p_motivo, int p_gestor_oid)
    {
    /*PROTECTED REGION ID(VideojuegoController_RechazarVideojuego_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Videojuego.VideojuegoUseCase(unitOfWork);
        await useCase.RechazarVideojuegoAsync(p_oid, p_motivo, p_gestor_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_DarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(VideojuegoController_DarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
        await useCase.DarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(VideojuegoController_RetirarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RetirarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(VideojuegoController_RetirarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.RetirarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    // =====================================================================
    // Páginas WikiGames (migradas desde WikiGames.HTML). Acciones enteramente
    // manuales: HARIA las conserva al regenerar.
    // =====================================================================

    /// <summary>conversaciones.html · Temas de conversación de un videojuego. Público.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Conversaciones(int id)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();

        var model = new ConversacionesJuegoViewModel
        {
            Videojuego = VideojuegoAssembler.ToViewModelConCategorias(entity),
            Temas = entity.Temas
                .OrderByDescending(t => t.FechaCreacion)
                .Select(t =>
                {
                    var tema = TemaConversacionAssembler.ToViewModel(t);
                    tema.NumeroRespuestas = t.Comentarios.Count;
                    return tema;
                })
                .ToList(),
        };
        return View(model);
    }

    /// <summary>comentarios.html · Valoraciones de un videojuego. Público; publicar requiere sesión.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Valoraciones(int id)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();
        return View(BuildValoraciones(entity, new NuevaValoracionViewModel()));
    }

    /// <summary>Publica una Valoracion (ValoracionUseCase.New). Estrellas 1–5 → puntuación 2–10.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Valorar(int id, [Bind(Prefix = "Nueva")] NuevaValoracionViewModel nueva)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();
        if (string.IsNullOrWhiteSpace(nueva.Texto)) ModelState.AddModelError("Nueva.Texto", "Escribe tu comentario.");
        if (!ModelState.IsValid) return View(nameof(Valoraciones), BuildValoraciones(entity, nueva));

        try
        {
            var useCase = new WikiGames.Application.UseCases.Valoracion.ValoracionUseCase(unitOfWork);
            await useCase.NewAsync(nueva.Texto.Trim(), nueva.Estrellas * 2f, usuarioId.Value, id);
        }
        catch (ModelException error)
        {
            _logger.LogError(error, "Could not create the Valoracion.");
            ModelState.AddModelError(string.Empty, "No se ha podido publicar el comentario. Inténtalo de nuevo.");
            return View(nameof(Valoraciones), BuildValoraciones(entity, nueva));
        }

        TempData["Toast"] = "Comentario publicado";
        return RedirectToAction(nameof(Valoraciones), new { id });
    }

    /// <summary>crear-videojuego.html · Formulario de solicitud de un videojuego nuevo.</summary>
    [HttpGet]
    public async Task<IActionResult> Nuevo()
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var model = new NuevoVideojuegoViewModel
        {
            Categorias = WikiGamesPaginas.ArbolCategorias(await unitOfWork.CategoriaRepository.ReadAllAsync()),
        };
        return View(model);
    }

    /// <summary>
    /// Crea el videojuego en estado Pendiente (VideojuegoUseCase.NewCustomized) y después, ya con Id,
    /// guarda la imagen en wwwroot/uploads/Videojuego/Imagen/{id}/ y asocia las categorías.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(NuevoVideojuegoViewModel model,
        [FromServices] IWebHostEnvironment environment, [FromServices] IConfiguration configuration)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var categorias = await unitOfWork.CategoriaRepository.ReadAllAsync();
        model.CategoriaIds = model.CategoriaIds.Distinct().ToList();
        if (model.CategoriaIds.Any(cid => categorias.All(c => c.Id != cid)))
            ModelState.AddModelError(nameof(model.CategoriaIds), "Selecciona categorías existentes.");
        if (string.IsNullOrWhiteSpace(model.Titulo))
            ModelState.AddModelError(nameof(model.Titulo), "El título es obligatorio.");
        if (model.Imagen is not null && WikiGamesPaginas.ValidarImagen(model.Imagen, configuration) is string imageError)
            ModelState.AddModelError(nameof(model.Imagen), imageError);
        if (!ModelState.IsValid)
        {
            model.Categorias = WikiGamesPaginas.ArbolCategorias(categorias);
            return View(model);
        }

        Videojuego entity;
        try
        {
            var useCase = new WikiGames.Application.UseCases.Videojuego.VideojuegoUseCase(unitOfWork);
            entity = await useCase.NewCustomizedAsync(model.Texto?.Trim() ?? string.Empty, model.Titulo.Trim(),
                model.Enlace?.Trim() ?? string.Empty, string.Empty, usuarioId.Value);
        }
        catch (ModelException error)
        {
            _logger.LogError(error, "Could not create the Videojuego.");
            ModelState.AddModelError(string.Empty, "No se ha podido enviar la solicitud. Inténtalo de nuevo.");
            model.Categorias = WikiGamesPaginas.ArbolCategorias(categorias);
            return View(model);
        }

        if (model.Imagen is not null || model.CategoriaIds.Count > 0)
        {
            try
            {
                var imagen = model.Imagen is null ? null
                    : await WikiGamesPaginas.GuardarImagenAsync(model.Imagen, environment, configuration, "Videojuego", "Imagen", entity.Id, HttpContext.RequestAborted);
                await unitOfWork.BeginTransactionAsync("MVC.Videojuego.Nuevo.ImagenCategorias");
                if (imagen is not null) entity.Imagen = imagen;
                foreach (var categoria in categorias.Where(c => model.CategoriaIds.Contains(c.Id)))
                    if (!entity.Categorias.Contains(categoria)) entity.Categorias.Add(categoria);
                await unitOfWork.VideojuegoRepository.SaveAsync(entity);
                await unitOfWork.CommitAsync();
            }
            catch (Exception error) when (error is not OperationCanceledException)
            {
                await unitOfWork.RollbackAsync();
                _logger.LogError(error, "Videojuego {Id} created, but its image or categories could not be saved.", entity.Id);
                TempData["Toast"] = $"«{entity.Titulo}» enviado, pero no se han podido guardar la imagen o las categorías";
                return RedirectToAction(nameof(Nuevo));
            }
        }

        TempData["Toast"] = $"«{entity.Titulo}» enviado para revisión";
        return RedirectToAction(nameof(Nuevo));
    }

    /// <summary>Un videojuego no aprobado solo lo ven su creador y los moderadores/administradores.</summary>
    private bool PuedeVerVideojuego(Videojuego entity) =>
        entity.Estado == EstadoContenido.Aprobado
        || User.IsUsuario<ModeradorModel>()
        || (WikiGamesPaginas.UsuarioActualId(User) is int usuarioId && entity.UsuarioCreador?.Id == usuarioId);

    private static ValoracionesJuegoViewModel BuildValoraciones(Videojuego entity, NuevaValoracionViewModel nueva) => new()
    {
        Videojuego = VideojuegoAssembler.ToViewModelConCategorias(entity),
        Valoraciones = entity.Valoraciones
            .OrderByDescending(v => v.FechaCreacion)
            .Select(ValoracionAssembler.ToViewModel)
            .ToList(),
        Nueva = nueva,
    };
}
