using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using WikiGames.Application.Abstractions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;
using WikiGames.Domain.ValueObjects;
using WikiGames.Infrastructure.Exceptions;

namespace WikiGames.Infrastructure.Repositories;

public sealed class ContenidoRepository : RepositoryBase<Contenido>, IContenidoRepository
{
    public ContenidoRepository(ILogger<ContenidoRepository>? logger = null)
        : base(logger)
    {
    }

    public ContenidoRepository(ISession session, ILogger<ContenidoRepository>? logger = null)
        : base(session, logger)
    {
    }

    public ContenidoRepository(ISessionFactory sessionFactory, ILogger<ContenidoRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<Contenido>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Contenido>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<Contenido?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<Contenido>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(Contenido entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(Contenido entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }


}