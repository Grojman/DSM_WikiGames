using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Contenido_darMeGusta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class ContenidoService
{
    public async Task DarMeGustaAsync(int p_oid, int p_user)
    {
        Usuario userEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_user)
        ?? throw new ModelException("Usuario not found for Contenido.Usuario.");
        
    }
}
