using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Videojuego_aprobarVideojuego) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Videojuego;

public sealed partial class VideojuegoUseCase
{
    public async Task AprobarVideojuegoAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Videojuego_aprobarVideojuego) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        VideojuegoService videojuegoService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("VideojuegoUseCase.AprobarVideojuegoAsync");
            videojuegoService = new VideojuegoService(_unitOfWork);

            global::WikiGames.Domain.Entities.Videojuego videojuego = await _unitOfWork.VideojuegoRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe el videojuego {p_oid}.");

            if (videojuego.Estado != EstadoContenido.Pendiente)
            {
                throw new ModelException($"El videojuego {p_oid} no está pendiente de aprobación.");
            }

            videojuego.Estado = EstadoContenido.Aprobado;
            await _unitOfWork.VideojuegoRepository.SaveAsync(videojuego);

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"¡Enhorabuena! Tu videojuego \"{videojuego.Titulo}\" ha sido aprobado. Ahora es público y otros usuarios pueden verlo.",
                DateTime.Now,
                new[] { videojuego.UsuarioCreador.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Videojuego.aprobarVideojuego.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
