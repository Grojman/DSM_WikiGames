using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Videojuego_newCustomized) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Videojuego;

public sealed partial class VideojuegoUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Videojuego> NewCustomizedAsync(string p_texto, string p_titulo, string p_enlace, string p_imagen, int p_usuarioCreador_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Videojuego_newCustomized) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        VideojuegoService videojuegoService = null!;
        global::WikiGames.Domain.Entities.Videojuego result = default(global::WikiGames.Domain.Entities.Videojuego)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("VideojuegoUseCase.NewCustomizedAsync");
            videojuegoService = new VideojuegoService(_unitOfWork);

            if (string.IsNullOrWhiteSpace(p_titulo))
            {
                throw new ModelException("El título es obligatorio");
            }

            global::WikiGames.Domain.Entities.Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
                ?? throw new ModelException("Usuario not found for Videojuego.UsuarioCreador.");

            global::WikiGames.Domain.Entities.Videojuego entity = new global::WikiGames.Domain.Entities.Videojuego
            {
                Texto = p_texto,
                FechaCreacion = DateTime.Now,
                NumeroMeGusta = 0,
                Titulo = p_titulo,
                Enlace = p_enlace,
                Imagen = p_imagen,
                Estado = EstadoContenido.Pendiente,
                PuntuacionMedia = 0f,
                UsuarioCreador = usuarioCreadorEntity,
            };

            await _unitOfWork.VideojuegoRepository.SaveAsync(entity);
            result = entity;

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"Tu videojuego \"{entity.Titulo}\" se ha creado correctamente. Ahora tiene que esperar a que un moderador lo apruebe.",
                DateTime.Now,
                new[] { usuarioCreadorEntity.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Videojuego.newCustomized.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
