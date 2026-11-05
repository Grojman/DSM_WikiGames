using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Notificacion_new) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Notificacion;

public sealed partial class NotificacionUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Notificacion> NewAsync(string p_texto, DateTime p_fecha)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Notificacion_new) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        NotificacionService notificacionService = null!;
        global::WikiGames.Domain.Entities.Notificacion result = default(global::WikiGames.Domain.Entities.Notificacion)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("NotificacionUseCase.NewAsync");
            notificacionService = new NotificacionService(_unitOfWork);

            global::WikiGames.Domain.Entities.Notificacion entity = new global::WikiGames.Domain.Entities.Notificacion
            {
                Texto = p_texto,
                Fecha = p_fecha,
            };

            await _unitOfWork.NotificacionRepository.SaveAsync(entity);
            result = entity;

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Notificacion.new.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
