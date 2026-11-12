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

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"Hola {entity.Name}, tu cuenta de WikiGames ha sido eliminada.",
                DateTime.Now,
                new[] { entity.Id });

            //Eliminamos las relaciones con el usuario para que las notificaciones sigan existiendo aun con el usuario eliminado
            foreach (global::WikiGames.Domain.Entities.Notificacion notificacion in entity.Notificaciones.ToList())
            {
                notificacion.Notificados.Remove(entity);
                await _unitOfWork.NotificacionRepository.SaveAsync(notificacion);
            }
            entity.Notificaciones.Clear();

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
