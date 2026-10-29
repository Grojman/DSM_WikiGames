using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Contenido_retirarMeGusta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class ContenidoService
{
    public async Task RetirarMeGustaAsync(int p_oid, int p_user)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Contenido_retirarMeGusta) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Contenido.retirarMeGusta is pending.", new NotImplementedException("Method RetirarMeGustaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
