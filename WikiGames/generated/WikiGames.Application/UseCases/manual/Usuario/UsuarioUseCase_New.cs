using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Usuario_new) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Usuario;

public sealed partial class UsuarioUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Usuario> NewAsync(string p_name, string p_email, string p_password, global::WikiGames.Domain.Enums.Rol p_rolUsuario)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Usuario_new) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        UsuarioService usuarioService = null!;
        global::WikiGames.Domain.Entities.Usuario result = default(global::WikiGames.Domain.Entities.Usuario)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("UsuarioUseCase.NewAsync");
            usuarioService = new UsuarioService(_unitOfWork);

            if (string.IsNullOrWhiteSpace(p_name) || string.IsNullOrWhiteSpace(p_email) || string.IsNullOrWhiteSpace(p_password))
            {
                throw new ModelException("El nombre, el email y la contraseña son obligatorios.");
            }

            global::WikiGames.Domain.Entities.Usuario entity = new global::WikiGames.Domain.Entities.Usuario
            {
                Name = p_name,
                Email = p_email,
                Password = _unitOfWork.PasswordHasher.HashPasswordIfNeeded(p_password),
                RolUsuario = p_rolUsuario,
            };

            await _unitOfWork.UsuarioRepository.SaveAsync(entity);
            result = entity;

            // Notificación de bienvenida (al final de la transacción).
            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"¡Bienvenido/a a WikiGames, {entity.Name}! Ya formas parte del foro.",
                DateTime.Now,
                new[] { entity.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Usuario.new.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
