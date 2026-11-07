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

            global::WikiGames.Domain.Entities.Denuncia denuncia = await _unitOfWork.DenunciaRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe la denuncia {p_oid}.");

            if (denuncia.Estado != EstadoContenido.Pendiente)
            {
                throw new ModelException($"La denuncia {p_oid} no está pendiente.");
            }

            denuncia.Estado = EstadoContenido.Aprobado;
            await _unitOfWork.DenunciaRepository.SaveAsync(denuncia);

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"Tu denuncia del {denuncia.Fecha:dd/MM/yyyy} ha sido aceptada. Gracias por ayudar a mantener el foro.",
                DateTime.Now,
                new[] { denuncia.Denunciante.Id });

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
