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
    public async Task<Contenido> NewCustomizedAsync(string p_texto, int p_usuarioCreador_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Contenido_newCustomized) ENABLED START*/
        if (string.IsNullOrWhiteSpace(p_texto))
        {
            throw new ModelException("El contenido debe tener texto.");
        }

        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Contenido.UsuarioCreador.");

        Contenido entity = new Contenido
        {
            Texto = p_texto,
            FechaCreacion = DateTime.Now,
            NumeroMeGusta = 0,
            UsuarioCreador = usuarioCreadorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
