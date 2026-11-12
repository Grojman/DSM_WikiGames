using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IReadOnlyList<Categoria>> FindByNameLinQAsync(string p_name);
}
