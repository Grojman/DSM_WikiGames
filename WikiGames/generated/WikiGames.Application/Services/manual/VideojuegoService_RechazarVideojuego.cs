using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Videojuego_rechazarVideojuego) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class VideojuegoService
{
    public async Task RechazarVideojuegoAsync(int p_oid)
    {
        var videojuego = await _repository.ReadAsync(p_oid);

        if (videojuego is null)
        {
            throw new ModelException($"Videojuego with id {p_oid} not found.");
        }

        if (videojuego.Estado != EstadoContenido.Pendiente)
        {
            throw new ModelException($"Videogame with id {p_oid} is not in a pending state.");
        }

        videojuego.Estado = EstadoContenido.Aprobado;

        await _repository.SaveAsync(videojuego);
    }
}
