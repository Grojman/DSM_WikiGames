using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using WikiGames.Application.Abstractions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;
using WikiGames.Domain.ValueObjects;
using WikiGames.Infrastructure.Exceptions;

namespace WikiGames.Infrastructure.Repositories;

public sealed class VideojuegoRepository : RepositoryBase<Videojuego>, IVideojuegoRepository
{
    public VideojuegoRepository(ILogger<VideojuegoRepository>? logger = null)
        : base(logger)
    {
    }

    public VideojuegoRepository(ISession session, ILogger<VideojuegoRepository>? logger = null)
        : base(session, logger)
    {
    }

    public VideojuegoRepository(ISessionFactory sessionFactory, ILogger<VideojuegoRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<Videojuego>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Videojuego>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<Videojuego?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<Videojuego>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(Videojuego entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(Videojuego entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }

    public async Task<IReadOnlyList<Videojuego>> FindByNombreAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Videojuego>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter Videojuego.findByNombre.");
            throw new DataException("Error executing ReadFilter Videojuego.findByNombre.", ex);
        }
    }

    public async Task<IReadOnlyList<Videojuego>> FindByCategoriaAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Videojuego>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter Videojuego.findByCategoria.");
            throw new DataException("Error executing ReadFilter Videojuego.findByCategoria.", ex);
        }
    }

    public async Task<IReadOnlyList<Videojuego>> FindRecientesAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Videojuego>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter Videojuego.findRecientes.");
            throw new DataException("Error executing ReadFilter Videojuego.findRecientes.", ex);
        }
    }

    public async Task<IReadOnlyList<Videojuego>> FindAntiguosAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Videojuego>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter Videojuego.findAntiguos.");
            throw new DataException("Error executing ReadFilter Videojuego.findAntiguos.", ex);
        }
    }

    public async Task<IReadOnlyList<Videojuego>> FindPopularesAsync(string p_texto)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Videojuego>()
                .Where(entity => entity.Texto == p_texto)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error executing ReadFilter Videojuego.findPopulares.");
            throw new DataException("Error executing ReadFilter Videojuego.findPopulares.", ex);
        }
    }
}