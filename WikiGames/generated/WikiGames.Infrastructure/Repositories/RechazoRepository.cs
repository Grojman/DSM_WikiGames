using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using WikiGames.Application.Abstractions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;
using WikiGames.Domain.ValueObjects;
using WikiGames.Infrastructure.Exceptions;

namespace WikiGames.Infrastructure.Repositories;

public sealed class RechazoRepository : RepositoryBase<Rechazo>, IRechazoRepository
{
    public RechazoRepository(ILogger<RechazoRepository>? logger = null)
        : base(logger)
    {
    }

    public RechazoRepository(ISession session, ILogger<RechazoRepository>? logger = null)
        : base(session, logger)
    {
    }

    public RechazoRepository(ISessionFactory sessionFactory, ILogger<RechazoRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<Rechazo>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Rechazo>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<Rechazo?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<Rechazo>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(Rechazo entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(Rechazo entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }


}