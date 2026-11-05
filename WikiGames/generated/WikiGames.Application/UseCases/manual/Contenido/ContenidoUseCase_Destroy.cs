using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Contenido_destroy) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Contenido;

public sealed partial class ContenidoUseCase
{
    public async Task DestroyAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Contenido_destroy) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ContenidoService contenidoService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ContenidoUseCase.DestroyAsync");
            contenidoService = new ContenidoService(_unitOfWork);

            global::WikiGames.Domain.Entities.Contenido entity = await _unitOfWork.ContenidoRepository.ReadAsync(p_oid)
                ?? throw new ModelException("Contenido not found.");
            await _unitOfWork.ContenidoRepository.DeleteAsync(entity);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Contenido.destroy.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
