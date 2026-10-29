using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Valoracion_actualizarMediaVideojuego) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Valoracion;

public sealed partial class ValoracionUseCase
{
    public async Task ActualizarMediaVideojuegoAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Valoracion_actualizarMediaVideojuego) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ValoracionService valoracionService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ValoracionUseCase.ActualizarMediaVideojuegoAsync");
            valoracionService = new ValoracionService(_unitOfWork);

            // Write here your custom transaction code...
            // Use valoracionService, _unitOfWork.ValoracionRepository or _utils!.GetRequiredService<TService>() as the generated CP equivalent.

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Valoracion.actualizarMediaVideojuego.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
