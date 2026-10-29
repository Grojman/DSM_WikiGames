using WikiGames.Domain.Entities;

namespace WikiGames.Application.Abstractions;

public interface ITemaConversacionRepository : IRepository<TemaConversacion>
{
    Task<IReadOnlyList<TemaConversacion>> FindByTituloAsync(string p_texto);
    Task<IReadOnlyList<TemaConversacion>> FindByRecientesAsync(string p_texto);
}
