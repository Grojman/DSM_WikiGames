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
        var denuncia = await _repository.ReadAsync(p_oid);

        if (denuncia is null)
        {
            throw new ModelException($"Denuncia with id {p_oid} not found.");
        }

        if (denuncia.Estado != EstadoContenido.Pendiente)
        {
            throw new ModelException($"Denuncia with id {p_oid} is not in a pending state.");
        }

        denuncia.Estado = EstadoContenido.Rechazado;

        await _repository.SaveAsync(denuncia);
    }
}
