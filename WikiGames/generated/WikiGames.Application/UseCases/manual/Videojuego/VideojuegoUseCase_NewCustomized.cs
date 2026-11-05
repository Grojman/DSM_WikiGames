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
    public async Task<global::WikiGames.Domain.Entities.Videojuego> NewCustomizedAsync(string p_texto, DateTime p_fechaCreacion, string p_titulo, string p_enlace, string p_imagen, int p_usuarioCreador_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Videojuego_newCustomized) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        VideojuegoService videojuegoService = null!;
        global::WikiGames.Domain.Entities.Videojuego result = default(global::WikiGames.Domain.Entities.Videojuego)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("VideojuegoUseCase.NewCustomizedAsync");
            videojuegoService = new VideojuegoService(_unitOfWork);

            global::WikiGames.Domain.Entities.Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
                ?? throw new ModelException("Usuario not found for Videojuego.UsuarioCreador.");

            global::WikiGames.Domain.Entities.Videojuego entity = new global::WikiGames.Domain.Entities.Videojuego
            {
                Texto = p_texto,
                FechaCreacion = p_fechaCreacion,
                Titulo = p_titulo,
                Enlace = p_enlace,
                Imagen = p_imagen,
                UsuarioCreador = usuarioCreadorEntity,
            };

            await _unitOfWork.VideojuegoRepository.SaveAsync(entity);
            result = entity;

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
