using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Contenido_retirarMeGusta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class ContenidoService
{
    public async Task RetirarMeGustaAsync(int p_oid, int p_user)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Contenido_retirarMeGusta) ENABLED START*/
        Contenido entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException($"No existe el contenido {p_oid}.");

        Usuario usuario = await _unitOfWork.UsuarioRepository.ReadAsync(p_user)
            ?? throw new ModelException($"No existe el usuario {p_user}.");

        if (!entity.UsuariosMeGusta.Any(u => u.Id == usuario.Id))
        {
            throw new ModelException("El usuario no había dado me gusta a este contenido.");
        }

        // Unrelationer Contenido.quitarUsuarioMeGusta.
        await QuitarUsuarioMeGustaAsync(p_oid, new[] { usuario.Id });

        entity.NumeroMeGusta = Math.Max(0, entity.NumeroMeGusta - 1);
        await _repository.SaveAsync(entity);
        /*PROTECTED REGION END*/
    }
}
