using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Abstractions;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<IReadOnlyList<Usuario>> FindByNameLinQAsync(string p_name);
    Task<IReadOnlyList<Usuario>> FindByTipoUsuarioAsync(Rol p_rol);
}
