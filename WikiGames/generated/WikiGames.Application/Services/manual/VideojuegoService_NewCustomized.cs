using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Videojuego_newCustomized) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class VideojuegoService
{
    public async Task<Videojuego> NewCustomizedAsync(string p_texto, DateTime p_fechaCreacion, string p_titulo, string p_enlace, string p_imagen, int p_usuarioCreador_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Videojuego_newCustomized) ENABLED START*/
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Videojuego.UsuarioCreador.");

        Videojuego entity = new Videojuego
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            Titulo = p_titulo,
            Enlace = p_enlace,
            Imagen = p_imagen,
            UsuarioCreador = usuarioCreadorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
