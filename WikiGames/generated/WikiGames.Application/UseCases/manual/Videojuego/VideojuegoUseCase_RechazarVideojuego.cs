using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Videojuego_rechazarVideojuego) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Videojuego;

public sealed partial class VideojuegoUseCase
{
    public async Task RechazarVideojuegoAsync(int p_oid, string p_motivo, int p_gestor_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Videojuego_rechazarVideojuego) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        VideojuegoService videojuegoService = null!;
        RechazoService rechazoService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("VideojuegoUseCase.RechazarVideojuegoAsync");
            videojuegoService = new VideojuegoService(_unitOfWork);
            rechazoService = new RechazoService(_unitOfWork);

            if (string.IsNullOrWhiteSpace(p_motivo))
            {
                throw new ModelException("El motivo del rechazo es obligatorio.");
            }

            global::WikiGames.Domain.Entities.Videojuego videojuego = await _unitOfWork.VideojuegoRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe el videojuego {p_oid}.");

            if (videojuego.Estado != EstadoContenido.Pendiente)
            {
                throw new ModelException($"El videojuego {p_oid} no está pendiente de aprobación.");
            }

            global::WikiGames.Domain.Entities.Rechazo rechazo = await rechazoService.NewAsync(DateTime.Now, p_motivo, p_oid, p_gestor_oid);
            videojuego.Rechazo = rechazo;
            videojuego.Estado = EstadoContenido.Rechazado;
            await _unitOfWork.VideojuegoRepository.SaveAsync(videojuego);

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"Tu videojuego \"{videojuego.Titulo}\" ha sido rechazado. Motivo: {p_motivo}",
                DateTime.Now,
                new[] { videojuego.UsuarioCreador.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Videojuego.rechazarVideojuego.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
