using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Contenido_newCustomized) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class ContenidoService
{
    public async Task<Contenido> NewCustomizedAsync(string p_texto, DateTime p_fechaCreacion, int p_usuarioCreador_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Contenido_newCustomized) ENABLED START*/
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Contenido.UsuarioCreador.");

        Contenido entity = new Contenido
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            UsuarioCreador = usuarioCreadorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
