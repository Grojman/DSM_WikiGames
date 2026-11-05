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
        Contenido entity = await _repository.ReadAsync(p_oid)
        ?? throw new ModelException("Contenido not found.");

        Usuario userEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_user)
        ?? throw new ModelException("Usuario not found for Contenido.Usuario.");
        
         entity.UsuariosMeGusta ??= new List<Usuario>();

    if (!entity.UsuariosMeGusta.Any(u => u.Id == userEntity.Id))
    {
        entity.UsuariosMeGusta.Add(userEntity);
        entity.NumeroMeGusta++;

        await _repository.SaveAsync(entity);
    }
    }
}
