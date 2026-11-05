using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Denuncia_rechazarDenuncia) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class DenunciaService
{
    public async Task RechazarDenunciaAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Denuncia_rechazarDenuncia) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Denuncia.rechazarDenuncia is pending.", new NotImplementedException("Method RechazarDenunciaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
