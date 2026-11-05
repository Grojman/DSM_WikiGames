using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IVideojuegoRepository : IRepository<Videojuego>
{
    Task<IReadOnlyList<Videojuego>> FindByNombreAsync(string p_texto);
    Task<IReadOnlyList<Videojuego>> FindByCategoriaAsync(string p_texto);
    Task<IReadOnlyList<Videojuego>> FindRecientesAsync();
    Task<IReadOnlyList<Videojuego>> FindAntiguosAsync();
    Task<IReadOnlyList<Videojuego>> FindByMeGustaAsync();
    Task<IReadOnlyList<Videojuego>> FindPopularesAsync();
}
