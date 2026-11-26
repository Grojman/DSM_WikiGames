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
using WikiGames.Web.Utils.Security;
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

    /// <summary>videojuego.html · Ficha completa del videojuego (vista pública, autor y moderador).</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Ficha(int id)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();

        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        var model = new FichaVideojuegoViewModel
        {
            Videojuego = VideojuegoAssembler.ToViewModelConCategorias(entity),
            AutorEmail = entity.UsuarioCreador?.Email,
            EstaAutenticado = usuarioId is not null,
            TieneMeGusta = usuarioId is int uidMg && entity.UsuariosMeGusta.Any(u => u.Id == uidMg),
            EsFavorito = usuarioId is int uidFav && entity.UsuarioFavorito.Any(u => u.Id == uidFav),
            YaDenunciado = usuarioId is int uidDen && entity.Denuncias.Any(d => d.Denunciante?.Id == uidDen && d.Estado == EstadoContenido.Pendiente),
            EsDueno = usuarioId is int uidDueno && entity.UsuarioCreador?.Id == uidDueno,
            EsModerador = User.IsUsuario<ModeradorModel>(),
            RechazoId = entity.Rechazo?.Id,
            RechazoMotivo = entity.Rechazo?.Motivo,
            RechazoFecha = entity.Rechazo?.Fecha,
            RechazoGestor = entity.Rechazo?.Gestor?.Name,
        };
        return View(model);
    }

    /// <summary>Alterna el Me gusta del usuario actual sobre un videojuego.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AlternarMeGusta(int id)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();

        var yaTieneMeGusta = entity.UsuariosMeGusta.Any(u => u.Id == usuarioId.Value);
        try
        {
            if (yaTieneMeGusta)
            {
                await unitOfWork.BeginTransactionAsync("MVC.Videojuego.RetirarMeGusta");
                await new ContenidoService(unitOfWork).RetirarMeGustaAsync(id, usuarioId.Value);
                await unitOfWork.CommitAsync();
                TempData["Toast"] = "Me gusta retirado";
            }
            else
            {
                var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
                await useCase.DarMeGustaAsync(id, usuarioId.Value);
                TempData["Toast"] = "Has dado Me gusta a este videojuego";
            }
        }
        catch (ModelException error)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogWarning(error, "Could not toggle like for Videojuego {Id}.", id);
            TempData["Toast"] = "No se ha podido actualizar el Me gusta";
        }

        return RedirectToAction(nameof(Ficha), new { id });
    }

    /// <summary>Alterna el estado de Favorito del usuario actual sobre un videojuego.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AlternarFavorito(int id)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();

        var usuario = await unitOfWork.UsuarioRepository.ReadAsync(usuarioId.Value);
        if (usuario is null) return Challenge();

        var yaEsFavorito = usuario.ContenidoFavorito.Any(c => c.Id == id);
        try
        {
            await unitOfWork.BeginTransactionAsync("MVC.Videojuego.AlternarFavorito");
            if (yaEsFavorito)
            {
                var existente = usuario.ContenidoFavorito.FirstOrDefault(c => c.Id == id);
                if (existente is not null) usuario.ContenidoFavorito.Remove(existente);
                entity.UsuarioFavorito.Remove(usuario);
                await unitOfWork.UsuarioRepository.SaveAsync(usuario);
                await unitOfWork.VideojuegoRepository.SaveAsync(entity);
                TempData["Toast"] = "Eliminado de favoritos";
            }
            else
            {
                await new ContenidoService(unitOfWork).AddUsuarioFavoritoAsync(id, new[] { usuarioId.Value });
                TempData["Toast"] = "Añadido a favoritos";
            }
            await unitOfWork.CommitAsync();
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogError(error, "Could not toggle favorite for Videojuego {Id}.", id);
            TempData["Toast"] = "No se ha podido actualizar favoritos";
        }

        return RedirectToAction(nameof(Ficha), new { id });
    }

    /// <summary>Crea una denuncia sobre el videojuego actual (DenunciaUseCase.NewCustomized).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Denunciar(int id)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null || !PuedeVerVideojuego(entity)) return NotFound();

        if (entity.Denuncias.Any(d => d.Denunciante?.Id == usuarioId.Value && d.Estado == EstadoContenido.Pendiente))
        {
            TempData["Toast"] = "Ya tienes una denuncia pendiente sobre este videojuego";
            return RedirectToAction(nameof(Ficha), new { id });
        }

        try
        {
            var denunciaUseCase = new WikiGames.Application.UseCases.Denuncia.DenunciaUseCase(unitOfWork);
            var denuncia = await denunciaUseCase.NewCustomizedAsync(usuarioId.Value);

            await unitOfWork.BeginTransactionAsync("MVC.Videojuego.Denunciar.Asociar");
            denuncia.ContenidoDenunciado = entity;
            denuncia.Denunciado = entity.UsuarioCreador;
            await unitOfWork.DenunciaRepository.SaveAsync(denuncia);
            await unitOfWork.CommitAsync();

            TempData["Toast"] = "Denuncia enviada para revisión";
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogError(error, "Could not report Videojuego {Id}.", id);
            TempData["Toast"] = "No se ha podido enviar la denuncia";
        }

        return RedirectToAction(nameof(Ficha), new { id });
    }

    /// <summary>Formulario de edición del videojuego para su autor o un moderador.</summary>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null) return NotFound();
        if (entity.UsuarioCreador?.Id != usuarioId.Value && !User.IsUsuario<ModeradorModel>()) return Forbid();

        var categorias = await unitOfWork.CategoriaRepository.ReadAllAsync();
        var model = new EditarVideojuegoViewModel
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Texto = entity.Texto,
            Enlace = entity.Enlace,
            ImagenActual = entity.Imagen,
            CategoriaIds = entity.Categorias.Select(c => c.Id).ToList(),
            Categorias = WikiGamesPaginas.ArbolCategorias(categorias),
        };
        return View(model);
    }

    /// <summary>Guarda los cambios de la ficha de un videojuego (autor o moderador).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EditarVideojuegoViewModel model,
        [FromServices] IWebHostEnvironment environment, [FromServices] IConfiguration configuration)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null) return NotFound();
        if (entity.UsuarioCreador?.Id != usuarioId.Value && !User.IsUsuario<ModeradorModel>()) return Forbid();

        var categorias = await unitOfWork.CategoriaRepository.ReadAllAsync();
        model.Id = id;
        model.ImagenActual = entity.Imagen;
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

        try
        {
            var nuevaImagen = model.Imagen is null ? null
                : await WikiGamesPaginas.GuardarImagenAsync(model.Imagen, environment, configuration, "Videojuego", "Imagen", entity.Id, HttpContext.RequestAborted);

            await unitOfWork.BeginTransactionAsync("MVC.Videojuego.Editar");
            entity.Titulo = model.Titulo.Trim();
            entity.Texto = model.Texto?.Trim() ?? string.Empty;
            entity.Enlace = model.Enlace?.Trim() ?? string.Empty;
            if (nuevaImagen is not null) entity.Imagen = nuevaImagen;

            entity.Categorias.Clear();
            foreach (var categoria in categorias.Where(c => model.CategoriaIds.Contains(c.Id)))
                entity.Categorias.Add(categoria);

            await unitOfWork.VideojuegoRepository.SaveAsync(entity);
            await unitOfWork.CommitAsync();
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogError(error, "Could not update Videojuego {Id}.", id);
            ModelState.AddModelError(string.Empty, "No se han podido guardar los cambios. Inténtalo de nuevo.");
            model.Categorias = WikiGamesPaginas.ArbolCategorias(categorias);
            return View(model);
        }

        TempData["Toast"] = $"«{entity.Titulo}» actualizado";
        return RedirectToAction(nameof(Ficha), new { id });
    }

    /// <summary>Elimina un videojuego (su autor o un moderador/administrador).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarFicha(int id, string? origen)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null) return NotFound();

        var esDueno = entity.UsuarioCreador?.Id == usuarioId.Value;
        var esModerador = User.IsUsuario<ModeradorModel>();
        if (!esDueno && !esModerador) return Forbid();

        var titulo = entity.Titulo;
        try
        {
            async Task LimpiarContenidoAsync(Contenido contenido)
            {
                foreach (var u in contenido.UsuariosMeGusta.ToList())
                {
                    u.ContenidosMeGusta.Remove(contenido);
                    await unitOfWork.UsuarioRepository.SaveAsync(u);
                }
                contenido.UsuariosMeGusta.Clear();

                foreach (var u in contenido.UsuarioFavorito.ToList())
                {
                    u.ContenidoFavorito.Remove(contenido);
                    await unitOfWork.UsuarioRepository.SaveAsync(u);
                }
                contenido.UsuarioFavorito.Clear();

                foreach (var d in contenido.Denuncias.ToList())
                {
                    d.ContenidoDenunciado = null;
                    await unitOfWork.DenunciaRepository.SaveAsync(d);
                }
                contenido.Denuncias.Clear();

                foreach (var a in contenido.Acciones.ToList())
                    await unitOfWork.AccionSistemaRepository.DeleteAsync(a);
                contenido.Acciones.Clear();
            }

            await unitOfWork.BeginTransactionAsync("MVC.Videojuego.LimpiarRelaciones");
            await LimpiarContenidoAsync(entity);
            entity.Categorias.Clear();

            foreach (var valoracion in entity.Valoraciones.ToList())
                await LimpiarContenidoAsync(valoracion);

            foreach (var tema in entity.Temas.ToList())
            {
                foreach (var comentario in tema.Comentarios.ToList())
                {
                    await LimpiarContenidoAsync(comentario);
                    comentario.ComentarioAsociado = null;
                    await unitOfWork.ComentarioRepository.SaveAsync(comentario);
                }
                foreach (var comentario in tema.Comentarios.ToList())
                    await unitOfWork.ComentarioRepository.DeleteAsync(comentario);
                await LimpiarContenidoAsync(tema);
                await unitOfWork.TemaConversacionRepository.DeleteAsync(tema);
            }
            entity.Temas.Clear();

            var rechazo = entity.Rechazo;
            if (rechazo is not null)
            {
                entity.Rechazo = null;
                await unitOfWork.VideojuegoRepository.SaveAsync(entity);
                await unitOfWork.RechazoRepository.DeleteAsync(rechazo);
            }
            else
            {
                await unitOfWork.VideojuegoRepository.SaveAsync(entity);
            }
            await unitOfWork.CommitAsync();

            if (esModerador && !esDueno)
            {
                var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
                await useCase.DestroyAsync(id);
            }
            else
            {
                await unitOfWork.BeginTransactionAsync("MVC.Videojuego.Eliminar");
                await new VideojuegoService(unitOfWork).DestroyAsync(id);
                await unitOfWork.CommitAsync();
            }
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogError(error, "Could not delete Videojuego {Id}.", id);
            TempData["Toast"] = $"No se ha podido eliminar «{titulo}»";
            return RedirectToAction(nameof(Ficha), new { id });
        }

        TempData["Toast"] = $"Videojuego «{titulo}» eliminado";
        return string.Equals(origen, "pendientes", StringComparison.OrdinalIgnoreCase)
            ? RedirectToAction(nameof(Pendientes))
            : RedirectToAction("Index", "Home");
    }

    /// <summary>videojuegos-pendientes.html · Listado de videojuegos pendientes de aprobación. Moderadores y administradores.</summary>
    [HttpGet]
    [Authorize(Roles = UserRoles.Moderador)]
    public async Task<IActionResult> Pendientes(string? q)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var filtro = q?.Trim();
        var videojuegos = (await unitOfWork.VideojuegoRepository.ReadAllAsync())
            .Where(v => v.Estado == EstadoContenido.Pendiente);

        if (!string.IsNullOrEmpty(filtro))
        {
            videojuegos = videojuegos.Where(v =>
                v.Titulo.Contains(filtro, StringComparison.CurrentCultureIgnoreCase)
                || v.Texto.Contains(filtro, StringComparison.CurrentCultureIgnoreCase)
                || (v.UsuarioCreador?.Name?.Contains(filtro, StringComparison.CurrentCultureIgnoreCase) ?? false)
                || (v.UsuarioCreador?.Email?.Contains(filtro, StringComparison.CurrentCultureIgnoreCase) ?? false));
        }

        var model = new VideojuegosPendientesViewModel
        {
            Q = filtro,
            Videojuegos = videojuegos
                .OrderByDescending(v => v.FechaCreacion)
                .ThenByDescending(v => v.Id)
                .Select(v => new VideojuegoPendienteItemViewModel
                {
                    Videojuego = VideojuegoAssembler.ToViewModelConCategorias(v),
                    AutorEmail = v.UsuarioCreador?.Email,
                })
                .ToList(),
        };
        return View(model);
    }

    /// <summary>Aprueba un videojuego pendiente (VideojuegoUseCase.aprobarVideojuego).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = UserRoles.Moderador)]
    public async Task<IActionResult> Aprobar(int id, string? origen, string? q)
    {
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null) return NotFound();
        var titulo = entity.Titulo;

        try
        {
            var useCase = new WikiGames.Application.UseCases.Videojuego.VideojuegoUseCase(unitOfWork);
            await useCase.AprobarVideojuegoAsync(id);
            TempData["Toast"] = $"«{titulo}» aprobado y publicado";
        }
        catch (ModelException error)
        {
            _logger.LogWarning(error, "Could not approve Videojuego {Id}.", id);
            TempData["Toast"] = error.InnerException is ModelException regla ? regla.Message : $"No se ha podido aprobar «{titulo}»";
        }

        return string.Equals(origen, "ficha", StringComparison.OrdinalIgnoreCase)
            ? RedirectToAction(nameof(Ficha), new { id })
            : RedirectToAction(nameof(Pendientes), new { q });
    }

    /// <summary>Rechaza un videojuego pendiente con motivo obligatorio (VideojuegoUseCase.rechazarVideojuego).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = UserRoles.Moderador)]
    public async Task<IActionResult> Rechazar(int id, string? motivo, string? origen, string? q)
    {
        var gestorId = WikiGamesPaginas.UsuarioActualId(User);
        if (gestorId is null) return Challenge();

        if (string.IsNullOrWhiteSpace(motivo))
        {
            TempData["Toast"] = "Debes indicar un motivo de rechazo";
            return string.Equals(origen, "ficha", StringComparison.OrdinalIgnoreCase)
                ? RedirectToAction(nameof(Ficha), new { id })
                : RedirectToAction(nameof(Pendientes), new { q });
        }

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.VideojuegoRepository.ReadAsync(id);
        if (entity is null) return NotFound();
        var titulo = entity.Titulo;

        try
        {
            var useCase = new WikiGames.Application.UseCases.Videojuego.VideojuegoUseCase(unitOfWork);
            await useCase.RechazarVideojuegoAsync(id, motivo.Trim(), gestorId.Value);
            TempData["Toast"] = $"«{titulo}» rechazado";
        }
        catch (ModelException error)
        {
            _logger.LogWarning(error, "Could not reject Videojuego {Id}.", id);
            TempData["Toast"] = error.InnerException is ModelException regla ? regla.Message : $"No se ha podido rechazar «{titulo}»";
        }

        return string.Equals(origen, "ficha", StringComparison.OrdinalIgnoreCase)
            ? RedirectToAction(nameof(Ficha), new { id })
            : RedirectToAction(nameof(Pendientes), new { q });
    }
}
