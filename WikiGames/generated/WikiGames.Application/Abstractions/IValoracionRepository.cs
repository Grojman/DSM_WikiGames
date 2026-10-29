using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IValoracionRepository : IRepository<Valoracion>
{
    Task<IReadOnlyList<Valoracion>> FindByPuntuacionAsync(string p_texto);
    Task<IReadOnlyList<Valoracion>> FindPeoresAsync(string p_texto);
    Task<IReadOnlyList<Valoracion>> FindMejoresAsync(string p_texto);
}
