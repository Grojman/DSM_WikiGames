using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Comentario_addRespuestaTransaction) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Comentario;

public sealed partial class ComentarioUseCase
{
    public async Task AddRespuestaTransactionAsync(int p_oid, IReadOnlyList<int> p_comentario2_oids)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Comentario_addRespuestaTransaction) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ComentarioService comentarioService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ComentarioUseCase.AddRespuestaTransactionAsync");
            comentarioService = new ComentarioService(_unitOfWork);

            // Write here your custom transaction code...
            // Use comentarioService, _unitOfWork.ComentarioRepository or _utils!.GetRequiredService<TService>() as the generated CP equivalent.

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Comentario.addRespuestaTransaction.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
