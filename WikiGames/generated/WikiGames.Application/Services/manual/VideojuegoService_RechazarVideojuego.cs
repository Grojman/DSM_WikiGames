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
        /*PROTECTED REGION ID(WikiGames.Application.Services_Videojuego_rechazarVideojuego) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Videojuego.rechazarVideojuego is pending.", new NotImplementedException("Method RechazarVideojuegoAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
