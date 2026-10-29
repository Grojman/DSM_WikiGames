using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_AccionSistema_newUsuarioValido) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class AccionSistemaService
{
    public async Task<AccionSistema> NewUsuarioValidoAsync(DateTime p_fecha, string p_descripcion, int p_causante_oid, int p_contenidoAfectado_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_AccionSistema_newUsuarioValido) ENABLED START*/
        Usuario causanteEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_causante_oid)
            ?? throw new ModelException("Usuario not found for AccionSistema.Causante.");

        Contenido contenidoAfectadoEntity = await _unitOfWork.ContenidoRepository.ReadAsync(p_contenidoAfectado_oid)
            ?? throw new ModelException("Contenido not found for AccionSistema.ContenidoAfectado.");

        AccionSistema entity = new AccionSistema
        {
            Fecha = p_fecha,
            Descripcion = p_descripcion,
            Causante = causanteEntity,
            ContenidoAfectado = contenidoAfectadoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
