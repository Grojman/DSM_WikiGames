using WikiGames.Application.Abstractions;
using WikiGames.Infrastructure.Utils;

namespace WikiGames.Infrastructure.Security;

public sealed class HariaPasswordHasher : IHariaPasswordHasher
{
    public string HashPasswordIfNeeded(string password) =>
        HariaPasswordHashingUtils.HashPasswordIfNeeded(password);

    public string HashPassword(string password) =>
        HariaPasswordHashingUtils.HashPassword(password);

    public bool VerifyPassword(string? storedHash, string? password) =>
        HariaPasswordHashingUtils.VerifyPassword(storedHash, password);
}