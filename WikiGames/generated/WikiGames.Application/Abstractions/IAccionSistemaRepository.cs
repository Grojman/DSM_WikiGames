using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IAccionSistemaRepository : IRepository<AccionSistema>
{
    Task<IReadOnlyList<AccionSistema>> FindByFechaLinQAsync(DateTime p_fecha);
}
