using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Usuario_destroy) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Usuario;

public sealed partial class UsuarioUseCase
{
    public async Task DestroyAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Usuario_destroy) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        UsuarioService usuarioService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("UsuarioUseCase.DestroyAsync");
            usuarioService = new UsuarioService(_unitOfWork);

            global::WikiGames.Domain.Entities.Usuario entity = await _unitOfWork.UsuarioRepository.ReadAsync(p_oid)
                ?? throw new ModelException("Usuario not found.");

            // La notificación se crea ANTES de borrar el usuario (requisito): sirve para enviarle el correo.
            // Al borrarse el usuario la notificación queda huérfana (aceptado).
            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"Hola {entity.Name}, tu cuenta de WikiGames ha sido eliminada.",
                DateTime.Now,
                new[] { entity.Id });

            await _unitOfWork.UsuarioRepository.DeleteAsync(entity);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Usuario.destroy.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
