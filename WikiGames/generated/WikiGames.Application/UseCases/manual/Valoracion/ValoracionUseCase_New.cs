using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Valoracion_new) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Valoracion;

public sealed partial class ValoracionUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Valoracion> NewAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, float p_puntuacion, int p_usuarioCreador_oid, int p_videojuego_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Valoracion_new) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ValoracionService valoracionService = null!;
        global::WikiGames.Domain.Entities.Valoracion result = default(global::WikiGames.Domain.Entities.Valoracion)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ValoracionUseCase.NewAsync");
            valoracionService = new ValoracionService(_unitOfWork);

            global::WikiGames.Domain.Entities.Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
                ?? throw new ModelException("Usuario not found for Valoracion.UsuarioCreador.");

            global::WikiGames.Domain.Entities.Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
                ?? throw new ModelException("Videojuego not found for Valoracion.Videojuego.");

            global::WikiGames.Domain.Entities.Valoracion entity = new global::WikiGames.Domain.Entities.Valoracion
            {
                Texto = p_texto,
                FechaCreacion = p_fechaCreacion,
                NumeroMeGusta = p_numeroMeGusta,
                Puntuacion = p_puntuacion,
                UsuarioCreador = usuarioCreadorEntity,
                Videojuego = videojuegoEntity,
            };

            await _unitOfWork.ValoracionRepository.SaveAsync(entity);
            result = entity;

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Valoracion.new.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
