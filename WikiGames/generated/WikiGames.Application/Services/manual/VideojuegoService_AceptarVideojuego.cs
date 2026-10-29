using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Videojuego_aceptarVideojuego) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class VideojuegoService
{
    public async Task AceptarVideojuegoAsync(int p_oid, int p_user_oid)
    {

        var usuario = await _unitOfWork.UsuarioRepository.ReadAsync(p_user_oid);

        var videojuego = await this._repository.ReadAsync(p_oid);

        if (videojuego is null || usuario is null)
        {
            throw new Exception();
        }

        if (usuario.RolUsuario == Rol.Registrado)
        {
            throw new Exception();
        }

        videojuego.Estado = EstadoContenido.Aprobado;

        await _repository.SaveAsync(videojuego);
        
    }
}
