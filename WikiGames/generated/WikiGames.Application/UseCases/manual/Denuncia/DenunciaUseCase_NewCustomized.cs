using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Denuncia_newCustomized) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Denuncia;

public sealed partial class DenunciaUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Denuncia> NewCustomizedAsync(int p_denunciante_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Denuncia_newCustomized) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        DenunciaService denunciaService = null!;
        global::WikiGames.Domain.Entities.Denuncia result = default(global::WikiGames.Domain.Entities.Denuncia)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("DenunciaUseCase.NewCustomizedAsync");
            denunciaService = new DenunciaService(_unitOfWork);

            global::WikiGames.Domain.Entities.Usuario denuncianteEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_denunciante_oid)
                ?? throw new ModelException("Usuario not found for Denuncia.Denunciante.");

            global::WikiGames.Domain.Entities.Denuncia entity = new global::WikiGames.Domain.Entities.Denuncia
            {
                Fecha = DateTime.Now,
                Estado = EstadoContenido.Pendiente,
                Denunciante = denuncianteEntity,
            };

            await _unitOfWork.DenunciaRepository.SaveAsync(entity);
            result = entity;

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                "Hemos recibido tu denuncia. Un moderador la revisará.",
                DateTime.Now,
                new[] { denuncianteEntity.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Denuncia.newCustomized.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
