using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface INotificacionRepository : IRepository<Notificacion>
{
    Task<IReadOnlyList<Notificacion>> FindByFechaAsync(DateTime p_fecha);
}
