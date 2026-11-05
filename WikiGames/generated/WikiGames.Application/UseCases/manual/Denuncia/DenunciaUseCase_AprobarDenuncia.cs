using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Denuncia_aprobarDenuncia) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Denuncia;

public sealed partial class DenunciaUseCase
{
    public async Task AprobarDenunciaAsync(int p_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Denuncia_aprobarDenuncia) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        DenunciaService denunciaService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("DenunciaUseCase.AprobarDenunciaAsync");
            denunciaService = new DenunciaService(_unitOfWork);

            // Write here your custom transaction code...
            // Use denunciaService, _unitOfWork.DenunciaRepository or _utils!.GetRequiredService<TService>() as the generated CP equivalent.

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Denuncia.aprobarDenuncia.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
