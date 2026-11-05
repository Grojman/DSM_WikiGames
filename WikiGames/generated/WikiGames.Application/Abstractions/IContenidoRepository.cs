using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IContenidoRepository : IRepository<Contenido>
{
    Task<IReadOnlyList<Contenido>> FindByTextoLinQAsync(string p_texto);
    Task<IReadOnlyList<Contenido>> FindOrderByFechaAsync();
    Task<IReadOnlyList<Contenido>> FindOrderMeGustaAsync();
    Task<IReadOnlyList<Contenido>> FindBYUserLikedAsync(int p_user);
}
