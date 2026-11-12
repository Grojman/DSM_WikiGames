using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IValoracionRepository : IRepository<Valoracion>
{
    Task<IReadOnlyList<Valoracion>> FindByPuntuacionAsync(float puntuacion);
    Task<IReadOnlyList<Valoracion>> FindPeoresAsync();
    Task<IReadOnlyList<Valoracion>> FindMejoresAsync();
}
