using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Valoracion_actualizarValoracion) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Valoracion;

public sealed partial class ValoracionUseCase
{
    public async Task ActualizarValoracionAsync(int p_oid, float nuevo_valor)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Valoracion_actualizarValoracion) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ValoracionService valoracionService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ValoracionUseCase.ActualizarValoracionAsync");
            valoracionService = new ValoracionService(_unitOfWork);
            // Recalcula la puntuación media almacenada en el videojuego.
           
            global::WikiGames.Domain.Entities.Valoracion valoracion = await _unitOfWork.ValoracionRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe la valoración {p_oid}.");

            valoracion.Puntuacion = nuevo_valor;
            await _unitOfWork.ValoracionRepository.SaveAsync(valoracion);

            global::WikiGames.Domain.Entities.Videojuego videojuego = valoracion.Videojuego;
            videojuego.PuntuacionMedia = videojuego.Valoraciones.Count == 0 ? 0f : videojuego.Valoraciones.Average(x => x.Puntuacion);
            await _unitOfWork.VideojuegoRepository.SaveAsync(videojuego);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Valoracion.actualizarValoracion.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
