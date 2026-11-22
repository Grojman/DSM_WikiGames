using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Usuario_actualizarRol) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Usuario;

public sealed partial class UsuarioUseCase
{
    /// <summary>
    /// Cambia el rol de un usuario entre Registrado y Moderador.
    /// Un Administrador no puede ser modificado, y nadie puede ser ascendido a Administrador.
    /// </summary>
    public async Task ActualizarRolAsync(int p_oid, global::WikiGames.Domain.Enums.Rol nuevo_rol)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Usuario_actualizarRol) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        try
        {
            await _unitOfWork.BeginTransactionAsync("UsuarioUseCase.ActualizarRolAsync");

            global::WikiGames.Domain.Entities.Usuario usuario = await _unitOfWork.UsuarioRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe el usuario {p_oid}.");

            if (usuario.RolUsuario == Rol.Administrador)
            {
                throw new ModelException($"El usuario {p_oid} es administrador y su rol no se puede cambiar.");
            }

            // Solo se puede pasar de Registrado a Moderador y viceversa.
            if (nuevo_rol is not (Rol.Registrado or Rol.Moderador))
            {
                throw new ModelException($"El rol {nuevo_rol} no se puede asignar a un usuario.");
            }

            usuario.RolUsuario = nuevo_rol;
            await _unitOfWork.UsuarioRepository.SaveAsync(usuario);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Usuario.actualizarRol.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
