using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IContenidoRepository : IRepository<Contenido>
{
    Task<IReadOnlyList<Contenido>> FindOrderByFechaAsync(string p_texto);
    Task<IReadOnlyList<Contenido>> FindOrderMeGustaAsync(string p_texto);
    Task<IReadOnlyList<Contenido>> FindBYUserLikedAsync(string p_texto);
}
