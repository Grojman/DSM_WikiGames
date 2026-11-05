using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Videojuego_rechazarVideojuego) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Videojuego;

public sealed partial class VideojuegoUseCase
{
    public async Task RechazarVideojuegoAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Videojuego_rechazarVideojuego) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        VideojuegoService videojuegoService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("VideojuegoUseCase.RechazarVideojuegoAsync");
            videojuegoService = new VideojuegoService(_unitOfWork);

            // Write here your custom transaction code...
            // Use videojuegoService, _unitOfWork.VideojuegoRepository or _utils!.GetRequiredService<TService>() as the generated CP equivalent.

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Videojuego.rechazarVideojuego.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
