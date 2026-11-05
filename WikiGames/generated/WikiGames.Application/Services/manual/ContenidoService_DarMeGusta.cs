using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Contenido_darMeGusta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class ContenidoService
{
    public async Task DarMeGustaAsync(int p_oid, int p_user)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Contenido_darMeGusta) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Contenido.darMeGusta is pending.", new NotImplementedException("Method DarMeGustaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
