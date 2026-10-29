using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Valoracion_newValorValidado) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class ValoracionService
{
    public async Task<Valoracion> NewValorValidadoAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, float p_puntuacion, int p_videojuego_oid, int p_usuarioCreador_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Valoracion_newValorValidado) ENABLED START*/
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Valoracion.UsuarioCreador.");

        Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
            ?? throw new ModelException("Videojuego not found for Valoracion.Videojuego.");

        Valoracion entity = new Valoracion
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            NumeroMeGusta = p_numeroMeGusta,
            Puntuacion = p_puntuacion,
            UsuarioCreador = usuarioCreadorEntity,
            Videojuego = videojuegoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
