using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using WikiGames.Application.Abstractions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;
using WikiGames.Domain.ValueObjects;
using WikiGames.Infrastructure.Exceptions;

namespace WikiGames.Infrastructure.Repositories;

public sealed class ValoracionRepository : RepositoryBase<Valoracion>, IValoracionRepository
{
    public ValoracionRepository(ILogger<ValoracionRepository>? logger = null)
        : base(logger)
    {
    }

    public ValoracionRepository(ISession session, ILogger<ValoracionRepository>? logger = null)
        : base(session, logger)
    {
    }

    public ValoracionRepository(ISessionFactory sessionFactory, ILogger<ValoracionRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<Valoracion>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Valoracion>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<Valoracion?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<Valoracion>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(Valoracion entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(Valoracion entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }


}