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
        Denuncia denuncia = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException($"No existe la denuncia {p_oid}.");

        if (denuncia.Estado != EstadoContenido.Pendiente)
        {
            throw new ModelException($"La denuncia {p_oid} no está pendiente.");
        }

        denuncia.Estado = EstadoContenido.Rechazado;
        await _repository.SaveAsync(denuncia);
        /*PROTECTED REGION END*/
    }
}
