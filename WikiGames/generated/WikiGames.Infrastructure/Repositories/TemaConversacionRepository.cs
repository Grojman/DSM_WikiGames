using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using WikiGames.Application.Abstractions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;
using WikiGames.Domain.ValueObjects;
using WikiGames.Infrastructure.Exceptions;

namespace WikiGames.Infrastructure.Repositories;

public sealed class TemaConversacionRepository : RepositoryBase<TemaConversacion>, ITemaConversacionRepository
{
    public TemaConversacionRepository(ILogger<TemaConversacionRepository>? logger = null)
        : base(logger)
    {
    }

    public TemaConversacionRepository(ISession session, ILogger<TemaConversacionRepository>? logger = null)
        : base(session, logger)
    {
    }

    public TemaConversacionRepository(ISessionFactory sessionFactory, ILogger<TemaConversacionRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<TemaConversacion>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<TemaConversacion>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<TemaConversacion?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<TemaConversacion>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(TemaConversacion entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(TemaConversacion entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }

    public async Task<IReadOnlyList<TemaConversacion>> FindByTituloAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<TemaConversacion>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter TemaConversacion.findByTitulo.");
            throw new DataException("Error executing ReadFilter TemaConversacion.findByTitulo.", ex);
        }
    }

    public async Task<IReadOnlyList<TemaConversacion>> FindByRecientesAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<TemaConversacion>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter TemaConversacion.findByRecientes.");
            throw new DataException("Error executing ReadFilter TemaConversacion.findByRecientes.", ex);
        }
    }
}