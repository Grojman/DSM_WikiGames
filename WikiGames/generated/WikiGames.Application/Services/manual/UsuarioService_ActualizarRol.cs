using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Usuario_actualizarRol) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class UsuarioService
{
    public async Task ActualizarRolAsync(int p_oid, int p_oid_admin, Rol nuevo_rol)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Usuario_actualizarRol) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Usuario.actualizarRol is pending.", new NotImplementedException("Method ActualizarRolAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
