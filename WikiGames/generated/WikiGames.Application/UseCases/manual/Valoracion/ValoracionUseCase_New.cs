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
    public async Task<global::WikiGames.Domain.Entities.Valoracion> NewAsync(string p_texto, float p_puntuacion, int p_usuarioCreador_oid, int p_videojuego_oid)
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
                FechaCreacion = DateTime.Now,
                NumeroMeGusta = 0,
                Puntuacion = p_puntuacion,
                UsuarioCreador = usuarioCreadorEntity,
                Videojuego = videojuegoEntity,
            };

            await _unitOfWork.ValoracionRepository.SaveAsync(entity);
            if (!videojuegoEntity.Valoraciones.Contains(entity))
            {
                videojuegoEntity.Valoraciones.Add(entity);
            }

            //He copiado esta función tres veces en los casos de uso. Si tan solo se pudiera utilizar clases estáticas para  no tener que repetir código...
            videojuegoEntity.PuntuacionMedia = videojuegoEntity.Valoraciones.Count == 0 ? 0f : videojuegoEntity.Valoraciones.Average(x => x.Puntuacion);
            await _unitOfWork.VideojuegoRepository.SaveAsync(videojuegoEntity);
            result = entity;

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"Tu videojuego \"{videojuegoEntity.Titulo}\" ha recibido una nueva valoración de {usuarioCreadorEntity.Name}: {p_puntuacion}.",
                DateTime.Now,
                new[] { videojuegoEntity.UsuarioCreador.Id });

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
