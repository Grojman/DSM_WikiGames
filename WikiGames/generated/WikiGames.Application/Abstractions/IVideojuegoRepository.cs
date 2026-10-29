using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface IVideojuegoRepository : IRepository<Videojuego>
{
    Task<IReadOnlyList<Videojuego>> FindByNombreAsync(string p_texto);
    Task<IReadOnlyList<Videojuego>> FindByCategoriaAsync(string p_texto);
    Task<IReadOnlyList<Videojuego>> FindRecientesAsync(string p_texto);
    Task<IReadOnlyList<Videojuego>> FindAntiguosAsync(string p_texto);
    Task<IReadOnlyList<Videojuego>> FindPopularesAsync(string p_texto);
}
