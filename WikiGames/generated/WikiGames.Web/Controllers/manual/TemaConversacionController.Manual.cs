// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(TemaConversacionController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;

using WikiGames.Domain.Enums;
using WikiGames.Application.Exceptions;
using WikiGames.Web.Utils.Paginas;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(TemaConversacionController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class TemaConversacionController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_DarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(TemaConversacionController_DarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
        await useCase.DarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TemaConversacionController_RetirarMeGusta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RetirarMeGusta(int p_oid, int p_user)
    {
    /*PROTECTED REGION ID(TemaConversacionController_RetirarMeGusta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _contenidoService.RetirarMeGustaAsync(p_oid, p_user);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    /// <summary>
    /// Ordena los comentarios de un tema como árbol: cada comentario seguido de sus respuestas
    /// (recursivo), con su nivel de profundidad para tabular las respuestas en la vista.
    /// </summary>
    private static List<WikiGames.Web.ViewModels.ComentarioViewModel> ArbolComentarios(IEnumerable<Comentario> comentarios, int? usuarioId)
    {
        var all = comentarios.ToList();
        var ids = all.Select(c => c.Id).ToHashSet();
        var result = new List<WikiGames.Web.ViewModels.ComentarioViewModel>();
        var visitados = new HashSet<int>();

        void Add(Comentario c, int nivel)
        {
            if (!visitados.Add(c.Id)) return; // protección frente a ciclos
            var vm = ComentarioAssembler.ToViewModel(c);
            vm.Nivel = nivel;
            vm.TieneMeGusta = usuarioId is int uid && c.UsuariosMeGusta.Any(u => u.Id == uid);
            result.Add(vm);
            foreach (var hija in all.Where(h => h.ComentarioAsociado?.Id == c.Id).OrderBy(h => h.FechaCreacion).ThenBy(h => h.Id))
                Add(hija, nivel + 1);
        }

        // Raíces: sin comentario padre, o cuyo padre no pertenece a este tema.
        foreach (var raiz in all.Where(c => c.ComentarioAsociado is null || !ids.Contains(c.ComentarioAsociado.Id))
                                .OrderBy(c => c.FechaCreacion).ThenBy(c => c.Id))
            Add(raiz, 0);

        return result;
    }

    /// <summary>Crea una respuesta del usuario actual a un comentario del tema.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponderComentario(int id, int comentarioId, string? texto)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        texto = texto?.Trim();
        if (string.IsNullOrEmpty(texto))
        {
            TempData["Toast"] = "La respuesta no puede estar vacía";
            return RedirectToAction(nameof(Details), new { id });
        }

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var padre = await unitOfWork.ComentarioRepository.ReadAsync(comentarioId);
        if (padre is null || padre.Tema?.Id != id) return NotFound();

        var comentarioService = new ComentarioService(unitOfWork);
        Comentario respuesta;
        try
        {
            await unitOfWork.BeginTransactionAsync("MVC.TemaConversacion.ResponderComentario");
            respuesta = await comentarioService.NewAsync(texto, DateTime.Now, 0, id, usuarioId.Value);
            await unitOfWork.CommitAsync();
        }
        catch (ModelException error)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogWarning(error, "Could not create reply to Comentario {Id}.", comentarioId);
            TempData["Toast"] = "No se ha podido publicar la respuesta";
            return RedirectToAction(nameof(Details), new { id });
        }

        try
        {
            // Enlaza la respuesta con su comentario padre y notifica al autor del comentario.
            var useCase = new WikiGames.Application.UseCases.Comentario.ComentarioUseCase(unitOfWork);
            await useCase.AddRespuestaTransactionAsync(comentarioId, new[] { respuesta.Id });
        }
        catch (ModelException error)
        {
            _logger.LogWarning(error, "Could not link reply {RespuestaId} to Comentario {Id}.", respuesta.Id, comentarioId);
            // Evita dejar la respuesta huérfana como comentario raíz.
            await unitOfWork.BeginTransactionAsync("MVC.TemaConversacion.ResponderComentario.Deshacer");
            await comentarioService.DestroyAsync(respuesta.Id);
            await unitOfWork.CommitAsync();
            TempData["Toast"] = "No se ha podido publicar la respuesta";
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["Toast"] = "Respuesta publicada";
        return Redirect(Url.Action(nameof(Details), new { id }) + $"#comentario-{respuesta.Id}");
    }

    /// <summary>Alterna el Me gusta del usuario actual sobre un comentario del tema.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AlternarMeGustaComentario(int id, int comentarioId)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var comentario = await unitOfWork.ComentarioRepository.ReadAsync(comentarioId);
        if (comentario is null || comentario.Tema?.Id != id) return NotFound();

        var yaTieneMeGusta = comentario.UsuariosMeGusta.Any(u => u.Id == usuarioId.Value);
        try
        {
            if (yaTieneMeGusta)
            {
                await unitOfWork.BeginTransactionAsync("MVC.Comentario.RetirarMeGusta");
                await new ContenidoService(unitOfWork).RetirarMeGustaAsync(comentarioId, usuarioId.Value);
                await unitOfWork.CommitAsync();
            }
            else
            {
                var useCase = new WikiGames.Application.UseCases.Contenido.ContenidoUseCase(unitOfWork);
                await useCase.DarMeGustaAsync(comentarioId, usuarioId.Value);
            }
        }
        catch (ModelException error)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogWarning(error, "Could not toggle like for Comentario {Id}.", comentarioId);
            TempData["Toast"] = "No se ha podido actualizar el Me gusta";
        }

        return Redirect(Url.Action(nameof(Details), new { id }) + $"#comentario-{comentarioId}");
    }
}
