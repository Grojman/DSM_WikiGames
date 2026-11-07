using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Comentario_addRespuestaTransaction) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Comentario;

public sealed partial class ComentarioUseCase
{
    public async Task AddRespuestaTransactionAsync(int p_oid, IReadOnlyList<int> p_comentario2_oids)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Comentario_addRespuestaTransaction) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ComentarioService comentarioService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ComentarioUseCase.AddRespuestaTransactionAsync");
            comentarioService = new ComentarioService(_unitOfWork);

            global::WikiGames.Domain.Entities.Comentario comentario = await _unitOfWork.ComentarioRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe el comentario {p_oid}.");

            if (p_comentario2_oids is null || p_comentario2_oids.Count == 0)
            {
                throw new ModelException("Hay que indicar al menos una respuesta.");
            }

            if (p_comentario2_oids.Contains(p_oid))
            {
                throw new ModelException("Un comentario no puede responderse a sí mismo.");
            }

            // Relationer Comentario.addRespuesta.
            await comentarioService.AddRespuestaAsync(p_oid, p_comentario2_oids);

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            string texto =  "Alguien ha respondido a tu comentario.";
            await notificacionUseCase.NewAsync(
                $"{texto} Comentario: \"{comentario.Texto}\"",
                DateTime.Now,
                new[] { comentario.UsuarioCreador.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Comentario.addRespuestaTransaction.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
