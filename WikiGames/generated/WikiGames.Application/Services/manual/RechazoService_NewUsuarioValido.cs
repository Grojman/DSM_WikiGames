using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Rechazo_newUsuarioValido) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class RechazoService
{
    public async Task<Rechazo> NewUsuarioValidoAsync(DateTime p_fecha, string p_motivo, int p_videojuego_oid, int p_gestor_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Rechazo_newUsuarioValido) ENABLED START*/
        Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
            ?? throw new ModelException("Videojuego not found for Rechazo.Videojuego.");

        Usuario gestorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_gestor_oid)
            ?? throw new ModelException("Usuario not found for Rechazo.Gestor.");

        Rechazo entity = new Rechazo
        {
            Fecha = p_fecha,
            Motivo = p_motivo,
            Videojuego = videojuegoEntity,
            Gestor = gestorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
